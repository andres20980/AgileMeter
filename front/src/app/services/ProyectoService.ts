import { Proyecto } from './../Models/Proyecto';
import { Unity } from './../Models/Unit';
import { Office } from './../Models/Office';

import { throwError as observableThrowError, Observable } from 'rxjs';

import { catchError, map } from 'rxjs/operators';
import { Injectable, Inject, isDevMode } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';
import { UserWithRole } from 'app/Models/UserWithRole';
import { StaticHelper } from './Helper';
import { Equipo } from 'app/Models/Equipo';

@Injectable()
export class ProyectoService {
  public url: string;
  public UsuarioLogeado: string;
  public UserLongName: string;
  public equipo:Equipo;

  constructor(private _http: Http,private _router:Router,
    private _appComponent: AppComponent) {

    //this.url = window.location.protocol +"//"+ window.location.hostname + ":60406/api/";    
    this.url = StaticHelper.ReturnUrlByEnvironment();


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
    return this._http.get(this.url + 'proyectos/' + userNombre + "/fullproyectos", { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  getAllAssessments() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'proyectos/allassessments', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge todos los proyectos de un usuario de la base de datos
  getProyectosDeUsuario() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'proyectos/' + this.UsuarioLogeado + '/proyectos', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  getProyectosDeUsuarioSeleccionado(user: UserWithRole) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'proyectos/' + user.nombre + "/proyectos", { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
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
    return this._http.get(this.url + 'proyectos/allnottestprojects', { headers: headers }).pipe(
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

  public getAllOficinas() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'oficina/allOficina', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  public getAllUnitDe(oficina: Office) {//devuelve todas las unidades de la oficina seleccionada  
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'unidad/allUnidad/' + oficina.oficinaId, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  public getAllLineasDe(unidad: Unity) {//devuelve todas las unidades de la unidad seleccionada
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'linea/allLinea/' + unidad.unidadId, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //add a team
  setTeam(equipo: Proyecto) {
    let params = JSON.stringify(equipo);
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'proyectos/proyectos/addTeam', params, { headers: headers }).pipe(
      map(res => res));
  }

  //Delete Team
  deleteTeam(team) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(team);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'proyectos/proyectos/delete', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));
  }

  getProyecto(idProyecto) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'proyectos/' + 'proyecto/' + idProyecto, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  updateTeam(equipo: Proyecto) {
    let params = JSON.stringify(equipo);
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.put(this.url + 'proyectos/proyectos/update', params, { headers: headers }).pipe(
      map(res => res));
  }  
}
