import { Component, OnInit, HostListener } from '@angular/core';
import { SectionService } from '../services/SectionService';
import { EvaluacionService } from '../services/EvaluacionService';
import { ProyectoService } from '../services/ProyectoService';
import { AppComponent } from '../app.component';
import { Section } from 'app/Models/Section';
import { Router } from "@angular/router";
import { Proyecto } from 'app/Models/Proyecto';
import { Evaluacion } from 'app/Models/Evaluacion';
import { SectionInfo } from 'app/Models/SectionInfo';
import { SectionModify } from 'app/Models/SectionModify';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-menunewevaluation',
  templateUrl: './menunewevaluation.component.html',
  styleUrls: ['./menunewevaluation.component.scss'],
  providers: [SectionService, ProyectoService, EvaluacionService]
})
export class MenunewevaluationComponent implements OnInit {

  public ErrorMessage: string = null;
  public ListaDeDatos: Array<SectionInfo> = [];
  public ProjectSelected: Proyecto;
  public Evaluacion: Evaluacion = null;
  public UserSelected: string;
  public MostrarInfo = false;
  public textoModal: string;
  public anadeNota: string = null;
  public ScreenWidth;
  public cargar: boolean = false;

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.ScreenWidth = window.innerWidth;
  }

  constructor(
    private _proyectoService: ProyectoService,
    private _sectionService: SectionService,
    private _router: Router,
    private _evaluacionService: EvaluacionService,
    private _appComponent: AppComponent,
    private modalService: NgbModal) {

    this.ScreenWidth = window.innerWidth;

  }

  ngOnInit() {

    //Empezamos cargando el usuario en el componente mientras verificamos si esta logueado
    //En caso de no estar logeado nos enviara devuelta al login
    //En caso de no tener asignado ningun proyecto nos enviara a home para que lo seleccionemos
    this.ProjectSelected = this._appComponent._storageDataService.UserProjectSelected;
    this.Evaluacion = this._appComponent._storageDataService.Evaluacion;
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    } else if (this.ProjectSelected == null || this.ProjectSelected == undefined || this.ProjectSelected.id == -1 || this.Evaluacion == null || this.Evaluacion == undefined) {
      this._router.navigate(['/home']);
    }
    //Recogemos el nombre del usuario con el que nos logueamos
    this.UserSelected = this._proyectoService.UsuarioLogeado;
    this.MostrarInfo = false;

    if(this.Evaluacion != null){
      //this._appComponent.pushBreadcrumb(this.ProjectSelected.nombre, null);
      //this._appComponent.pushBreadcrumb(this.Evaluacion.assessmentName, null);
      this._appComponent.pushBreadcrumb("Secciones", "/evaluationsections");
    }
    

    //Recogemos todos los datos
    if (this.Evaluacion != null && this.Evaluacion != undefined) {
      this._sectionService.getSectionInfo(this.Evaluacion.id,this._appComponent._storageDataService.AssessmentSelected.assessmentId).subscribe(
        res => {
          this.ListaDeDatos = res;
          this._appComponent._storageDataService.Sections = this.ListaDeDatos;
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
    } else {
      this.VolverInicio();
    }

    //Para que no de error en modo development
    setTimeout(() => {
    this._appComponent.anadirUserProyecto(null, null, this.ProjectSelected.nombre,this._appComponent._storageDataService.AssessmentSelected.assessmentName);
    });

  }

  //Calcula el total de las ceremonias que llevamos completadas de forma dinamica
  public CalcularPorcentaje(preguntasRespondidas: number, totalPreguntas: number) {
    //Calculamos el porcentaje de las preguntas respondidas a partir del total
    var Total = (preguntasRespondidas / totalPreguntas) * 100;
    //Redondeamos el porcentaje obtenido y lo devolvemos
    return Math.round(Total * 10) / 10;
  }

  //Permite redirigir y guardar la id de la sección seleccionada
  public RedirectToAsignaciones(SectionSeleccionada: SectionInfo, index:number) {
    this._appComponent._storageDataService.SectionSelectedInfo = SectionSeleccionada;
    this._appComponent._storageDataService.nextSection = (index+1) != this.ListaDeDatos.length ? this.ListaDeDatos[index+1] : null;
    this._appComponent._storageDataService.prevSection = (index-1) != -1 ? this.ListaDeDatos[index-1] : null;

    this._router.navigate(['/evaluationquestions']);
  }

  //Volver a la pagina principal
  public VolverInicio() {
    this._router.navigate(['/home']);
  }
  
  //Para abrir las notas de secciones
  public AbrirModalNotas(content, i) {

    this.anadeNota = null;

    if (this.ListaDeDatos[i].notas != null) {
      this.textoModal = this.ListaDeDatos[i].notas;
    } else {
      this.textoModal = "";
    }

    this.modalService.open(content).result.then(
      (closeResult) => {
        //Si cierra, no se guarda

      }, (dismissReason) => {
        if (dismissReason == 'Guardar') {

          this.cargar = true;

          if (this.textoModal != "") {
            this.ListaDeDatos[i].notas = this.textoModal;
          } else {
            this.ListaDeDatos[i].notas = null;
          }


          var SeccionModificada = new SectionModify(this.Evaluacion.id, this.ListaDeDatos[i].id, this.ListaDeDatos[i].notas);

          this._sectionService.addNota(SeccionModificada).subscribe(
            res => {

              this.anadeNota = "Nota añadida correctamente";
              setTimeout(()=>{this.anadeNota = null},2000);
            },
            error => {

              if (error == 404) {
                this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización de la respuesta, lo sentimos.";
              } else if (error == 500) {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              } else if (error == 401) {
                this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
              } else {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              }
            },
            () => {
              this.cargar = false;

            });

        }
        //Else, Click fuera, no se guarda
      })
  }
}
