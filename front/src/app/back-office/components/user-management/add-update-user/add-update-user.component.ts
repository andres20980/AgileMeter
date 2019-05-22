import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Role } from 'app/Models/Role';
import { UserService } from 'app/services/UserService';
import { Router } from '@angular/router';
import { AppComponent } from 'app/app.component';

@Component({
  selector: 'app-add-update-user',
  templateUrl: './add-update-user.component.html',
  styleUrls: ['./add-update-user.component.scss']
})
export class AddUpdateUserComponent implements OnInit {
  public ErrorMessage: string = null;
  public Error: string = null;
  public userForm: FormGroup;

  public rolList: Role[];
  rol: Role = { id: 1, role: "Usuario" };
  compareRol(o1: any, o2: any): boolean {
    return o1.id === o2.id;
  }
  constructor(private _UserService: UserService, private _router: Router, private _appComponent: AppComponent) { }

  ngOnInit() {
    this.getAllRol();
    this.formValidate();
  }

  //form validate
  public formValidate() {
    this.userForm = new FormGroup({
      Nombre: new FormControl('', Validators.required),
      NombreCompleto: new FormControl('', Validators.required),
      Password: new FormControl('', Validators.required),
      Role: new FormControl(this.rol),

    });
  }

  //form validate
  public hasError = (controlName: string, errorName: string) => {
    return this.userForm.controls[controlName].hasError(errorName);
  }

  public getAllRol() {
    this._UserService.getAllRoles().subscribe(
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
    this._UserService.addUser(form).subscribe(
      res => {
        this._router.navigate(['/backoffice/usermanagement']);
      },
      error => {
        if (error == 400) {
          this.ErrorMessage = "Error : Ese usuario ya existe.";
          this.Error ="400";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      });
  }
}
