import { Component, OnInit, ViewChild, Input, ɵConsole, SimpleChanges } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { PreviousevaluationComponent } from 'app/previousevaluation/previousevaluation.component'
import { AppComponent } from 'app/app.component';
import { Evaluacion } from 'app/Models/Evaluacion';
import { EnumRol } from 'app/Models/EnumRol';
import { Assessment } from 'app/Models/Assessment';
import { Proyecto } from 'app/Models/Proyecto';
import { ProyectoService } from 'app/services/ProyectoService';
import { AssessmentEv } from 'app/Models/AssessmentEv';

// export interface Evaluacion {
//   id: number,
//   fecha: string;
//   nombre: string,
//   userNombre: string;
//   puntuacion: number;
//   estado: boolean;
//   notasEv: string;
//   notasOb: string;
//   assessmentName: string;
// }

@Component({
  selector: 'sorted-table',
  templateUrl: './sorted-table.component.html',
  styleUrls: ['./sorted-table.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
  ],
})

export class SortedTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @Input() dataInput: any;//Array<EvaluacionInfo>;
  dataSource: MatTableDataSource<Evaluacion>;
  userRole: number;
  expandedElement: Evaluacion;
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName', 'puntuacion', 'notas', 'informe'];
  public rol: EnumRol = new EnumRol();
  public ListaDeOficinas: string[] = [];
  public OficinaSeleccionada: string[] = [];

  public EquipoSeleccionado: Proyecto[] = [];
  public ListaDeProyectos: Array<Proyecto> = [];
  public ListaDeProyectosFiltrada: Array<Proyecto> = [];

  public ListaDeAssessment: Array<Assessment> = [];
  public AssessmentSeleccionado: Assessment[] = [];
  public ListaDeAssessmentFiltrada: Array<Assessment> = [];

  public fieldsTable : any[];
  public objectTranslate : string;

  ngOnInit() {

    //fieldsTable = [header, data, translate, size, formato, tipo]
    this.fieldsTable = [
        ["date", "fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
        ["user", "userNombre", "EXCEL_USER",20,"", "String"],
        ["office", "oficina", "EXCEL_OFFICE", 25,"", "String"],
        ["team", "nombre", "EXCEL_TEAM", 40,"", "String"], 
        ["assessment", "assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
        ["score", "puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];
    this.objectTranslate = "PREVIOUS_EVALUATION";

    if(this.prevEval.DatosSelectOficinas.length > 0 || this.prevEval.DatosSelectProyectos.length > 0)
    {
      this.ListaDeOficinas = this.prevEval.DatosSelectOficinas;
      this.ListaDeProyectosFiltrada = this.prevEval.DatosSelectProyectos;
      this.ListaDeProyectos = this.prevEval.ListaDeProyectos;
      this.prevEval.ListaAssessments.forEach(element => {
        let a: Assessment = { assessmentId: element.id, assessmentName: element.name };
        this.ListaDeAssessmentFiltrada.push(a);
      });

      this.refresh();
    }
    else
    {
      this.getProyectos();
      this.getAssessmentDeUsuario();
    }

    this.GetPaginacion();

  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
    this.prevEval.TableFilteredData = this.dataSource.filteredData;
  }

  public GetPaginacion() {
    this.dataSource = new MatTableDataSource(this.dataInput);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.userRole = this._appComponent._storageDataService.Role;
    this.prevEval.TableFilteredData = this.dataSource.filteredData;

    //Inicialmente asignamos el primer proyecto de la tabla sin filtrar
    if (this.dataSource.data.length > 0)
      this.prevEval.Project.id = this.dataSource.data[0].proyectoId;

    this.dataSource.filterPredicate = function (data, filter: string): boolean {
      let date = new Date(data.fecha);
      //console.log ((date.getDate()<10?"0":"")+date.getDate()+"/"+(date.getMonth()<10?"0":"")+(date.getMonth()+1)+"/"+date.getFullYear());
      return data.nombre.toLowerCase().includes(filter)
        || data.assessmentName.toLowerCase().includes(filter)
        || data.userNombre.toLowerCase().includes(filter)
        || data.oficina.toLowerCase().includes(filter)
        || data.nombre.toLowerCase().includes(filter)
        || data.puntuacion.toString().concat("%").includes(filter)
        || (data.notasEvaluacion != null && data.notasEvaluacion.toLowerCase().includes(filter))
        || (data.notasObjetivos != null && data.notasObjetivos.toLowerCase().includes(filter))
        || ((date.getDate() < 10 ? "0" : "") + date.getDate() + "/" + (date.getMonth() < 10 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getFullYear()).includes(filter)
        ;
    };
    
  }

  public parseDate(value: string): string {
    let date = new Date(value);
    //console.log(date.getDay() + "/" + date.getMonth() + "/" + date.getFullYear());
    return date.getDay() + "/" + date.getMonth() + 1 + "/" + date.getFullYear();
  }

  constructor(
    public prevEval: PreviousevaluationComponent,
    private _appComponent: AppComponent,
    private _proyectoService: ProyectoService
  ) {
  }

  SaveDataToPDF(evaluacion: EvaluacionInfo): void {
    this.prevEval.SaveDataToPDF(evaluacion);
  }

  saveNotas(model: Evaluacion): void {
    if (this.userRole == this.rol.Administrador || this.userRole == this.rol.Evaluador) {
      this.prevEval._evaluacionService.updateEvaluacion(model).subscribe(
        res => {
          // console.log("success");
        },
        error => {
          // console.log("error");
        },
        () => {
          // this.Mostrar = true;
        });
    }
  }
   //Metodo encargado de refrescar la tabla 
   public refresh() {
    var o = this.OficinaSeleccionada;

    var e = [];
    if (this.EquipoSeleccionado.length === 0) {
      e = [];
    } else {
      this.EquipoSeleccionado.forEach(function (element) {
        e.push(element.id);
      });
    }
    var a = [];
    if (this.AssessmentSeleccionado.length === 0) {
      a = [];
    } else {
      this.AssessmentSeleccionado.forEach(function (element) {
        a.push(element.assessmentId);
      });
    }
    this.dataSource.filter = "";
    this.prevEval.EvaluacionFiltrar = { 'nombre': '', 'estado': 'true', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': 1, 'oficinas': o, equipos: e, 'idAssessment': a };
    this.prevEval.GetPaginacion();
  }

  public getProyectos() {
    this._proyectoService.getProyectosDeUsuarioConEvaluacionesFinalizadas().subscribe(
      res => {
        this.ListaDeProyectos = res;
        this.getOficinasDeUsuario(res);
        this.ListaDeProyectosFiltrada = res;

        //Actualizamos los datos del componente padre
        this.prevEval.DatosSelectProyectos = this.ListaDeProyectosFiltrada;
        this.prevEval.ListaDeProyectos = this.ListaDeProyectos;
      },
      error => {
        //Si el servidor tiene algún tipo de problema mostraremos este error
        if (error == 404) {
          console.log("Error: " + error + " El usuario o proyecto autenticado no existe.");
        } else if (error == 500) {
          console.log("Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.");
        } else if (error == 401) {
          console.log("Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.");
        } else {
          console.log("Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.");
        }
      });
  }

  // FUNCIONES SELECT DE OFICINAS
  public getOficinasDeUsuario(res) {
    var oficinas = [];
    res.forEach(function (value) {
      if (oficinas.indexOf(value.oficina) < 0) {
        oficinas.push(value.oficina);
      }
    });
    this.ListaDeOficinas = oficinas.sort();
    this.prevEval.DatosSelectOficinas = this.ListaDeOficinas;
  }

  public selectOficinas() {
    //Limpiamos la lista de oficinas en storage y añadimos los nuevas oficinas seleccionadas
    this._appComponent._storageDataService.OfficesSelected = [];
    this._appComponent._storageDataService.ProjectsSelected = [];
      this.OficinaSeleccionada.forEach(element => {
        this._appComponent._storageDataService.OfficesSelected.push(element);
       });

    //console.log(this._appComponent._storageDataService.OfficesSelected);

    this.equiposDeLasOficinasSeleccionadas();
    this.refresh();
  }

  // FUNCIONES SELECT DE EQUIPOS
  public selectEquipos() {
    //Limpiamos la lista de equipos en storage y añadimos los nuevos equipos seleccionados
    this._appComponent._storageDataService.ProjectsSelected = [];
    this.EquipoSeleccionado.forEach(element => {
      this._appComponent._storageDataService.ProjectsSelected.push(element);
    });

    //console.log(this._appComponent._storageDataService.ProjectsSelected);

    //Refrescamos las oficinas
    this.oficinasDeLosEquiposSeleccionados();    
    this.refresh();
  }

  // FUNCIONES SELECT DE ASSESSMENT
  public getAssessmentDeUsuario() {
    var aux = []
    var assessment = [];
    var a: Assessment;
    //creamos la lista    
    this.prevEval.ListaDeEvaluacionesPaginada.forEach(function (value) {
      if (aux.indexOf(value.assessmentId) < 0 ) {
        a = { assessmentId: value.assessmentId, assessmentName: value.assessmentName };
        assessment.push(a);
        aux.push(value.assessmentId);
      }
    });
    //ordenamos la lista por nombre
    this.ListaDeAssessment = assessment.sort(function (a, b) {
      if (a.assessmentName > b.assessmentName) {
        return 1;
      }
      if (a.assessmentName < b.assessmentName) {
        return -1;
      }
      // a must be equal to b
      return 0;
    });
    this.ListaDeAssessmentFiltrada = this.ListaDeAssessment;
  }

  public selectAssessment() {

    //Limpiamos la lista de assessments en storage y añadimos los nuevas assessments seleccionadas
    this._appComponent._storageDataService.AssessmentsSelected = [];
      this.AssessmentSeleccionado.forEach(element => {
        this._appComponent._storageDataService.AssessmentsSelected.push(element);
       });

    //Refrescamos los datos para que sea coherente con el select de assessment no seleccionado.
    this.refresh();
  }

  public equiposDeLasOficinasSeleccionadas() {
    //ponemos los equipos seleccionados y la lista de equipos a vacio
    this.EquipoSeleccionado = [];
    this.ListaDeProyectosFiltrada = [];

    if (this.OficinaSeleccionada.length === 0) {
      this.ListaDeProyectosFiltrada = this.ListaDeProyectos;
    } else {
      this.ListaDeProyectosFiltrada = this.ListaDeProyectos.filter(x => this.OficinaSeleccionada.indexOf(x.oficina) >= 0);
    }

    if (this.ListaDeProyectosFiltrada.length === 1)
    {
      //Limpiamos la lista de equipos en storage y añadimos el único equipo
      this._appComponent._storageDataService.ProjectsSelected = [];
      this.ListaDeProyectosFiltrada.forEach(element => {
        this._appComponent._storageDataService.ProjectsSelected.push(element);
      });

      this.EquipoSeleccionado = this._appComponent._storageDataService.ProjectsSelected;

      //Actualizamos los datos del componente padre
      this.prevEval.DatosSelectProyectos = this.ListaDeProyectosFiltrada;

    //console.log(this._appComponent._storageDataService.ProjectsSelected);
      
    }
  }

  public oficinasDeLosEquiposSeleccionados() {
    if (this.EquipoSeleccionado.length != 0) {
      var oficinas = [];
      this.EquipoSeleccionado.forEach(function (value) {
        if (oficinas.indexOf(value.oficina) < 0) {
          oficinas.push(value.oficina);
        }
      });
      oficinas = oficinas.sort();
      this.OficinaSeleccionada = oficinas;

      //Actualizamos la lista de equipos asociados a la nueva lista de oficinas
      this.refrescamosEquiposOficinasSeleccionadas();

      //Limpiamos la lista de equipos en storage y añadimos los nuevos equipos seleccionados
      this._appComponent._storageDataService.OfficesSelected = [];
      this.OficinaSeleccionada.forEach(element => {
        this._appComponent._storageDataService.OfficesSelected.push(element);
       });

       //Marcamos los equipos previamnete seleccionados
       this.EquipoSeleccionado = this._appComponent._storageDataService.ProjectsSelected;

      //console.log(this._appComponent._storageDataService.OfficesSelected);
    }
  }


  //Este método refresca los equipos una vez que actualizamos las oficinas tras seleccionar
  //un equipo ya que solo debe mostrar los equipos de dicha oficina.
  public refrescamosEquiposOficinasSeleccionadas() {

    this.EquipoSeleccionado = [];
    this.ListaDeProyectosFiltrada = [];

    if (this.OficinaSeleccionada.length === 0) {
      this.ListaDeProyectosFiltrada = this.ListaDeProyectos;
    } else {
      this.ListaDeProyectosFiltrada = this.ListaDeProyectos.filter(x => this.OficinaSeleccionada.indexOf(x.oficina) >= 0);
    }

    if (this.ListaDeProyectosFiltrada.length === 1)
    {
      this.EquipoSeleccionado = this._appComponent._storageDataService.ProjectsSelected;
      //console.log(this._appComponent._storageDataService.ProjectsSelected);      
    }

    //Actualizamos los datos del componente padre
    //this.prevEval.DatosSelectProyectos = this.ListaDeProyectosFiltrada;
    this.prevEval.DatosSelectProyectos = this.ListaDeProyectosFiltrada;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.dataInput) {
      this.GetPaginacion();

      this.EquipoSeleccionado = this._appComponent._storageDataService.ProjectsSelected;
      this.OficinaSeleccionada = this._appComponent._storageDataService.OfficesSelected;

      //Por el momento lo dejamos comentado ya que aunque lo asignenmos no refresca el select de assessment
      //TODO
      //this.AssessmentSeleccionado = this._appComponent._storageDataService.AssessmentsSelected;
    }
  }
}
