import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'app/Models/User';
import { Role } from 'app/Models/Role';
import { Proyecto } from 'app/Models/Proyecto';
import { ProyectoService } from '../services/ProyectoService';
import { EvaluacionService } from '../services/EvaluacionService';
import { AssignationService } from '../services/AssignationService';
import { Router } from "@angular/router";
import { AppComponent } from '../app.component';
import { Evaluacion } from 'app/Models/Evaluacion';
import { EvaluacionCreate } from 'app/Models/EvaluacionCreate';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { Assessment } from '../Models/Assessment';
import { BreadcrumbComponent } from 'app/breadcrumb/breadcrumb.component';
import { EnumRol } from 'app/Models/EnumRol';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ProyectoService, EvaluacionService, AssignationService]
})
export class HomeComponent implements OnInit {
  public ErrorMessage: string = null;
  public ListaDeProyectos: Array<Proyecto> = [];
  public AllAssessments: Assessment[] = [];
  public permisosDeUsuario: Role;
  public SeeAllProjects = false;
  public ProyectoSeleccionado: Proyecto;
  public AssessmentSelected: Assessment;
  public NombreDeUsuario: string;
  public Deshabilitar = false;
  public MostrarInfo = false;
  public SendingInfo = false;
  public existeRepetida = false;
  public fadeInError = false;
  public rol: EnumRol = new EnumRol();
  public evaluacionesPendientes: boolean = false;
  public evaluacionesFinalizadas: boolean = false;

  constructor(
    private _proyectoService: ProyectoService,
    private _evaluacionService: EvaluacionService,
    private modalService: NgbModal,
    private _router: Router,
    private _appComponent: AppComponent) { }

  ngOnInit() {
    //Empezamos cargando el usuario en el componente mientras verificamos si esta logueado
    //En casao de no estar logeado nos enviara devuelta al login
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    //console.log(this._proyectoService.UserLongName);

    this._appComponent.popBreadcrumb(0);
    this._appComponent.pushBreadcrumb("BREADCRUMB.HOME", "/home");
    //console.log(this._breadcrumb.breadcrumbList);

    this.getUserRole();

    //Recogemos el nombre del usuario con el que nos logueamos
    this.NombreDeUsuario = this._proyectoService.UsuarioLogeado;

    //Reiniciamos los proyectos seleccionados en el servicio
    this._appComponent._storageDataService.UserProjectSelected = { id: -1, nombre: '', codigo: null, fecha: null, numFinishedEvals: 0, numPendingEvals: 0, oficina:null};

    //Intentamos recoger los roles de los usuarios
    this._proyectoService.getRolesUsuario().subscribe(
      res => {
        this.permisosDeUsuario = res;
        //Si no hay errores y son recogidos busca si tienes permisos de usuario
        // for (let num = 0; num < this.permisosDeUsuario.length; num++) {
        //   console.log("1" + this.permisosDeUsuario);
        //   if (this.permisosDeUsuario[num].role == "Administrador" || this.permisosDeUsuario[num].role == "Evaluador") {
        //     this.AdminOn = true;
        //   }
        // }

        this.SeeAllProjects = this.permisosDeUsuario.role == "Administrador" || this.permisosDeUsuario.role == "Evaluador";
        //Llamamos al metodo para asignar proyectos
        this.MostrarInfo = true;
        this.RecogerProyectos();
        this.GetAssessments();

      },
      error => {
        //Si el servidor tiene algún tipo de problema mostraremos este error
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " El usuario autenticado no existe.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        setTimeout(() => {
          this.fadeInError = false;
          setTimeout(() => this.ErrorMessage = "", 900);
        }, 2000);
      });

    //Para que no de error en modo development
    setTimeout(() => {
      this._appComponent.anadirUserProyecto(this.NombreDeUsuario, this._proyectoService.UserLongName, null);
    });

  }


  public GetAssessments(): any {
    this._proyectoService.getAllAssessments().subscribe(
      res => { this.AllAssessments = res; }
    )
  }

  //Metodo que asigna los proyectos por permisos y usuario
  public RecogerProyectos() {

    //Segun el tipo de rol que tengas te permitira tener todos los proyectos o solo los tuyos
    //El servicio se encargara de enviar una respuesta con el listado de proyecto
    //El usuario necesario ya tendria que haber sido cargado en el logueo
    if (!this.SeeAllProjects) {
      //Aqui se entra solo si no tienes permisos de administrador dandote los proyectos que te tocan
      this._proyectoService.getProyectosDeUsuario().subscribe(
        res => {
          this.ListaDeProyectos = res;
          this.comprobarEvaluaciones();
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
          setTimeout(() => {
            this.fadeInError = false;
            setTimeout(() => this.ErrorMessage = "", 900);
          }, 2000);
        });
    } else {
      //Aqui entra si eres administrador dandote todos los proyectos
      this._proyectoService.getProyectosDeUsuario().subscribe(
        res => {
          this.ListaDeProyectos = res;
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
          setTimeout(() => {
            this.fadeInError = false;
            setTimeout(() => this.ErrorMessage = "", 900);
          }, 2000);
        });
    }
  }

  public comprobarEvaluaciones() {
    var evalPendientes = this.ListaDeProyectos.filter(p => p.numPendingEvals > 0);
    if (evalPendientes.length > 0) {
      this.evaluacionesPendientes = true;
    } else {
      this.evaluacionesPendientes = false;
    }

    var evalFinalizadas = this.ListaDeProyectos.filter(p => p.numFinishedEvals > 0);
    if (evalFinalizadas.length > 0) {
      this.evaluacionesFinalizadas = true;
    } else {
      this.evaluacionesFinalizadas = false;
    }
  }
/* Eliminado en el nuevo home  
  //Este metodo guarda el proyecto que a sido seleccionado en el front
  public SeleccionDeProyecto() {
    //this.ProyectoSeleccionado = this.ListaDeProyectos[index];
    this._appComponent._storageDataService.UserProjectSelected = this.ProyectoSeleccionado;
    this.existeRepetida = false;


    //Comprueba que no esta vacia el proyecto elegido
    if (this.checkIfIsSet(this.ProyectoSeleccionado) && this.checkIfIsSet(this.AssessmentSelected)) {
      //Comprueba si ya termino de enviarse la información desde la api
      if (!this.SendingInfo) {
        this.SendingInfo = true;
        this._evaluacionService.getIncompleteEvaluacionFromProjectAndAssessment(this.ProyectoSeleccionado.id, this.AssessmentSelected.assessmentId).subscribe(
          res => {
            //Lo guarda en el storage
            this._appComponent._storageDataService.Evaluacion = res;
            //Si hay un proyecto sin finalizar
            if (res != null) {
              this.existeRepetida = true;
            }
          },
          error => {
            //Habilitamos la pagina nuevamente
            this.Deshabilitar = false;
            if (error == 404) {
              this.ErrorMessage = "Error: " + error + " No se puede completar la comprobación en la evaluación lo sentimos.";
            } else if (error == 500) {
              this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
            } else if (error == 401) {
              this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
            } else {
              this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
            }
            setTimeout(() => {
              this.fadeInError = false;
              setTimeout(() => this.ErrorMessage = "", 900);
            }, 2000);
          },
          () => {
            this.SendingInfo = false;
          });
      }
    }
  }
  */

  public SeleccionDeAssessment() {
    // console.log("assessment",index);

    // this.AssessmentSelected = this.AllAssessments[index];
    // console.log(this.AssessmentSelected);

    this._appComponent._storageDataService.AssessmentSelected = this.AssessmentSelected;
    this.existeRepetida = false;


    //Comprueba que no esta vacia el proyecto elegido
    if (this.checkIfIsSet(this.ProyectoSeleccionado) && this.checkIfIsSet(this.AssessmentSelected)) {
      //Comprueba si ya termino de enviarse la información desde la api
      if (!this.SendingInfo) {
        this.SendingInfo = true;
        this._evaluacionService.getIncompleteEvaluacionFromProjectAndAssessment(this.ProyectoSeleccionado.id, this.AssessmentSelected.assessmentId).subscribe(
          res => {
            //Lo guarda en el storage
            this._appComponent._storageDataService.Evaluacion = res;
            //Si hay un proyecto sin finalizar
            // console.log("XXXXX",res);
            if (res != null) {
              this.existeRepetida = true;
            }
          },
          error => {
            //Habilitamos la pagina nuevamente
            this.Deshabilitar = false;
            if (error == 404) {
              this.ErrorMessage = "Error: " + error + " No se puede completar la comprobación en la evaluación lo sentimos.";
            } else if (error == 500) {
              this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
            } else if (error == 401) {
              this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
            } else {
              this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
            }
            setTimeout(() => {
              this.fadeInError = false;
              setTimeout(() => this.ErrorMessage = "", 900);
            }, 2000);
          },
          () => {
            this.SendingInfo = false;
          });
      }
    }

  }

  //Este metodo crea una nueva evaluación y la manda para guardarla en la base de datos
  public GuardarEvaluacion() {

    var NuevaEvaluacion: EvaluacionCreate = { 'estado': false, 'proyectoid': this.ProyectoSeleccionado.id, 'userNombre': this._proyectoService.UsuarioLogeado, 'assessmentId': this.AssessmentSelected.assessmentId, 'assesmentName': this.AssessmentSelected.assessmentName };    // console.log("assessmeeeent", this.AssessmentSelected);
    // console.log(NuevaEvaluacion);

    this._evaluacionService.addEvaluacion(NuevaEvaluacion).subscribe(
      res => {
        this._appComponent._storageDataService.Evaluacion = res;
        this._appComponent._storageDataService.Evaluacion.assessmentName = this.AssessmentSelected.assessmentName;
        this.SendingInfo = false;

        this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);
        this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.Evaluacion.assessmentName, null);
        this._appComponent.pushBreadcrumb("BREADCRUMB.NEW_ASSESSMENT", null);
        this._router.navigate(['/evaluationsections']);
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " La petición de crear una evaluación es incorrecta.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        setTimeout(() => {
          this.fadeInError = false;
          setTimeout(() => this.ErrorMessage = "", 900);
        }, 2000);
        this.SendingInfo = false;
      });
  }

  //Este metodo consulta las evaluaciones anteriores de este proyecto si esta seleccionado y existe
  public EvaluacionesAnteriores() {
    this._router.navigate(['/finishedevaluations']);
    // if (this.SeeAllProjects || this.ProyectoSeleccionado != null && this.ProyectoSeleccionado != undefined) {
    //   this._router.navigate(['/finishedevaluations']);
    // } else {
    //   this.ErrorMessage = "Seleccione un proyecto para realizar esta acción.";
    //   setTimeout(() => {
    //     this.fadeInError = false;
    //     setTimeout(() => this.ErrorMessage = "", 900);
    //   }, 2000);
    // }
  }

  //Este metodo consulta las evaluaciones anteriores de este proyecto si esta seleccionado y existe
  public EvaluacionesPendientes() {
    this._router.navigate(['/pendingevaluations']);
    // if (this.SeeAllProjects || this.ProyectoSeleccionado != null && this.ProyectoSeleccionado != undefined) {
    //   this._router.navigate(['/pendingevaluations']);
    // } else {
    //   this.ErrorMessage = "Seleccione un proyecto para realizar esta acción.";
    //   setTimeout(() => {
    //     this.fadeInError = false;
    //     setTimeout(() => this.ErrorMessage = "", 900);
    //   }, 2000);
    // }
  }

  //Este metodo guarda la evaluacion y cambia su estado como finalizado
  public FinishEvaluation() {
    //Recoge la evaluación
    var Evaluacion = this._appComponent._storageDataService.Evaluacion;

    //La cambia a temrinada
    Evaluacion.estado = true;
    Evaluacion.userNombre = localStorage.getItem("user");
    //La envia a la base de datos ya terminada
    this._evaluacionService.updateEvaluacion(Evaluacion).subscribe(
      res => {
        //Una vez terminado guarda una nueva evaluación
        this.GuardarEvaluacion();
        this.SendingInfo = false;
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " La petición de modificación de evaluación no puede ser realizada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        setTimeout(() => {
          this.fadeInError = false;
          setTimeout(() => this.ErrorMessage = "", 900);
        }, 2000);
        this.SendingInfo = false;
      });
  }

  checkIfIsSet(x: any): boolean {
    if (x != null && x != undefined) return true;
  }

  //Muestra un modal con lo que se debe hacer en cada caso
  showModal(content) {

    //Comprueba que no esta vacia el proyecto elegido
    if (this.checkIfIsSet(this.ProyectoSeleccionado) && this.checkIfIsSet(this.AssessmentSelected)) {
      //Comprueba si ya termino de enviarse la información desde la api
      if (!this.SendingInfo) {
        if (this.existeRepetida) {
          this.modalService.open(content).result.then(
            (closeResult) => {
            }, (dismissReason) => {
              //Si selecciona continuar cargara la valuación que no termino
              if (dismissReason == 'EvaluacionesPendientes') {
                this.EvaluacionesPendientes();
              } else if (dismissReason == 'Nueva') {
                //Crea una nueva evaluacion
                //this.FinishEvaluation(); //termina la evaluacion
                this.GuardarEvaluacion();
              }
            })
        } else {
          //Si no encuentra ninguna repetida directamente te crea una nueva evaluación
          this.GuardarEvaluacion();
        }
      }
    } else {
      let item = this.checkIfIsSet(this.ProyectoSeleccionado) ? "una evaluación" : "un proyecto";
      this.ErrorMessage = `Seleccione ${item} para realizar esta acción.`;
      this.fadeInError = true;
      setTimeout(() => {
        this.fadeInError = false;
        setTimeout(() => this.ErrorMessage = "", 900);
      }, 2000);

    }
  }

  //Para seguir con la evaluacion seleccionada
  public continuarEvaluacion() {
    this._router.navigate(['/evaluationsections']);
  }

  public getUserRole() {
    this._proyectoService.getRolesUsuario().subscribe(
      res => {
        var permisosDeUsuario = res;
        //console.log("permisos de usuario en getUserRole", permisosDeUsuario);
        this._appComponent._storageDataService.Role = permisosDeUsuario.id;
        //Si no hay errores y son recogidos busca si tienes permisos de usuario
        if (permisosDeUsuario.id == this.rol.Administrador) {
          //this._appComponent.RolDeUsuario = true;
          // console.log("this._appComponent.RolDeUsuario", this._appComponent.RolDeUsuario);
          // console.log("this._appComponent._storageDataService.RoleAdmin", this._appComponent._storageDataService.RoleAdmin);
          this._appComponent._storageDataService.RoleAdmin = true;
        } else {
          this._appComponent._storageDataService.RoleAdmin = false;
        }

        //Llamamos al metodo para asignar proyectos


      },
      error => {

      });
  }
}
