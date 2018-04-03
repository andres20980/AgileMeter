import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../app.component';
import { User } from '../login/User';
import { Proyecto } from '../login/Proyecto';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

    public ListaDeProyectos:Array<Proyecto>;

  constructor(private _app: AppComponent) { }

  ngOnInit() {
      this.ListaDeProyectos = this._app._storageDataService.UserData.proyectosDeUsuario;
  }

}
