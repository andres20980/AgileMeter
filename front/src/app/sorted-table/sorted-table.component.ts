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
  displayedColumns = ['fecha', 'oficina', 'nombre', 'userNombre', 'assessmentName', 'puntuacion', 'notas', 'informe'];
  public rol: EnumRol = new EnumRol();
  public ListaDeOficinas: string[] = [];
  public OficinaSeleccionada: string[] = [];

  public EquipoSeleccionado: Proyecto[] = [];
  public ListaDeProyectos: Array<Proyecto> = [];
  public ListaDeProyectosFiltrada: Array<Proyecto> = [];

  public ListaDeAssessment: Array<Assessment> = [];
  public AssessmentSeleccionado: Assessment[] = [];
  public ListaDeAssessmentFiltrada: Array<Assessment> = [];

  ngOnInit() {
    this.GetPaginacion();
    this.getProyectos();
    this.getAssessmentDeUsuario();
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

    this.dataSource.filterPredicate = function (data, filter: string): boolean {
      let date = new Date(data.fecha);
      //console.log ((date.getDate()<10?"0":"")+date.getDate()+"/"+(date.getMonth()<10?"0":"")+(date.getMonth()+1)+"/"+date.getFullYear());
      return data.nombre.toLowerCase().includes(filter)
        || data.assessmentName.toLowerCase().includes(filter)
        || data.userNombre.toLowerCase().includes(filter)
        || data.puntuacion.toString().concat("%").includes(filter)
        || (data.notasEvaluacion != null && data.notasEvaluacion.toLowerCase().includes(filter))
        || (data.notasObjetivos != null && data.notasObjetivos.toLowerCase().includes(filter))
        || ((date.getDate() < 10 ? "0" : "") + date.getDate() + "/" + (date.getMonth() < 10 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getFullYear()).includes(filter)
        ;
    };
  }

  public parseDate(value: string): string {
    let date = new Date(value);
    console.log(date.getDay() + "/" + date.getMonth() + "/" + date.getFullYear());
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

  public getProyectos() {
    this._proyectoService.getProyectosDeUsuarioConEvaluacionesFinalizadas().subscribe(
      res => {
        this.ListaDeProyectos = res;
        this.getOficinasDeUsuario(res);
        this.ListaDeProyectosFiltrada = res;
        //añadimos la opcion de todas al principio del select de equipos
        this.ListaDeProyectosFiltrada.unshift({ id: -1, nombre: "TODAS", codigo: "", fecha: new Date(0, 0, 0), numFinishedEvals: 0, numPendingEvals: 0, oficina: "TODAS" });
        this.EquipoSeleccionado = this.ListaDeProyectos;// inicialmente marcamos todas 
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

  public refresh() {
    var o = this.OficinaSeleccionada;
    //si es TODAS dejamos el array vacio  --> pasamos menos datos y excluimos una comparacion en la consulta --> mas optima
    if (this.OficinaSeleccionada.length === 0 || this.OficinaSeleccionada[0] == "TODAS") {
      o = [];//o = this.ListaDeOficinas;
    }
    var e = [];
    if (this.EquipoSeleccionado.length === 0 || this.EquipoSeleccionado[0].nombre == "TODAS") {
      e = [];
    } else {
      this.EquipoSeleccionado.forEach(function (element) {
        e.push(element.id);
      });
    }
    var a = [];
    if (this.AssessmentSeleccionado.length === 0 || this.AssessmentSeleccionado[0].assessmentName == "TODAS") {
      a = [];
    } else {
      this.AssessmentSeleccionado.forEach(function (element) {
        a.push(element.assessmentId);
      });
    }

    this.dataSource.filter = "";
    this.prevEval.EvaluacionFiltrar = { 'nombre': '', 'estado': 'true', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': 0, 'oficinas': o, equipos: e, 'idAssessment': a };
    this.prevEval.GetPaginacion();
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
    this.ListaDeOficinas.unshift("TODAS");
    this.OficinaSeleccionada = this.ListaDeOficinas;// inicialmente marcamos todas las oficinas
  }

  public selectOficinas() {
    //si no estan marcadas todas las casillas se desmarca la opcion de todas
    if (this.OficinaSeleccionada[0] == "TODAS") {
      if (this.OficinaSeleccionada.length < this.ListaDeOficinas.length) {
        this.OficinaSeleccionada = this.OficinaSeleccionada.slice(1);
      }
    }
    //si estan marcadas todas las casillas menos la opcion de todas --> se marca la opcion de todas
    else {
      if (this.OficinaSeleccionada.length == (this.ListaDeOficinas.length - 1)) {
        this.OficinaSeleccionada = this.ListaDeOficinas;
      }
    }
    //Modificamos el select del equipo y assessment mostrando los de las oficinas seleccionadas
    this.equiposDeLasOficinasSeleccionadas();
    this.refresh();
  }
  public seleccionarTodasOficinas() {
    //marcamos / desmarcamos todas las opciones    
    if (this.OficinaSeleccionada[0] === "TODAS") {
      this.OficinaSeleccionada = this.ListaDeOficinas;
    } else {
      this.OficinaSeleccionada = [];
    }
    //Modificamos el select del equipo y assessment mostrando los de las oficinas seleccionadas
    this.equiposDeLasOficinasSeleccionadas();
    this.refresh();
  }

  // FUNCIONES SELECT DE EQUIPOS
  public selectEquipos() {
    //comprobamos que al menos 1 este seleccionado    
    if (this.EquipoSeleccionado.length > 0) {
      //si no estan marcadas todas las casillas se desmarca la opcion de todas
      if (this.EquipoSeleccionado[0].nombre == "TODAS") {
        if (this.EquipoSeleccionado.length < this.ListaDeProyectosFiltrada.length) {
          this.EquipoSeleccionado = this.EquipoSeleccionado.slice(1);
        }
      }
      //si estan marcadas todas las casillas menos la opcion de todas --> se marca la opcion de todas
      else {
        if (this.EquipoSeleccionado.length == (this.ListaDeProyectosFiltrada.length - 1)) {
          this.EquipoSeleccionado = this.ListaDeProyectos;
        }
      }
    }
    this.oficinasDeLosEquiposSeleccionados();
    this.refresh();
  }
  public seleccionarTodosEquipos() {
    //marcamos / desmarcamos todas las opciones    
    if (this.EquipoSeleccionado[0].nombre === "TODAS") {
      this.EquipoSeleccionado = this.ListaDeProyectos;
    } else {
      this.EquipoSeleccionado = [];
    }
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
      if (aux.indexOf(value.assessmentId) < 0) {
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
    //añadimos la opcion de todas al principio del select de assessment
    this.ListaDeAssessment.unshift({ assessmentId: -1, assessmentName: "TODAS" });
    this.AssessmentSeleccionado = this.ListaDeAssessment;// inicialmente marcamos todas 
    this.ListaDeAssessmentFiltrada = this.ListaDeAssessment;
  }

  public selectAssessment() {
    //comprobamos que al menos 1 este seleccionado
    if (this.AssessmentSeleccionado.length > 0) {
      //si no estan marcadas todas las casillas se desmarca la opcion de todas
      if (this.AssessmentSeleccionado[0].assessmentName == "TODAS") {
        if (this.AssessmentSeleccionado.length < this.ListaDeAssessment.length) {
          this.AssessmentSeleccionado = this.AssessmentSeleccionado.slice(1);
        }
      }
      //si estan marcadas todas las casillas menos la opcion de todas --> se marca la opcion de todas
      else {
        if (this.AssessmentSeleccionado.length == (this.ListaDeAssessment.length - 1)) {
          this.AssessmentSeleccionado = this.ListaDeAssessment;
        }
      }
    }
    this.refresh();
  }
  public seleccionarTodosAssessment() {
    //marcamos / desmarcamos todas las opciones    
    if (this.AssessmentSeleccionado[0].assessmentName === "TODAS") {
      this.AssessmentSeleccionado = this.ListaDeAssessment;
    } else {
      this.AssessmentSeleccionado = [];
    }
    this.refresh();
  }

  public equiposDeLasOficinasSeleccionadas() {
    //ponemos los equipos seleccionados y la lista de equipos a vacio
    this.EquipoSeleccionado = [];
    this.ListaDeProyectosFiltrada = [];

    if (this.OficinaSeleccionada.length === 0 || this.OficinaSeleccionada[0] === "TODAS") {
      this.ListaDeProyectosFiltrada = this.ListaDeProyectos;
    } else {
      this.ListaDeProyectosFiltrada = this.ListaDeProyectos.filter(x => x.oficina === "TODAS")
        .concat(this.ListaDeProyectos.filter(x => this.OficinaSeleccionada.indexOf(x.oficina) >= 0));
    }
    this.EquipoSeleccionado = this.ListaDeProyectosFiltrada;
  }

  public oficinasDeLosEquiposSeleccionados() {
    if (this.EquipoSeleccionado.length === 0 || this.EquipoSeleccionado[0].oficina === "TODAS") {
      if (this.EquipoSeleccionado.length !== 0 && this.EquipoSeleccionado[0].oficina === "TODAS") {
        var o = [];
        this.ListaDeProyectosFiltrada.forEach(function (element) {
          if (element.oficina != "TODAS") {
            o.push(element.oficina);
          }
        });
        this.OficinaSeleccionada = o;
      } else {
        this.OficinaSeleccionada = [];
      }
    } else {
      var oficinas = [];
      this.EquipoSeleccionado.forEach(function (value) {
        if (oficinas.indexOf(value.oficina) < 0) {
          oficinas.push(value.oficina);
        }
      });
      oficinas = oficinas.sort();
      this.OficinaSeleccionada = oficinas;
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.dataInput) {
      this.GetPaginacion();
    }
  }
}
