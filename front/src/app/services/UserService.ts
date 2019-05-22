
import { throwError as observableThrowError, Observable } from 'rxjs';

import { catchError, map } from 'rxjs/operators';
import { Injectable, Inject, isDevMode } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

import { User } from 'app/Models/User';
import { AppComponent } from 'app/app.component';
import { UserWithRole } from 'app/Models/UserWithRole';
import { UserProject } from 'app/Models/UserProject';
import { StaticHelper } from './Helper';
import { UserCreateUpdate } from 'app/Models/UserCreateUpdate';

@Injectable()
export class UserService {
  public identity;
  public token;
  public url: string;

  constructor(private _http: Http,
    private _appComponent: AppComponent) {
	
      //this.url = window.location.protocol +"//"+ window.location.hostname + ":60406/api/";    
    this.url = StaticHelper.ReturnUrlByEnvironment();

  }

  //Este metodo recoge todos los usuarios de la base de datos
  getUsers() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/AllUsers', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge todos los roles de la base de datos
  getAllRoles() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'users/AllRoles', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge todos los roles de la base de datos
  updateUser(User: UserWithRole) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(User);

    //console.log(params)

    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.put(this.url + 'users', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));
  }

  //Nos permite recoger un token si existe el usuario
  SignUpMe(Usuario: User) {
    let params = JSON.stringify(Usuario);
    let headers = new Headers({
      'Content-Type': 'application/json'
    });
    return this._http.post(this.url + 'Token', params, { headers: headers }).pipe(
      map(res => res.json()),
      catchError(this.errorHandler));
  }

  addUserProject(userProject: UserProject) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(userProject);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    
    return this._http.post(this.url + 'users/addUserProject/', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));
  }

  removeUserProject(userProject: UserProject) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(userProject);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    
    return this._http.post(this.url + 'users/removeUserProject/', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));
  }

  //Implementamos este metodo para permitir la recogida de los errores y su gestión
  errorHandler(error: Response) {
    return observableThrowError(error.status);
  }

  addUser(user:UserCreateUpdate){console.log(user);
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(user);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    
    return this._http.post(this.url + 'users/add', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));
  }

}
