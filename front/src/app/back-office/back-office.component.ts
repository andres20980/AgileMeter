import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AppComponent } from '../app.component';
import { ProyectoService } from '../services/ProyectoService';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { UserService } from 'app/services/UserService';

@Component({
  selector: 'app-back-office',
  templateUrl: './back-office.component.html',
  styleUrls: ['./back-office.component.scss'],
  providers: [ProyectoService,UserService]
})


export class BackOfficeComponent implements OnInit {

  //public AdminOn = false;
  public updateUser: string = null;
  public ActiveSection : string = null;
  //public collapsedButtons : boolean = false;

  constructor(
    private _proyectoService: ProyectoService,
    public _router: Router,
    private _eventService: EventEmitterService,
    private _appComponent: AppComponent,
    route:ActivatedRoute) {
    this._eventService.eventEmitter.subscribe(
      (data) => {
        this.updateUser = data,
          setTimeout(() => { this.updateUser = null }, 2000)
      }
    );

    this._router.events.subscribe((e: any) => {
      //console.log( e);
      if(e.urlAfterRedirects == "/backoffice"){
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

  }

  // buttonClick(option :  string){
  //   this.ActiveSection = option;
  // }
}
