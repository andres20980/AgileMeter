import { Component, OnInit} from '@angular/core';
import { ProyectoService } from 'app/services/ProyectoService';
import { Router, ActivatedRoute } from '@angular/router';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { AppComponent } from 'app/app.component';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Office } from 'app/Models/Office';
import { Unity } from 'app/Models/Unit';
import { Linea } from 'app/Models/Linea';
import { User } from 'app/Models/User';
import { Equipo } from 'app/Models/Equipo';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from "rxjs";
import { startWith, map } from "rxjs/operators";

@Component({
  selector: 'app-add-team',
  templateUrl: './add-team.component.html',
  styleUrls: ['./add-team.component.scss']
})
export class AddTeamComponent implements OnInit {
  public addTeamsForm: FormGroup;
  public officeList: Office[];
  /*eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos
    public unityList: Unity[];
    public lineaList: Linea[];*/
  public UserNombre: User;
  public ErrorMessage: string = null;
  public MensajeNotificacion: string = null;
  public idEquipo: number = 0;
  public equipo: Equipo;
  public oficina: Office;
  OficinaEntity = new FormControl('', Validators.required)
  filteredOficinas: Observable<Office[]>;
  public compareOficina(o1: any, o2: any): boolean {
    return o1.oficinaId === o2.oficinaId;
  }
  /*//eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos
    unidad: Unity;
    compareUnidad(o1: any, o2: any): boolean {
      return o1.unidadId === o2.unidadId;
    }
    linea: Linea;
    compareLinea(o1: any, o2: any): boolean {
      return o1.lineaId === o2.lineaId;
    }
  */
  constructor(   //ProyectoService its teams service --> table proyectos its teams in DataBase
    private _teamsService: ProyectoService,
    private _router: Router,
    private _eventService: EventEmitterService,
    private _appComponent: AppComponent,
    private _translateService: TranslateService,
    private _routeParams: ActivatedRoute,
    fb: FormBuilder) {
    this.addTeamsForm = fb.group({
      hideRequired: false,
      floatLabel: 'auto',
    });
  }

  ngOnInit() {
    // if user not login --> redirect to login
    if (!this._teamsService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    this.getUserNombre();
    this.getAllOficinas();
    this.formValidate();
    if (this._teamsService.equipo != undefined) {
      this.getEquipo();
    }
  }// end onInit

  private _filterOficinas(val: any): Office[] | any{
    console.log(val['oficinaNombre'])
    let filterValue: any;
    if(typeof val === "object"){ 
      filterValue= val['oficinaNombre'].toLowerCase() } 
    else {
      filterValue= val.toLowerCase()
      }
    
    return this.officeList.filter(s => s['oficinaNombre'].toLowerCase().indexOf(filterValue) === 0)
  }

  displayFn(ofi: Office[]): string {
    return ofi ? ofi['oficinaNombre'] : ofi;
  }


  public getUserNombre() {
    this.UserNombre = new User(this._teamsService.UsuarioLogeado);
  }

  public formValidate() {//form validate
    this.addTeamsForm = new FormGroup({
      UserNombre: new FormControl(this.UserNombre.nombre),
      OficinaEntity: this.OficinaEntity,
      // OficinaEntity: new FormControl('', Validators.required), //officina
      /* Eliminados temporalmente hasta tener la lista de oficinas, unidades y proyectos 
      UnidadEntity: new FormControl('', Validators.required),//unidad
      LineaEntity: new FormControl('', Validators.required),//linea
      */
      Nombre: new FormControl('', Validators.required),//team
      Codigo: new FormControl('',Validators.maxLength(100)),//team

      ProjectSize: new FormControl("", [Validators.pattern('[0-9 ]{1,6}'), Validators.required]),

      //campos temporales hasta tener la lista de oficinas, unidades y proyectos
      //Oficina: new FormControl('', Validators.required),
      Unidad: new FormControl('', Validators.required),
      Proyecto: new FormControl('', Validators.required)

    });
  }

  //form validate
  public hasError = (controlName: string, errorName: string) => {
    return this.addTeamsForm.controls[controlName].hasError(errorName);
  }

  public getAllOficinas() {
    this._teamsService.getAllOficinas().subscribe(
      res => {
        this.officeList = res;
        this.filteredOficinas = this.OficinaEntity.valueChanges
        .pipe(
          startWith(''),
          map(ofi => ofi ? this._filterOficinas(ofi) : this.officeList.slice())
        );
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " Oficinas List Not Found.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      });
  }

  /* eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos
    //Solo se llama cuando se modifica un equipo
    public cargarCombosUnidadesYLineas() {
      this.getAllUnidadesDe(this.equipo.oficinaEntity);
      this.getAllLineasDe(this.equipo.unidadEntity);
    }
  

  
    public getAllUnidadesDe(oficina) {
      this.addTeamsForm.removeControl('UnidadEntity');
      this.addTeamsForm.addControl('UnidadEntity', new FormControl('', Validators.required));
      //limpiamos el campo que hubiera en linea
      this.addTeamsForm.removeControl('LineaEntity');
      this.addTeamsForm.addControl('LineaEntity', new FormControl('', Validators.required));
      //retur all unidades of a select oficina
      this._teamsService.getAllUnitDe(oficina).subscribe(
        res => {
          this.unityList = res;
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " Unidades List Not Found.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        });
    }
  
    public getAllLineasDe(unidades) {
      this.addTeamsForm.removeControl('LineaEntity');
      this.addTeamsForm.addControl('LineaEntity', new FormControl('', Validators.required));
      //retur all unidades of a select oficina
      this._teamsService.getAllLineasDe(unidades).subscribe(
        res => {
          this.lineaList = res;
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " Linea List Not Found.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        });
    }
  */
  public altaEquipo() {
    var form = this.addTeamsForm.value;
    this._teamsService.setTeam(form).subscribe(
      res => {
        this._router.navigate(['/backoffice/teamsmanager']);
        //this.MensajeNotificacion = "Equipo creado correctamente";
        this._translateService.get('ADD_TEAM.NOTIFICATION_TEAM_ADD').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        //Si el servidor tiene algún tipo de problema mostraremos este error
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " Alta team Not Found.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        //this.MensajeNotificacion = "Ups, lo sentimos, no pudimos crear el equipo";
        this._translateService.get('ADD_TEAM.NOTIFICATION_ERROR_TEAM_ADD').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  public getEquipo() {
    // 1º Recogemos el equipo
    this.equipo = this._teamsService.equipo;
    // 2º Cargamos los combos para ese equipo
    //this.cargarCombosUnidadesYLineas(); //eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos

    this.addTeamsForm.get('OficinaEntity').setValue(this.equipo.oficinaEntity);

    // 3º Creamos el form control para las validaciones
    /*//eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos
    this.addTeamsForm.get('UnidadEntity').setValue(this.equipo.unidadEntity);
    this.addTeamsForm.get('LineaEntity').setValue(this.equipo.lineaEntity);*/

    //eliminar cuando se pongan los desplegables
    //this.addTeamsForm.get('Oficina').setValue("");
    this.addTeamsForm.get('Unidad').setValue("");
    this.addTeamsForm.get('Proyecto').setValue("");

    this.addTeamsForm.get('Nombre').setValue(this.equipo.nombre);
    this.addTeamsForm.get('Codigo').setValue(this.equipo.codigo);
    this.addTeamsForm.get('ProjectSize').setValue(this.equipo.projectSize);
    this.addTeamsForm.addControl('Id', new FormControl(this.equipo.id));
    this.addTeamsForm.addControl('TestProject', new FormControl(this.equipo.testProject));
    this.addTeamsForm.addControl('Evaluaciones', new FormControl(this.equipo.evaluaciones));

    //campos temporales hasta tener la lista de oficinas, unidades y proyectos
    this.addTeamsForm.get('OficinaEntity').setValue({oficinaId: this.equipo.oficinaEntity.oficinaId, oficinaNombre: this.equipo.oficina});
    this.addTeamsForm.get('Unidad').setValue(this.equipo.unidad);
    this.addTeamsForm.get('Proyecto').setValue(this.equipo.proyecto);


    //4º recogemos los objetos que marcaremos por defecto en los select
    this.oficina = this.equipo.oficinaEntity;
    
    /*//eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos
    this.unidad = this.equipo.unidadEntity;
    this.linea = this.equipo.lineaEntity;*/
  }

  public updateEquipo() {
    var form = this.addTeamsForm.value;
    this._teamsService.updateTeam(form).subscribe(
      res => {
        this._router.navigate(['/backoffice/teamsmanager']);
        //this.MensajeNotificacion = "Equipo modificado correctamente";
        this._translateService.get('ADD_TEAM.NOTIFICATION_TEAM_MODIFY').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " Update team Not Found.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
        //this.MensajeNotificacion = "Ups, lo sentimos, no pudimos modificar el equipo.";
        this._translateService.get('ADD_TEAM.NOTIFICATION_ERROR_TEAM_MODIFY').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }
  public volver() {
    this._router.navigate(['/backoffice/teamsmanager']);
  }
}