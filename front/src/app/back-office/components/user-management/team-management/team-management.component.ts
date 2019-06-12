import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CdkDragDrop, moveItemInArray, transferArrayItem, CdkDrag } from '@angular/cdk/drag-drop';
import { UserService } from 'app/services/UserService';
import { UserCreateUpdate } from 'app/Models/UserCreateUpdate';
import { ProyectoService } from 'app/services/ProyectoService';
import { Equipo } from 'app/Models/Equipo';
import { UserWithRole } from 'app/Models/UserWithRole';
import { UserProject } from 'app/Models/UserProject';
import { EventEmitterService } from 'app/services/event-emitter.service';

@Component({
  selector: 'app-team-management',
  templateUrl: './team-management.component.html',
  styleUrls: ['./team-management.component.scss'],
})
export class TeamManagementComponent implements OnInit {

  //items = Array.from({ length: 100000 }).map((_, i) => `Item #${i}`);

  public proyectosAll: Equipo[] = [];
  public proyectosAsig: Equipo[] = [];
  public proyectosPending: Equipo[] = [];
  public ErrorMessage: string = null;
  public MensajeNotificacion: string = null;
  public usuarioWithRole: UserWithRole;
  public user: UserCreateUpdate;
  public MostrarPending: boolean = false;
  public MostrarAsig: boolean = false;

  constructor(
    private _router: Router,
    private _userService: UserService,
    private _proyectoService: ProyectoService,
    private _eventService: EventEmitterService,
  ) { }

  ngOnInit() {

    //Obtenemos el usuario para el que queremos asignar los proyectos
    this.getUser();

    //Obtenemos todos los proyectos
    this.getTeams();
  }


  drop(event: CdkDragDrop<string[]>) {
    //moveItemInArray(this.moviesAll, event.previousIndex, event.currentIndex);
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }
    else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);

      var addUP = new UserProject(this.user.nombre, event.item.data.id);

      if (event.container.connectedTo === "proyectosAsig") {
        this.removeUserProyect(addUP, event);
      }
      else {
        this.addUserProyect(addUP, event);
      }
    }
  }

  private removeUserProyect(usuarioProyecto, evento) {

    this._userService.removeUserProject(usuarioProyecto).subscribe(
      res => {
        this.MensajeNotificacion = "Equipo " + evento.item.data.nombre + " desasignado al usuario " + this.user.nombre;
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        //Si el servidor tiene algún tipo de problema mostraremos este error
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " El usuario o proyecto autenticado no existe.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else if (error == 400) {
          this.ErrorMessage = "El usuario seleccionado podrá evaluar cualquier equipo.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }

        this.MensajeNotificacion = this.ErrorMessage;
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);

      });
  }

  private addUserProyect(usuarioProyecto, evento) {
    this._userService.addUserProject(usuarioProyecto).subscribe(
      res => {
        this.MensajeNotificacion = "Equipo " + evento.item.data.nombre + " asignado al usuario " + this.user.nombre;
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
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

    this.MensajeNotificacion = this.ErrorMessage
    this._eventService.displayMessage(this.MensajeNotificacion, true);
    setTimeout(() => { this.MensajeNotificacion = null }, 4000);
  }

  private getTeams() {
    this._proyectoService.GetAllNotTestProjects().subscribe(
      res => {
        this.proyectosAll = res;

        //Obtenemos los proyectos asignados al usuario asignado
        if (this.usuarioWithRole !== undefined) {
          this.getTeamsUser(this.usuarioWithRole);
        }
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la información solicitada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }

        this.MensajeNotificacion = this.ErrorMessage
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      }
    )
  };

  private getTeamsUser(usuario: UserWithRole) {
    this._proyectoService.getProyectosDeUsuarioSeleccionado(usuario).subscribe(
      res => {
        //hay que añadirle el filtro porque el metodo este no distingue si el proyecto es de prueba o no
        this.proyectosAsig = res.filter(r => !r.testProject);

        console.log("Entramos en el filtrado");

        // //Obtenemos los proyectos pendientes pero metermos un retardo para ver como se vería en la precarga
        //setTimeout(() => { console.log("Retrasamos la salida");
        // this.proyectosPending = this.proyectosAll.filter( e => function (proyecto: Equipo, proyestosAsignados: Equipo[]): boolean {
        //   return !proyestosAsignados.find(eq => eq.id === proyecto.id);}(e, this.proyectosAsig));

        // this.MostrarPending = true; 
        // this.MostrarAsig = true; 
        // }, 10000);

        //Obtenemos los proyectos pendientes
        this.proyectosPending = this.proyectosAll.filter(e => function (proyecto: Equipo, proyestosAsignados: Equipo[]): boolean {
          return !proyestosAsignados.find(eq => eq.id === proyecto.id);
        }(e, this.proyectosAsig));

        console.log("ProyectosALL: " + this.proyectosAll.length);

        if (this.proyectosAll.length > 0) {
          this.MostrarPending = true;
          this.MostrarAsig = true;
        }

      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la información solicitada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }

        this.MensajeNotificacion = this.ErrorMessage
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      }
    )
  }

  public volver() {
    this._router.navigate(['/backoffice/usermanagement']);
  }

  private getUser() {
    //Recogemos el usuario enviado desde la lista de usuarios
    this.user = this._userService.user;


    if (this.user !== undefined) {

      // if (this.user.role.role === "Administrador") {
      //   this.MensajeNotificacion = "El usuario seleccionado podrá evaluar cualquier equipo.";
      //   this._eventService.displayMessage(this.MensajeNotificacion, false);
      //   setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      // }

      this.usuarioWithRole =
        {
          nombre: this.user.nombre,
          nombreCompleto: this.user.nombreCompleto,
          activo: true, password: this.user.password,
          role: this.user.role
        };
    }
  }
}
