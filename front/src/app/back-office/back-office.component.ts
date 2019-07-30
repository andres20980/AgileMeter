import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AppComponent } from '../app.component';
import { ProyectoService } from '../services/ProyectoService';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { UserService } from 'app/services/UserService';
import { EnumRol } from 'app/Models/EnumRol';


@Component({
  selector: 'app-back-office',
  templateUrl: './back-office.component.html',
  styleUrls: ['./back-office.component.scss'],
  providers: [ProyectoService, UserService]
})

export class BackOfficeComponent implements OnInit {

  //public AdminOn = false;
  public MensajeNotificacion: string = null;
  public MensajeNotificacionError: boolean = false;
  public ActiveSection: string = null;
  public rol: EnumRol = new EnumRol();
  //public collapsedButtons : boolean = false;


  constructor(
    private _proyectoService: ProyectoService,
    public _router: Router,
    private _eventService: EventEmitterService,
    private _appComponent: AppComponent,
    route: ActivatedRoute) {
    this._eventService.eventEmitter.subscribe(
      res => {
        this.MensajeNotificacionError = res.error;
        this.MensajeNotificacion = res.message,
          setTimeout(() => { this.MensajeNotificacion = null }, 4000)
      }
    );

    this._router.events.subscribe((e: any) => {
      //console.log( e);
      if (e.urlAfterRedirects == "/backoffice") {
        this.ActiveSection = null;
      }
    });
  }

  ngOnInit() {
    //this.collapsedButtons = false;
    this.ActiveSection = null;
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    //console.log(this._appComponent._storageDataService.Role);
    if (this._appComponent._storageDataService.Role != this.rol.Administrador) {
      this._router.navigate(['/home']);
    }
  }
}
