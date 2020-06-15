import { ProyectoService } from 'app/services/ProyectoService';
import { Component, OnInit, HostListener } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Role } from 'app/Models/Role';
import { UserService } from 'app/services/UserService';
import { Router } from '@angular/router';
import { AppComponent } from 'app/app.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { TranslateService } from '@ngx-translate/core';
import { StorageDataService } from 'app/services/StorageDataService';

@Component({
  selector: 'edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss'],
  providers: [ProyectoService, UserService, StorageDataService,EventEmitterService]
})

export class EditUserComponent implements OnInit {
  public ScreenWidth;
  public ErrorMessage: string = null;
  public Error: string = null;
  public userForm: FormGroup;
  public user: any = null;
  public MensajeNotificacion: string = null;
  public disable = true;

  
  public rolList: Role[];
  rol: Role = { id: 1, role: "Usuario" };
  idiomaFavorito = "es";

  compareRol(o1: any, o2: any): boolean {
    return o1.id === o2.id;
  }



  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.ScreenWidth = window.innerWidth;
  }

  constructor(
    private _proyectoService: ProyectoService,
    private _router: Router,
    private _appComponent: AppComponent,
    private _userService: UserService,
    private modalService: NgbModal,
    private _eventService: EventEmitterService,
    private _translateService: TranslateService,
    private _storageDataService: StorageDataService
  ) {


  }



  ngOnInit() {
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    this._appComponent.pushBreadcrumb("BREADCRUMB.EDITUSER", "/editUser");
    this.user = new Object();
    this.formValidate();
    this.getAllRol(this._appComponent._storageDataService.Role);
    console.log("Role: ", this._appComponent._storageDataService.Role);
  }


    //form validate
  public hasError = (controlName: string, errorName: string) => {
    return this.userForm.controls[controlName].hasError(errorName);
  }

  public getAllRol(roleId) {
    this._userService.getAllRoles().subscribe(
      res => {
        this.rolList = res;
        let filt = this.rolList.filter(x => x.id == roleId);
        this.userForm.patchValue({Role: filt[0]});

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

  public formValidate() {

    this.user.idiomaFavorito = this._storageDataService.codigoIdioma;
    if (this._storageDataService.codigoIdioma == 1){
      this.user.idiomaFavorito = 'es'
    }else{
      this.user.idiomaFavorito = 'en'
    }

    this.userForm = new FormGroup({
      Nombre: new FormControl(this._proyectoService.UsuarioLogeado, Validators.required),
      NombreCompleto: new FormControl(null, Validators.required),
      Password: new FormControl(null),
      Role: new FormControl(null),
      Activo: new FormControl(true),
      IdiomaFavorito: new FormControl(this.user.idiomaFavorito)
    });

      this._userService.getUser(this._proyectoService.UsuarioLogeado).subscribe(r => {
      this.user.nombreCompleto = r.nombreCompleto;
      this.user.nombre = this._proyectoService.UsuarioLogeado;
      this.userForm.patchValue({NombreCompleto: r.nombreCompleto})
      })

      

      this.userForm.controls['Nombre'].disable();
      
  }
  

  public comprobarDatos(content) {
    var form = this.userForm.value;
    //le asignamos el nombre al form ( como el campo nombre esta deshabilitado se recoge como null)
    form.nombre = this._proyectoService.UsuarioLogeado;
    if (form.password == "") {
      form.password = null;
    }

    //si no se modifica
    else {
      this.updateUsuario(form);
    }
  }

  public updateUsuario(form) {
    console.log("form: ", form);
    this._userService.updateUser(form).subscribe(
      res => {
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

  public volver() {
    this._router.navigate(["/home"]);
  }

  
}


