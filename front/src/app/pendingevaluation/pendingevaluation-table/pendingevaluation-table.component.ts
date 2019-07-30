import { Component, OnInit, ViewChild, Input, SimpleChanges } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatCellDef } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Router } from "@angular/router";
import { AppComponent } from 'app/app.component';
import { EvaluacionInfoWithProgress } from 'app/Models/EvaluacionInfoWithProgress';
import { EvaluacionService } from '../../services/EvaluacionService';
import { SectionService } from 'app/services/SectionService';
import { AssignationService } from 'app/services/AssignationService';
import { Evaluacion } from 'app/Models/Evaluacion';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PendingEvaluationComponent } from '../pendingevaluation.component';
import { SectionInfo } from 'app/Models/SectionInfo';
import { DatePipe } from '@angular/common';
import { Proyecto } from 'app/Models/Proyecto';
import { Assessment } from 'app/Models/Assessment';
import { ProyectoService } from 'app/services/ProyectoService';



@Component({
  selector: 'pendingevaluation-table',
  templateUrl: './pendingevaluation-table.component.html',
  styleUrls: ['./pendingevaluation-table.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
  ],
})

export class PendingEvaluationTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @Input() ListaDeEvaluacionesPaginada: any;//Array<EvaluacionInfo>;
  public ErrorMessage: string = null;
  dataSource: MatTableDataSource<EvaluacionInfoWithProgress>;
  userRole: number;
  evaluationProgress: number;
  selectedEvaluacionInfoWithProgress;
  public ListaDeDatos: Array<SectionInfo> = [];
  public Evaluacion: Evaluacion = null;
  public sectionId: number;
  //expandedElement: Evaluacion;
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName', 'progress', 'actions'];

  public ListaDeOficinas: string[] = [];
  public OficinaSeleccionada: string[] = [];

  public EquipoSeleccionado: Proyecto[] = [];
  public ListaDeProyectos: Array<Proyecto> = [];
  public ListaDeProyectosFiltrada: Array<Proyecto> = [];

  public ListaDeAssessment: Array<Assessment> = [];
  public AssessmentSeleccionado: Assessment[] = [];
  public ListaDeAssessmentFiltrada: Array<Assessment> = [];


  constructor(
    private _evaluacionService: EvaluacionService,
    private _assignationService: AssignationService,
    private _sectionService: SectionService,
    private _router: Router,
    private _appComponent: AppComponent,
    private modalService: NgbModal,
    private parent: PendingEvaluationComponent,
    private _proyectoService: ProyectoService,
  ) {
  }

  ngOnInit() {
    this.LoadDataSource();
    this.getProyectos();
    this.getAssessmentDeUsuario();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.ListaDeEvaluacionesPaginada) {
      this.LoadDataSource();
    }
  }

  private LoadDataSource() {
    this.dataSource = new MatTableDataSource(this.ListaDeEvaluacionesPaginada);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.userRole = this._appComponent._storageDataService.Role;

    this.dataSource.filterPredicate = function (data, filter: string): boolean {
      let date = new Date(data.fecha);
      return data.nombre.toLowerCase().includes(filter)
        || data.assessmentName.toLowerCase().includes(filter)
        || (data.userNombre != null && data.userNombre.toLowerCase().includes(filter))
        || (data.progress != null && data.progress.toString().includes(filter))
        || ((date.getDate() < 10 ? "0" : "") + date.getDate() + "/" + (date.getMonth() < 10 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getFullYear()).includes(filter)
        ;
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  public parseDate(value: string): string {
    let date = new Date(value);
    return date.getDay() + "/" + date.getMonth() + 1 + "/" + date.getFullYear();
  }

  //Metodo encargado de establecer la información de la evaluacion en StorageData
  public ContinueEvaluation(evaluation: EvaluacionInfoWithProgress) {
    this._evaluacionService.getEvaluacion(evaluation.id).subscribe(
      res => {
        this._appComponent._storageDataService.Evaluacion = res;
        this._appComponent._storageDataService.Evaluacion.assessmentName = evaluation.assessmentName;
        this._appComponent._storageDataService.AssessmentSelected = { 'assessmentId': evaluation.assessmentId, 'assessmentName': evaluation.assessmentName };
        this.GetAssignation(evaluation.id);
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la evaluación solicitada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      }
    );
  }
  //Metodo encargado de refrescar la tabla 
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
    var a = 0;//[] ;
    this.parent.EvaluacionFiltrar ={ 'nombre': '', 'estado': 'false', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': a, 'oficinas':o, equipos:e};
    this.parent.GetPaginacion();

  }


  //Metodo encargado de eliminar la evaluacion pasandole una evaluacionId
  public EvaluationDelete(evaluationId: number) {
    this._evaluacionService.EvaluationDelete(evaluationId).subscribe(
      res => {
        this.refresh();
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo completar la actualización para esta evaluación.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      });
  }
  // Metodo encargado de abrir la ventana confirmando la eliminacion de la evaluacion
  public AbrirModal(content, row) {
    this.selectedEvaluacionInfoWithProgress = row;
    this.modalService.open(content).result.then(
      (closeResult) => {
        //Esto realiza la acción de cerrar la ventana
      }, (dismissReason) => {
        if (dismissReason == 'Finish') {
          //Si decide finalizarlo usaremos el metodo para finalizar la evaluación
          this.EvaluationDelete(row.id);
        }
      })
  }
  // Metodo encargado de establecer la información necesaria sobre las secciones en el StorageData y redirigir a la siguiente vista
  public GetAllSections(evaluationId: number, assessmentId: number) {
    this._sectionService.getSectionInfo(evaluationId, assessmentId).subscribe(
      res => {
        this.ListaDeDatos = res;
        this._appComponent._storageDataService.Sections = this.ListaDeDatos;
        this._appComponent._storageDataService.SectionSelectedInfo = this.ListaDeDatos.filter(x => x.id == this.sectionId)[0];

        //Se establece la siguiente sección validando si es la ultima
        let index = this.ListaDeDatos.indexOf(this._appComponent._storageDataService.SectionSelectedInfo);
        this._appComponent._storageDataService.nextSection = index != this.ListaDeDatos.length ? this.ListaDeDatos[index + 1] : null;
        this._appComponent._storageDataService.prevSection = index != -1 ? this.ListaDeDatos[index - 1] : null;

        //this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);
        this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.Evaluacion.assessmentName, null);
        var pipe = new DatePipe('en-US');
        this._appComponent.pushBreadcrumb(pipe.transform(this._appComponent._storageDataService.Evaluacion.fecha, 'dd/MM/yyyy'), null);
        //this._appComponent.pushBreadcrumb("Secciones", "/evaluationsections");
        this._appComponent.pushBreadcrumb("BREADCRUMB.SECTIONS", "/evaluationsections");

        this._router.navigate(['/evaluationquestions']);

      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No pudimos encontrar información de las secciones para esta evaluación.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      }
    );
  }

  //Metodo encargado de establecer la información de la asignacion en StorageData
  public GetAssignation(evaluationId: number) {
    this._assignationService.AssignationLastQuestionUpdated(evaluationId).subscribe(
      res => {
        this._appComponent._storageDataService.currentAssignation = res;
        this.sectionId = res.sectionId;
        this.GetAllSections(evaluationId, this._appComponent._storageDataService.AssessmentSelected.assessmentId);
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la asignación solicitada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      }
    );
  }

  public getProyectos() {
    this._proyectoService.getProyectosDeUsuarioConEvaluacionesPendientes().subscribe(
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
          this.ErrorMessage = "Error: " + error + " El usuario o proyecto autenticado no existe.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
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
    this.refresh();
    this.equiposDeLasOficinasSeleccionadas();
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
    this.parent.ListaDeEvaluacionesPaginada.forEach(function (value) {
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
  }
  public seleccionarTodosAssessment() {
    //marcamos / desmarcamos todas las opciones    
    if (this.AssessmentSeleccionado[0].assessmentName === "TODAS") {
      this.AssessmentSeleccionado = this.ListaDeAssessment;
    } else {
      this.AssessmentSeleccionado = [];
    }
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
}
