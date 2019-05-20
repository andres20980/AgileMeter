import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { AppComponent } from '../app.component';
import { ProyectoService } from '../services/ProyectoService';
import { EventEmitterService } from 'app/services/event-emitter.service';

@Component({
  selector: 'app-back-office',
  templateUrl: './back-office.component.html',
  styleUrls: ['./back-office.component.scss'],
  providers: [ProyectoService]
})


export class BackOfficeComponent implements OnInit {

  //public AdminOn = false;
  public updateUser: string = null;

  constructor(
    private _proyectoService: ProyectoService,
    public _router: Router,
    private _eventService: EventEmitterService,
    private _appComponent: AppComponent) {
    this._eventService.eventEmitter.subscribe(
      (data) => {
        this.updateUser = data,
          setTimeout(() => { this.updateUser = null }, 2000)
      }
    );
  }


  ngOnInit() {

    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }

  }

  teamsOptions() {
    var x = document.getElementById("addteam");
    if (x.style.display == "block")
      x.style.display = "none";
    else
      x.style.display = "block";
  }

  altaEquipo() {
    this._proyectoService.equipo = null;
  }
}
