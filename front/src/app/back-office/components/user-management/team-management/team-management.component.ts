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

  public usuarioWithRole: UserWithRole;

  // moviesAll = [
  //   'Episode I - The Phantom Menace',
  //   'Episode II - Attack of the Clones',
  //   'Episode III - Revenge of the Sith',
  //   'Episode IV - A New Hope',
  //   'Episode V - The Empire Strikes Back',
  //   'Episode VI - Return of the Jedi',
  //   'Episode VII - The Force Awakens',
  //   'Episode VIII - The Last Jedi'
  // ];

  moviesAll = [
  ];

  moviesAsig = [
    'Episode III - Revenge of the Sith',
    'Episode IV - A New Hope',
    'Episode V - The Empire Strikes Back',
  ];

  moviesPending = [
  ];

  public user: UserCreateUpdate;

  drop(event: CdkDragDrop<string[]>) {
    //moveItemInArray(this.moviesAll, event.previousIndex, event.currentIndex);
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      console.log(event);
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }

    this.ErrorMessage = "Transferido";
  }

  /** Predicate function that doesn't allow items to be dropped into a list. */
  noReturnPredicate() {
    return false;
  }

  constructor(
    private _router: Router,
    private _userService: UserService,
    private _proyectoService: ProyectoService,
  ) { }

  ngOnInit() {

    //Inicializamos el array de moviesPending
    //this.moviesPending = this.moviesAll.filter(item => this.moviesAsig.indexOf(item) < 0);

    //Find values that are in result2 but not in result1

    //Obtenemos el usuario para el que queremos asignar los proyectos
    this.getUser();

    //Obtenemos todos los proyectos
    this.getTeams();

    //Obtenemos los proyectos asignados al usuario asignado
    if (this.usuarioWithRole !== undefined) {
      this.getTeamsUser(this.usuarioWithRole);

    }
  }

  public getTeams() {
    this._proyectoService.GetAllNotTestProjects().subscribe(
      res => {
        this.proyectosAll = res;
        console.log("res");
        console.log(this.proyectosAll);
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


  public noEsTestProyect(proyecto: Equipo) {

    if (!proyecto.testProject)
      return true;
    return false;

  }

  public getTeamsUser(usuario: UserWithRole) {
    this._proyectoService.getProyectosDeUsuarioSeleccionado(usuario).subscribe(
      res => {
        this.proyectosAsig = res;

        //Quitamos el equipo de prueba
        if (this.proyectosAsig !== undefined)
          this.proyectosAsig = this.proyectosAsig.filter(this.noEsTestProyect);

        console.log("Entro en interval")

        if (this.proyectosAll !== undefined) {
          console.log("Entro en filter");
          console.log(this.proyectosAsig);
          console.log(this.proyectosAll);
          if (this.proyectosAll !== undefined && this.proyectosAsig !== undefined) {
            var proyectosAsig = this.proyectosAsig;
            this.proyectosPending = this.proyectosAll.filter(
              function (obj,order,proyectosAsig) {
                console.log(obj);
                if (proyectosAsig !== undefined) {
                  return !proyectosAsig.some(
                    function (obj2) {
                      console.log(obj2);
                      return obj.id == obj2.id;
                    });
                } else
                  return false;
              });
          } else if (this.proyectosAsig !== undefined) {
            this.proyectosPending = this.proyectosAll;
          }
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
      }
    )
  };

  public volver() {
    this._router.navigate(['/backoffice/usermanagement']);
  }

  public getUser() {
    // 1º Recogemos el usuario enviado desde la lista de usuarios

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
