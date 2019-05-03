
import { throwError as observableThrowError, Observable } from 'rxjs';

import { catchError, map } from 'rxjs/operators';
import { Injectable, Inject, isDevMode } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';



import { AppComponent } from '../app.component';
import { UserWithRole } from 'app/Models/UserWithRole';

@Injectable()
export class ProyectoService {
  public url: string;
  public UsuarioLogeado: string;
  public UserLongName: string;

  constructor(private _http: Http,
    private _appComponent: AppComponent) {

    /*if (isDevMode()) {
      this.url = "http://localhost:60406/api/";
    } else {
      var loc = window.location.href;
      var index = 0;
      for (var i = 0; i < 3; i++) {
        index = loc.indexOf("/", index + 1);
      }

      this.url = loc.substring(0, index) + "/api/";
    }*/
    this.url = window.location.protocol+"//"+ window.location.hostname + ":60406/api/";


  }

  //Este metdo nos permite verificar si el usuario ya esta logeado en la web
  //Puede estar recordado o puede estar iniciado solo para esta sesión
  //Si no esta logeado de ninguna manera enviara false
  public verificarUsuario() {
    var local = localStorage.getItem("user");
    var storage = this._appComponent._storageDataService.UserData;
    if (local != null && local != undefined) {
      this.UsuarioLogeado = localStorage.getItem("user");
      this.UserLongName = localStorage.getItem("userlongname");
      return true;
    } else if (storage != undefined && storage != null) {
      this.UsuarioLogeado = this._appComponent._storageDataService.UserData.nombre;
      this.UserLongName = this._appComponent._storageDataService.UserLongName;
      return true;
    } else {
      return false;
    }
  }

  //Este metodo devuelve todos los proyectos de todos los usuarios
  getAllProyectos(userNombre: string) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/' + userNombre + "/fullproyectos", { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  getAllAssessments() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/allassessments', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge todos los proyectos de un usuario de la base de datos
  getProyectosDeUsuario() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/' + this.UsuarioLogeado + '/proyectos', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  getProyectosDeUsuarioSeleccionado(user: UserWithRole) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/' + user.nombre + "/proyectos", { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler),);
  }

  //Este metodo recoge un proyecto de un usuario si existe mediante un nombre de usuario y su id de proyecto
  getOneProjecto(idProyecto) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/' + this.UsuarioLogeado + '/proyecto/' + idProyecto, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Devuelve un listado con todos los proyectos dados de alta en el sistema que no pertenezca al grupo de pruebas de los usuarios
  GetAllNotTestProjects() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/allnottestprojects', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo devuelve todos los permisos de un usuario
  getRolesUsuario() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/' + this.UsuarioLogeado + '/roles', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Implementamos este metodo para permitir la recogida de los errores y su gestión
  errorHandler(error: Response) {
    return observableThrowError(error.status);
  }

}
