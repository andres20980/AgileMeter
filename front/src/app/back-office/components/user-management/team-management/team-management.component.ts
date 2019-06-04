import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';

import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

import { UserService } from 'app/services/UserService';
import { UserCreateUpdate } from 'app/Models/UserCreateUpdate';
import { ProyectoService } from 'app/services/ProyectoService';
import { Equipo } from 'app/Models/Equipo';
import { UserWithRole } from 'app/Models/UserWithRole';
import { timingSafeEqual } from 'crypto';
import { IfStmt } from '@angular/compiler';

@Component({
  selector: 'app-team-management',
  templateUrl: './team-management.component.html',
  styleUrls: ['./team-management.component.scss']
})
export class TeamManagementComponent implements OnInit {

  public proyectosAll: Equipo[] = [];
  public proyectosAsig: Equipo[] = [];
  public proyectosPending: Equipo[] = [];
  public ErrorMessage: string = null;
  public InfoMessage: string = null;
  public usuarioWithRole: UserWithRole;
  public user: UserCreateUpdate;

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

      //Guardamos los proyectos asignados al usuario
      this.guardarCambios();

    }
  }

  private guardarCambios() {
    this.InfoMessage = "Transferido";
    setTimeout(() => { this.InfoMessage = null }, 2000);
  }

  constructor(
    private _router: Router,
    private _userService: UserService,
    private _proyectoService: ProyectoService,
  ) { }

  ngOnInit() {

    //Obtenemos el usuario para el que queremos asignar los proyectos
    this.getUser();

    //Obtenemos todos los proyectos
    this.getTeams();

    //Obtenemos los proyectos asignados al usuario asignado
    if (this.usuarioWithRole !== undefined) {
      this.getTeamsUser(this.usuarioWithRole);
    }
  }

  private getTeams() {
    this._proyectoService.GetAllNotTestProjects().subscribe(
      res => {
        this.proyectosAll = res;
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
      }
    )
  };

  protected noEsTestProyect(proyecto: Equipo) {
    if (!proyecto.testProject)
      return true;
    return false;
  }

  private getTeamsUser(usuario: UserWithRole) {
    this._proyectoService.getProyectosDeUsuarioSeleccionado(usuario).subscribe(
      res => {
        //hay que añadirle el filtro porque el metodo este no distingue si el proyecto es de prueba o no
        this.proyectosAsig = res.filter(r => !r.testProject);

        //Obtenemos los proyectos pendientes
        this.proyectosPending = this.proyectosAll.filter(e => function (proyecto: Equipo, proyestosAsignados: Equipo[]): boolean {
          return !proyestosAsignados.find(eq => eq.id === proyecto.id);
        }(e, this.proyectosAsig));
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
      }
    )
  }

  public volver() {
    this._router.navigate(['/backoffice/usermanagement']);
  }

  private getUser() {
    //Recogemos el usuario enviado desde la lista de usuarios
    this.user = this._userService.user;

    if (this.user !== undefined)
      this.usuarioWithRole =
        {
          nombre: this.user.nombre,
          nombreCompleto: this.user.nombreCompleto,
          activo: true, password: this.user.password,
          role: this.user.role
        };
  }
}
