import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { EvaluacionService } from '../services/EvaluacionService';
import { Router } from "@angular/router";
import { Evaluacion } from 'app/Models/Evaluacion';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { SectionInfo } from 'app/Models/SectionInfo';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from '../app.component';
import { Proyecto } from 'app/Models/Proyecto';
import { DatePipe } from '@angular/common';
import { ProyectoService } from 'app/services/ProyectoService';

@Component({
  selector: 'app-btn-finalize-evaluation',
  templateUrl: './btn-finalize-evaluation.component.html',
  styleUrls: ['./btn-finalize-evaluation.component.scss'],
})

export class BtnFinalizeEvaluationComponent {
  @Input() evaluacion: Evaluacion;
  @Input() changedQuestion: number;
  @Input() changedAnswer: number;
  @Input() checkNoBinary: boolean;
  @Output() Click = new EventEmitter<any>();

  public ErrorMessage: string = null;
  public ProjectSelected: Proyecto;
  public UserSelected: string;
  public MostrarInfo = false;
  public textoModal: string;
  public anadeNota: string = null;
  public ScreenWidth;
  public cargar: boolean = false;
  public evaluationAnswered: boolean = false;

  constructor(
    private _router: Router,
    private _evaluacionService: EvaluacionService,
    private _appComponent: AppComponent,
    private modalService: NgbModal,
    private _proyectoService: ProyectoService) {
    this.evaluacion = this._appComponent._storageDataService.Evaluacion;
    this.ProjectSelected = this._appComponent._storageDataService.UserProjectSelected;
  }

  ngOnInit() {
    this.evaluationAnswered = this.EvaluationAnswered(this.evaluacion);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.changedQuestion || changes.changedAnswer) {
      this.evaluationAnswered = this.EvaluationAnswered(this.evaluacion);
    }
  }

  //Este metodo guarda la evaluacion y cambia su estado como finalizado
  public FinishEvaluation(evaluation: Evaluacion) {
    evaluation.userNombre = this._proyectoService.UsuarioLogeado;
    this.evaluacion.estado = true;
    this._evaluacionService.updateEvaluacion(evaluation, true).subscribe(
      res => {
        this.goToPdfGenerator(evaluation.id);
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
      }
    );
  }

  //Para abrir la advertencia de finalizar proyecto
  public AbrirModal(content) {
    this.modalService.open(content).result.then(
      (closeResult) => {
        //Esto realiza la acción de cerrar la ventana
      }, (dismissReason) => {
        if (dismissReason == 'Finish') {
          //Si decide finalizarlo usaremos el metodo para finalizar la evaluación
          this.FinishEvaluation(this.evaluacion);
        }
      })
  }

  //Metodo que devuelve si la evaluación ha sido respondida al 100%
  public EvaluationAnswered(evaluacion) {

    this.evaluationAnswered = false;
    this._evaluacionService.CalculateEvaluationProgress(this.evaluacion.id, this.evaluacion.assessmentId).subscribe(
      res => {
        if (res == 100)
          this.evaluationAnswered = true;
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
      }
    );

    return this.evaluationAnswered;
  }

  //Metodo encargado de guardar los datos en el storage y cambia de ruta hacia la generación de grafica
  public goToPdfGenerator(idEvaluation: number) {

    this._evaluacionService.GetEvaluationInfoFromIdEvaluation(idEvaluation).subscribe(
      res => {
        this._appComponent._storageDataService.EvaluacionToPDF = res;
        //TODO
        this._appComponent.popBreadcrumb(1);
        //this._appComponent.pushBreadcrumb("Evaluaciones finalizadas", "/finishedevaluations");
        this._appComponent.pushBreadcrumb("BREADCRUMB.FINISHED_EVALUATIONS", "/finishedevaluations");
        this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);
        this._appComponent.pushBreadcrumb(this.evaluacion.assessmentName, null);
        var pipe = new DatePipe('en-US');
        this._appComponent.pushBreadcrumb(pipe.transform(res.fecha, 'dd/MM/yyyy'), null);
        //this._appComponent.pushBreadcrumb("Resultados", "/evaluationresults");
        this._appComponent.pushBreadcrumb("BREADCRUMB.RESULTS", "/evaluationresults");

        this._router.navigate(['/evaluationresults']);
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
      }
    );
  }
}
