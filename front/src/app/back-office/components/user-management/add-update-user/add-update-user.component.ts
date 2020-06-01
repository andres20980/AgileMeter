import { ProyectoService } from 'app/services/ProyectoService';
import { UserCreateUpdate } from './../../../../Models/UserCreateUpdate';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Role } from 'app/Models/Role';
import { UserService } from 'app/services/UserService';
import { Router } from '@angular/router';
import { AppComponent } from 'app/app.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-add-update-user',
  templateUrl: './add-update-user.component.html',
  styleUrls: ['./add-update-user.component.scss']
})
export class AddUpdateUserComponent implements OnInit {
  public ErrorMessage: string = null;
  public Error: string = null;
  public userForm: FormGroup;
  public user: UserCreateUpdate;
  public MensajeNotificacion: string = null;

  public rolList: Role[];
  rol: Role = { id: 1, role: "Usuario" };
  idiomaFavorito = "es";
  compareRol(o1: any, o2: any): boolean {
    return o1.id === o2.id;
  }
  constructor(
    private _userService: UserService,
    private _router: Router,
    private _proyectoService: ProyectoService,
    private modalService: NgbModal,
    private _appComponent: AppComponent,
    private _eventService: EventEmitterService,
    private _translateService: TranslateService
  ) { }

  ngOnInit() {
    this.getAllRol();
    if (this._userService.user != undefined) {
      this.getUser();
    }
    this.formValidate();
  }

  //form validate
  public formValidate() {
    if (this._userService.user != null) {
      this.userForm = new FormGroup({
        Nombre: new FormControl(this.user.nombre, Validators.required),
        NombreCompleto: new FormControl(this.user.nombreCompleto, Validators.required),
        Password: new FormControl(null),
        Role: new FormControl(this.rol),
        Activo: new FormControl(true),
        IdiomaFavorito: new FormControl(this.idiomaFavorito),
      });
      this.userForm.controls['Nombre'].disable();
    } else {
      this.userForm = new FormGroup({
        Nombre: new FormControl('', [Validators.required, Validators.maxLength(127)]),
        NombreCompleto: new FormControl('', Validators.required),
        Password: new FormControl('', Validators.required),
        Role: new FormControl(this.rol),
        IdiomaFavorito: new FormControl(this.idiomaFavorito),
      });
    }
  }

  //form validate
  public hasError = (controlName: string, errorName: string) => {
    return this.userForm.controls[controlName].hasError(errorName);
  }

  public getAllRol() {
    this._userService.getAllRoles().subscribe(
      res => {
        this.rolList = res;
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " Roles Not Found.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      });
  }

  public altaUsuario() {
    var form = this.userForm.value;
    this._userService.addUser(form).subscribe(
      res => {
        this._router.navigate(['/backoffice/usermanagement']);
        this._translateService.get('ADD_UPDATE_USER.NOTIFICATION_ADD_USER').subscribe(value => { this.MensajeNotificacion = value; });
        //this.MensajeNotificacion = "Usuario creado correctamente";
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        if (error == 400) {
          this.ErrorMessage = "Error : Ese usuario ya existe.";
          this.Error = "400";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        //this.MensajeNotificacion = "Ups, lo sentimos, no hemos podido crear el usuario";
        this._translateService.get('ADD_UPDATE_USER.NOTIFICATION_ERROR_ADD_USER').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  public comprobarDatos(content) {
    var form = this.userForm.value;
    //le asignamos el nombre al form ( como el campo nombre esta deshabilitado se recoge como null)
    form.nombre = this._userService.user.nombre;
    if (form.password == "") {
      form.password = null;
    }
    //si el usuario logueado es el usuario a modificar y le cambias el roll te muestra una ventana de advertencia indicando que te redirige al home
    if (this._proyectoService.UsuarioLogeado == form.nombre && this._userService.user.role.id != form.Role.id) {
      this.AbrirModal(content, form);
    }
    //si no se modifica
    else {
      this.updateUsuario(form);
    }
  }

  public updateUsuario(form) {
    this._userService.updateUser(form).subscribe(
      res => {
        //comprobamos si el usuario a modificar es el mismo que el usuario logueado y cambias el roll te redirige al home        
        if (this._proyectoService.UsuarioLogeado == form.nombre && this._userService.user.role.id != form.Role.id) {
          this._router.navigate(["/home"]);
        } else {//si no te mantiene en el listado de usuarios
          this._router.navigate(['/backoffice/usermanagement']);
        }
        //this.MensajeNotificacion = "Usuario modificado correctamente";
        this._translateService.get('ADD_UPDATE_USER.NOTIFICATION_UPDATE_USER').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        if (error == 400) {
          this.ErrorMessage = "Ups, lo sentimos,no pudimos modificar el usuario.";
          this.Error = "400";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        //this.MensajeNotificacion = "Ups, lo sentimos,no pudimos modificar el usuario.";
        this._translateService.get('ADD_UPDATE_USER.NOTIFICATION_ERROR_UPDATE_USER').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  public getUser() {
    // 1º Recogemos el user
    this.user = this._userService.user;

    //2º recogemos los objetos que marcaremos por defecto en los select
    this.rol = this.user.role;
    this.idiomaFavorito = this.user.idiomaFavorito;
  }

  public AbrirModal(content, form) {
    this.modalService.open(content).result.then(
      (closeResult) => {
        //Esto realiza la acción de cerrar la ventana
      }, (dismissReason) => {
        if (dismissReason == 'Finish') {
          //Si decide finalizarlo usaremos el metodo para finalizar la evaluación
          this.updateUsuario(form);
        }
      })
  }

  public volver() {
    this._router.navigate(['/backoffice/usermanagement']);
  }
}
