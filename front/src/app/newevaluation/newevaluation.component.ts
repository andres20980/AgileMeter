import { StorageDataService } from './../services/StorageDataService';
import { Component, OnInit } from '@angular/core';
import { SectionService } from '../services/SectionService';
import { RespuestasService } from '../services/RespuestasService';
import { AppComponent } from '../app.component';
import { Asignacion } from 'app/Models/Asignacion';
import { Pregunta } from 'app/Models/Pregunta';
import { Proyecto } from 'app/Models/Proyecto';
import { Respuesta } from 'app/Models/Respuesta';
import { AsignacionInfo } from 'app/Models/AsignacionInfo';
import { AsignacionUpdate } from 'app/Models/AsignacionUpdate';
import { Router } from "@angular/router";
import { Evaluacion } from 'app/Models/Evaluacion';
import { NgbModal, ModalDismissReasons, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Section } from 'app/Models/Section';
import { SectionInfo } from 'app/Models/SectionInfo';
import { SectionModify } from 'app/Models/SectionModify';
import { ProyectoService } from 'app/services/ProyectoService';
import { EvaluacionService } from 'app/services/EvaluacionService';
import { PreguntaInfo } from 'app/Models/PreguntaInfo';
import { BtnFinalizeEvaluationComponent } from 'app/btn-finalize-evaluation/btn-finalize-evaluation.component';

@Component({
  selector: 'app-newevaluation',
  templateUrl: './newevaluation.component.html',
  styleUrls: ['./newevaluation.component.scss'],
  providers: [SectionService, RespuestasService, ProyectoService, EvaluacionService]
})
export class NewevaluationComponent implements OnInit {
  public ListaAsignaciones: Array<Asignacion> = [];
  public InfoAsignacion: AsignacionInfo = { id: null, nombre: '', preguntas: null, 'notas': null };
  public NumMax: number = 0;
  public PageNow: number = 1;
  public Project: Proyecto = null;
  public Evaluation: Evaluacion = null;
  public AreaAsignada: Asignacion = { 'id': 0, 'nombre': "undefined" };
  public UserName: string = "";
  public SectionSelected: SectionInfo = null;
  public Deshabilitar = true;
  public ErrorMessage: string = null;
  public MostrarInfo = false;
  public textoModal: string;
  public anadeNota: string = null;
  public pagesArray: number[];
  public nextSection: SectionInfo = null;
  public prevSection: SectionInfo = null;
  public changedQuestion: number;
  public changedAnswer: number;
  modalOption: NgbModalOptions = {};

  //Recogemos todos los datos de la primera area segun su id y las colocamos en la lista
  constructor(
    private _sectionService: SectionService,
    private _respuestasService: RespuestasService,
    private _router: Router,
    private _appComponent: AppComponent,
    private modalService: NgbModal,
    private _proyectoService: ProyectoService,
    private _evaluacionService: EvaluacionService) {
    this.InitialiseComponent();

    this.modalOption.backdrop = 'static';
    this.modalOption.keyboard = false;
  }

  ngOnInit() {

    this.Evaluation = this._appComponent._storageDataService.Evaluacion;
  }

  private InitialiseComponent() {

    this.PageNow = 1;
    let allSections = this._appComponent._storageDataService.Sections;
    let lengthAllSections = this._appComponent._storageDataService.Sections.length
    this.SectionSelected = this._appComponent._storageDataService.SectionSelectedInfo;

  
    if(allSections != null && this.SectionSelected != null){
      let p = allSections.map(x => x['nombre']).indexOf(this.SectionSelected['nombre'])
      if(p < lengthAllSections  -1)  {
        this.nextSection = allSections[p + 1]
      } else {
        this.nextSection = null
      }

      if(p != 0)  {
        this.prevSection = allSections[p - 1]
      } else {
        this.prevSection = null
      }
    }
    
   // this.prevSection = this._appComponent._storageDataService.prevSection;
    this.AreaAsignada = this._appComponent._storageDataService.currentAssignation;

    //Recogemos el proyecto y el usuario si no coincide alguno lo redirigiremos
    this.Project = this._appComponent._storageDataService.UserProjectSelected;
    this.Evaluation = this._appComponent._storageDataService.Evaluacion;
    this._proyectoService.verificarUsuario();
    if (this._appComponent._storageDataService.UserData == undefined || this._appComponent._storageDataService.UserData == null) {
      this.UserName = this._proyectoService.UsuarioLogeado;
      if (this.UserName == undefined || this.UserName == null || this.UserName == "") {
        this._router.navigate(['/login']);
      }
      if (this.Project == null || this.Project == undefined || this.Evaluation == null || this.Evaluation == undefined) {
        this._router.navigate(['/home']);
      }
    } else {
      this.UserName = this._appComponent._storageDataService.UserData.nombre;
    }
    console.log("next section",this.nextSection)
    this.MostrarInfo = true;

    if (this.Evaluation != null) {
      //this._appComponent.pushBreadcrumb("Preguntas", "/evaluationquestions");
      this._appComponent.pushBreadcrumb("BREADCRUMB.QUESTIONS", "/evaluationquestions");
    }

    //Recoge todas las asignaciones de la section por id
    if (this.Evaluation != null && this.Evaluation != undefined && this.SectionSelected != null && this.SectionSelected != undefined) {
      this._sectionService.getAsignacionesSection(this.SectionSelected.id).subscribe(
        res => {
          if (res != null) {
            this.ListaAsignaciones = res;
            this.NumMax = this.ListaAsignaciones.length;

            let i = 0;
            this.pagesArray = [];

            while (this.NumMax > i) {
              i++;
              this.pagesArray.push(i);
            }

            //Se determina si es una nueva evaluacion o si se continua desde una asignacion pendiente
            if (this.AreaAsignada.id != 0) {
              //Se resetea el valor de la global
              this._appComponent._storageDataService.currentAssignation = { 'id': 0, 'nombre': "undefined" };
              this.PageNow = this.ListaAsignaciones.findIndex(x => x.id == this.AreaAsignada.id) + 1;
            } else {
              this.AreaAsignada = this.ListaAsignaciones[0]
            }

            this.getAsignacionActual(this.Evaluation.id, this.AreaAsignada.id);
            this.Deshabilitar = false;

          } else {
            this.ErrorMessage = "No se encontraron datos para esta sección.";
          }
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " No se pudo acceder a la información de esta evaluación.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
          setTimeout(() => { this.ErrorMessage = null }, 4000);
        }
      );
    } else {
      this._router.navigate(['/home']);
    }
  }

  //Establece los datos de la asignación indicada
  public getAsignacionActual(idEvaluacion, idAsignacion) {
    this._respuestasService.getRespuestasAsig(idEvaluacion, idAsignacion).subscribe(
      res => {
        if (res != null) {
          this.InfoAsignacion = res;

          this.Deshabilitar = false;
        } else {
          this.ErrorMessage = "No se encontraron datos para esta asignación.";
        }
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + "No se pudo acceder a los datos de esta asignación.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        setTimeout(() => { this.ErrorMessage = null }, 4000);
      }
    );
  }

  public redirectToNextSection() {
    this._appComponent._storageDataService.SectionSelectedInfo = this.nextSection;

    let index = this._appComponent._storageDataService.Sections.indexOf(this.nextSection);

    this._appComponent._storageDataService.nextSection = (index + 1) != this._appComponent._storageDataService.Sections.length
      ? this._appComponent._storageDataService.Sections[index + 1]
      : null;

    this._appComponent._storageDataService.prevSection = (index - 1) != -1
      ? this._appComponent._storageDataService.Sections[index - 1]
      : null;

    //console.log( this._appComponent._storageDataService.prevSection);
    window.scroll(0,0);
    this.InitialiseComponent();
  }

  public redirectToPrevSection() {
    this._appComponent._storageDataService.SectionSelectedInfo = this.prevSection;

    let index = this._appComponent._storageDataService.Sections.indexOf(this.prevSection);

    this._appComponent._storageDataService.prevSection = (index - 1) != -1
      ? this._appComponent._storageDataService.Sections[index - 1]
      : null;

    this._appComponent._storageDataService.nextSection = (index + 1) != this._appComponent._storageDataService.Sections.length
      ? this._appComponent._storageDataService.Sections[index + 1]
      : null;
      window.scroll(0,0);
    this.InitialiseComponent();
  }

  //Metodo que devuelve si una pregunta dependiente de otra habilitante se debe habilitar
  public checkHabilitante = (pregunta: PreguntaInfo): boolean => {
    let check: boolean = false;
    this.InfoAsignacion.preguntas.forEach(p => {
      if ((p.esHabilitante && !pregunta.esHabilitante && pregunta.preguntaHabilitanteId == p.id
        && p.respuesta.estado == 1) || pregunta.preguntaHabilitanteId == null) {
        check = true;
      }
    });
    return check;
  }

  public AnswerQuestion(pregunta: PreguntaInfo, index: number, optionAnswered: number) {
    var estadoOriginal = this.InfoAsignacion.preguntas[index].respuesta.estado;
    //Si la pregunta es Habilitante y se ha respondido NO
    if (this.InfoAsignacion.preguntas[index].esHabilitante && optionAnswered == 2) {
      this.InfoAsignacion.preguntas[index].respuesta.estado = optionAnswered;
      this._respuestasService.updateRespuestasAsig(this.Evaluation.id, pregunta.id).subscribe(
        res => {
          //Se actualizan las respuestas de las preguntas dependientes de esta pregunta habilitante a NC
          this.InfoAsignacion.preguntas.forEach(p => {
            if (this.InfoAsignacion.preguntas[index].id == p.preguntaHabilitanteId)
              p.respuesta.estado = 0;

            this.changedQuestion = index;
            this.changedAnswer = optionAnswered;
          });
        },
        error => {
          this.InfoAsignacion.preguntas[index].respuesta.estado = estadoOriginal;
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + "No se pudo acceder a los datos de esta asignación.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
          setTimeout(() => { this.ErrorMessage = null }, 4000);
        }
      );
    } else {
      if (optionAnswered != pregunta.respuesta.estado) {
        this.InfoAsignacion.preguntas[index].respuesta.estado = optionAnswered;
        let respuesta = this.InfoAsignacion.preguntas[index].respuesta;
        respuesta.userName = this.UserName;
        this._respuestasService.AlterRespuesta(respuesta).subscribe(
          res => {
            this.changedQuestion = index;
            this.changedAnswer = optionAnswered;
          },
          error => {
            this.InfoAsignacion.preguntas[index].respuesta.estado = estadoOriginal;
            if (error == 404) {
              this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización de la respuesta, lo sentimos.";
            } else if (error == 500) {
              this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
            } else if (error == 401) {
              this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
            } else {
              this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
            }
            setTimeout(() => { this.ErrorMessage = null }, 4000);
          });
      }
    }
  }

  //Al presionar el boton va avanzado y retrocediendo
  //0 finalizar
  //1 avanzar
  //2 retroceder
  public NextPreviousButton(Option: number) {
    this.anadeNota = null;

    if (Option == 1) {
      this.Deshabilitar = true;
      this.AreaAsignada = this.ListaAsignaciones[this.PageNow];
      this.getAsignacionActual(this.Evaluation.id, this.AreaAsignada.id);
      this.PageNow++;

    } else if (Option == 2) {
      this.PageNow--;
      this.Deshabilitar = true;
      this.AreaAsignada = this.ListaAsignaciones[this.PageNow - 1];
      this.getAsignacionActual(this.Evaluation.id, this.AreaAsignada.id);

    } else if (Option == 0) {
      this._router.navigate(['/menunuevaevaluacion']);
    }
    window.scroll(0,0);
  }

  //Para abrir las notas de preguntas
  public AbrirModalPreg(content, i) {

    this.anadeNota = null;

    if (this.InfoAsignacion.preguntas[i].respuesta.notas != null) {
      this.textoModal = this.InfoAsignacion.preguntas[i].respuesta.notas;
    } else {
      this.textoModal = "";
    }

    this.modalService.open(content, this.modalOption).result.then(
      (closeResult) => {
        //Si cierra, no se guarda

      }, (dismissReason) => {
        if (dismissReason == 'Guardar') {

          this.Deshabilitar = true;

          if (this.textoModal != "") {
            this.InfoAsignacion.preguntas[i].respuesta.notas = this.textoModal;
          } else {
            this.InfoAsignacion.preguntas[i].respuesta.notas = null;
          }

          var Respuesta = this.InfoAsignacion.preguntas[i].respuesta;
          Respuesta.userName = this.UserName; //le añadimos el usuario que hace la nota

          this._respuestasService.AlterRespuesta(Respuesta).subscribe(
            res => {

              // this.anadeNota = "Nota añadida correctamente";
              // setTimeout(() => { this.anadeNota = null }, 4000);
            },
            error => {
              this.InfoAsignacion.preguntas[i].respuesta.notas = null
              this.Deshabilitar = false;
              if (error == 404) {
                this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización de la respuesta, lo sentimos.";
              } else if (error == 500) {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              } else if (error == 401) {
                this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
              } else {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              }
              setTimeout(() => { this.ErrorMessage = null }, 4000);
            },
            () => {
              this.Deshabilitar = false;

            });
        }
        //Else, Click fuera, no se guarda
      })
  }

  //Para abrir las notas de asignaciones
  public AbrirModalAsig(content) {

    this.anadeNota = null;

    if (this.InfoAsignacion.notas != null) {
      this.textoModal = this.InfoAsignacion.notas;
    } else {
      this.textoModal = "";
    }

    this.modalService.open(content, this.modalOption).result.then(
      (closeResult) => {
        //Si cierra, no se guarda

      }, (dismissReason) => {
        if (dismissReason == 'Guardar') {

          this.Deshabilitar = true;

          if (this.textoModal != "") {
            this.InfoAsignacion.notas = this.textoModal;
          } else {
            this.InfoAsignacion.notas = null;
          }
          var asig = new AsignacionUpdate(this.Evaluation.id, this.InfoAsignacion.id, this.InfoAsignacion.notas);

          this._respuestasService.AddNotaAsig(asig).subscribe(
            res => {
              // this.anadeNota = "Nota añadida correctamente";
              // setTimeout(() => { this.anadeNota = null }, 4000);

              this.Evaluation.userNombre = this.UserName;
              this._evaluacionService.updateEvaluacion(this.Evaluation).subscribe(
                res => {
                  //usuario modificado correctamente                  
                },
                error => {
                  this.InfoAsignacion.notas = null;
                  this.Deshabilitar = false;
                  if (error == 404) {
                    this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización del usuario, lo sentimos.";
                  } else if (error == 500) {
                    this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
                  } else if (error == 401) {
                    this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
                  } else {
                    this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
                  }
                });
            },
            error => {
              this.InfoAsignacion.notas = null;
              this.Deshabilitar = false;
              if (error == 404) {
                this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización de la respuesta, lo sentimos.";
              } else if (error == 500) {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              } else if (error == 401) {
                this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
              } else {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              }
              setTimeout(() => { this.ErrorMessage = null }, 4000);
            },
            () => {
              this.Deshabilitar = false;

            });
        }
        //Else, Click fuera, no se guarda
      })
  }

  //Para abrir las notas de secciones
  public AbrirModalSec(content) {

    this.anadeNota = null;

    if (this.SectionSelected.notas != null) {
      this.textoModal = this.SectionSelected.notas;
    } else {
      this.textoModal = "";
    }

    this.modalService.open(content, this.modalOption).result.then(
      (closeResult) => {
        //Si cierra, no se guarda

      }, (dismissReason) => {
        if (dismissReason == 'Guardar') {

          this.Deshabilitar = true;

          if (this.textoModal != "") {
            this.SectionSelected.notas = this.textoModal;
          } else {
            this.SectionSelected.notas = null;
          }


          var SeccionModificada = new SectionModify(this.Evaluation.id, this.SectionSelected.id, this.SectionSelected.notas);

          this._sectionService.addNota(SeccionModificada).subscribe(
            res => {

              // this.anadeNota = "Nota añadida correctamente";
              // setTimeout(() => { this.anadeNota = null }, 4000);

              this.Evaluation.userNombre = this.UserName;
              this._evaluacionService.updateEvaluacion(this.Evaluation).subscribe(
                res => {
                  //usuario modificado correctamente
                },
                error => {
                  this.InfoAsignacion.notas = null;
                  this.Deshabilitar = false;
                  if (error == 404) {
                    this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización del usuario, lo sentimos.";
                  } else if (error == 500) {
                    this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
                  } else if (error == 401) {
                    this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
                  } else {
                    this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
                  }
                });

            },
            error => {
              this.SectionSelected.notas = null;
              this.Deshabilitar = false;
              if (error == 404) {
                this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización de la respuesta, lo sentimos.";
              } else if (error == 500) {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              } else if (error == 401) {
                this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
              } else {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              }
              setTimeout(() => { this.ErrorMessage = null }, 4000);
            },
            () => {
              this.Deshabilitar = false;

            });

        }
        //Else, Click fuera, no se guarda
      })
  }
}
