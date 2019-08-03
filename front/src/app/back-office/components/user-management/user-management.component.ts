import { Component, OnInit, OnDestroy, ViewChild, NgZone, Output, EventEmitter } from '@angular/core';
import { UserService } from '../../../services/UserService';
import { ProyectoService } from 'app/services/ProyectoService';
import { Router } from "@angular/router";
import { UserWithRole } from 'app/Models/UserWithRole';
import { Proyecto } from 'app/Models/Proyecto';
import { AppComponent } from 'app/app.component';
import { Role } from 'app/Models/Role';
import { FormControl } from '@angular/forms';
import { take, takeUntil } from 'rxjs/operators';
import { Subject, Subscription } from 'rxjs';
import { ReplaySubject } from 'rxjs';
import { MatList, MatIcon, VERSION } from '@angular/material';
import { User } from 'app/Models/User';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { StorageDataService } from 'app/services/StorageDataService';
import { UserProject } from 'app/Models/UserProject';


@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
  //providers: [UserService, ProyectoService]
})


export class UserManagementComponent implements OnInit {
  public ErrorMessage: string = null;
  public ListaDeUsuarios: UserWithRole[] = [];
  public UsuarioSeleccionado: UserWithRole;
  public UsuarioLogueado: string;
  public ListaDeRoles: Role[] = [];
  public ListaDeProyectos: Proyecto[] = [];
  public ListaDeProyectosUsuario: Proyecto[] = [];
  public radioSelected;
  public rolControl;
  public showProjects: boolean = false;
  public MensajeNotificacion: string = null;

  public projectsSelected: string[] = [];
  /** control for the selected bank */
  public userCtrl: FormControl = new FormControl();

  /** control for the MatSelect filter keyword */
  public userFilterCtrl: FormControl = new FormControl();
  /** control for the selected bank for multi-selection */
  public projectMultiCtrl: FormControl = new FormControl();

  /** control for the MatSelect filter keyword multi-selection */
  public projectMultiFilterCtrl: FormControl = new FormControl();


  /** list of banks filtered by search keyword */
  public filteredUsers: ReplaySubject<UserWithRole[]> = new ReplaySubject<UserWithRole[]>(1);
  //public filteredProjects: ReplaySubject<Proyecto[]> = new ReplaySubject<Proyecto[]>(1);

  /** list of banks filtered by search keyword for multi-selection */
  public filteredProjectsMulti: ReplaySubject<Proyecto[]> = new ReplaySubject<Proyecto[]>(1);

  private _onDestroy = new Subject<void>();

  constructor(
    private _UserService: UserService,
    private _proyectoService: ProyectoService,
    private modalService: NgbModal,
    public _storageDataService: StorageDataService,
    private _router: Router,
    private _appComponent: AppComponent,
    private _eventService: EventEmitterService,
    private _zone: NgZone
  ) {
  }

  ngOnInit() {
    //Empezamos cargando el usuario en el componente mientras verificamos si esta logueado
    //En casao de no estar logeado nos enviara devuelta al login
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }

    var local = localStorage.getItem("user");

    this.getAllUsers();
    this.getAllRoles();

    if (local == null) {
      this.UsuarioLogueado = this._appComponent._storageDataService.UserData.nombre
      this.getAllProjects(this._appComponent._storageDataService.UserData.nombre);
    } else {
      this.UsuarioLogueado = local;
      this.getAllProjects(this.UsuarioLogueado);
    }

    //listen for search field value changes
    this.userFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterUsers();

      });

    this.userCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.showProjects = false;
        this.getAllProjects(this.userCtrl.value.nombre);
        this.getUserProjects();
        this.filterProjectMulti();
      });

    this.projectMultiFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterProjectMulti();
      });

    this.projectMultiCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.ListaDeProyectosUsuario = this.projectMultiCtrl.value;
      });
  }

  ngAfterViewInit() {
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  private getAllUsers() {
    this._UserService.getUsers().subscribe(
      res => {
        this.ListaDeUsuarios = res;
        this.filteredUsers.next(this.ListaDeUsuarios.slice());
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

  private getAllRoles() {
    this._UserService.getAllRoles().subscribe(
      res => { this.ListaDeRoles = res; },
      error => {
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

  private getAllProjects(userNombre: string) {
    this._proyectoService.getAllProyectos(userNombre).subscribe(
      res => {
        this.ListaDeProyectos = res;
        // this.projectMultiCtrl.setValue(this.ListaDeProyectos); 
        this.filteredProjectsMulti.next(this.ListaDeProyectos);
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

  private filterUsers() {
    if (!this.ListaDeUsuarios) {
      return;
    }
    // get the search keyword
    let search = this.userFilterCtrl.value;
    if (!search) {
      this.filteredUsers.next(this.ListaDeUsuarios.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the Users
    this.filteredUsers.next(
      this.ListaDeUsuarios.filter(user => user.nombre.toLowerCase().indexOf(search) > -1)
    );
  }

  private filterProjectMulti() {
    if (!this.ListaDeProyectos) {
      return;
    }
    // get the search keyword
    let search = this.projectMultiFilterCtrl.value;
    if (!search) {
      this.filteredProjectsMulti.next(this.ListaDeProyectos);
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredProjectsMulti.next(
      this.ListaDeProyectos.filter(project => project.nombre.toLowerCase().indexOf(search) > -1)
    );
  }

  private getUserProjects() {
    if (this.userCtrl.value.role.id == 1) {
      this.showProjects = true;
    }

    this._proyectoService.getProyectosDeUsuarioSeleccionado(this.userCtrl.value).subscribe(
      res => {
        this.ListaDeProyectosUsuario = res;
        this.projectsSelected = [];

        this.ListaDeProyectosUsuario.forEach((element) => {
          this.projectsSelected.push(`${element.id}`);
        });
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
        setTimeout(() => this.ErrorMessage = "", 4000);
      });
  }

  manageUserProjects(project) {
    let exists: boolean = false;
    let i = 0;

    this.ListaDeProyectosUsuario.forEach((element, index) => {
      if (element.id == project.id) { exists = true; i = index; }
    });

    if (exists) {
      var removeUP = new UserProject(this.userCtrl.value.nombre, project.id);
      this.removeUserProyect(removeUP);
      this.ListaDeProyectosUsuario.splice(i, 1);
    }
    else {
      var addUP = new UserProject(this.userCtrl.value.nombre, project.id);
      this.addUserProyect(addUP);
      this.ListaDeProyectosUsuario.push(project);
    }
  }

  deleteSingleUserProjects(project) {
    console.log("borrar single");
    let i = 0;
    this.ListaDeProyectosUsuario.forEach((element, index) => {
      if (element.id == project.id) { i = index; }
    });
    var removeUP = new UserProject(this.userCtrl.value.nombre, project.id);
    this.removeUserProyect(removeUP);
    this.ListaDeProyectosUsuario.splice(i, 1);
  }

  private updateUserRol(UsuarioSeleccionado) {
    UsuarioSeleccionado.password = null;
    this._UserService.updateUser(UsuarioSeleccionado).subscribe(
      res => {
        this.getUserProjects();

        // this.MensajeNotificacion = "Rol del usuario actualizado correctamente";
        // this._eventService.displayMessage(this.MensajeNotificacion,false);
        // setTimeout(() => { this.MensajeNotificacion = null }, 4000);
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
        this.MensajeNotificacion = "Ups, lo sentimos, no pudimos actualizar el rol del usuario";
        this._eventService.displayMessage(this.MensajeNotificacion,true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  private addUserProyect(usuarioProyecto) {
    this._UserService.addUserProject(usuarioProyecto).subscribe(
      res => {
        this.getUserProjects();
        // this.MensajeNotificacion = "Equipo asignado correctamente";
        // this._eventService.displayMessage(this.MensajeNotificacion,false);
        // setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        console.log(error);
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
        this.MensajeNotificacion = "Ups , lo sentimos, no pudimos asignar el equipo";
        this._eventService.displayMessage(this.MensajeNotificacion,true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  private removeUserProyect(usuarioProyecto) {
    this._UserService.removeUserProject(usuarioProyecto).subscribe(
      res => {
        this.getUserProjects();
        this.MensajeNotificacion = "Proyecto eliminado correctamente";
        this._eventService.displayMessage(this.MensajeNotificacion,false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        console.log("falla el borrar", error);
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
        this.MensajeNotificacion = "Ups, lo sentimos, no pudimos eliminar el proyecto";
        this._eventService.displayMessage(this.MensajeNotificacion,true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  showModal(content, role: Role) {
    if (this.userCtrl.value.nombre == this.UsuarioLogueado && this.userCtrl.value.role.role == 'Administrador') {
      if (role.id != 2) {
        console.log("entra", this.userCtrl.value.role.role);
        this.modalService.open(content).result.then(
          (closeResult) => {
          }, (dismissReason) => {
            if (dismissReason == 'Cerrar') {

            } else if (dismissReason == 'Aceptar') {
              this.seleccionDeRol(role);
              this._router.navigate(['/login']);
            }
          })
      }
    } else {
      this.seleccionDeRol(role);
    }
  }

  public seleccionDeRol(role: Role) {
    console.log("seleccion de rol", role.role)
    this.showProjects = false;
    this.userCtrl.value.role = role;
    this.UsuarioSeleccionado = this.userCtrl.value;
    this.updateUserRol(this.UsuarioSeleccionado);
  }  
}
