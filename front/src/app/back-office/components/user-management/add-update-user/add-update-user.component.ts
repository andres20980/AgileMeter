import { ProyectoService } from 'app/services/ProyectoService';
import { UserCreateUpdate } from './../../../../Models/UserCreateUpdate';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Role } from 'app/Models/Role';
import { UserService } from 'app/services/UserService';
import { Router } from '@angular/router';
import { AppComponent } from 'app/app.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

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

  public rolList: Role[];
  rol: Role = { id: 1, role: "Usuario" };
  compareRol(o1: any, o2: any): boolean {
    return o1.id === o2.id;
  }
  constructor(
    private _userService: UserService,
    private _router: Router,
    private _proyectoService: ProyectoService,
    private modalService: NgbModal,
    private _appComponent: AppComponent
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
        Activo: new FormControl(true)
      });
      this.userForm.controls['Nombre'].disable();
    } else {
      this.userForm = new FormGroup({
        Nombre: new FormControl('', Validators.required),
        NombreCompleto: new FormControl('', Validators.required),
        Password: new FormControl('', Validators.required),
        Role: new FormControl(this.rol),
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
      },
      error => {
        if (error == 400) {
          this.ErrorMessage = "No pudimos actualizar el usuario.";
          this.Error = "400";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      });
  }

  public getUser() {
    // 1º Recogemos el user
    this.user = this._userService.user;

    //2º recogemos los objetos que marcaremos por defecto en los select
    this.rol = this.user.role;
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
