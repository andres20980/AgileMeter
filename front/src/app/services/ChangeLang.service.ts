import { UserPermission } from './../login/login.component';
import { Router } from '@angular/router';
import { StorageDataService } from './StorageDataService';
import { UserService } from './UserService';
import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { StaticHelper } from './Helper';
import { map } from 'rxjs/operators'

@Injectable({
  providedIn: 'root'
})
export class ChangeLangService {
  public url: string;

constructor(private _http: Http, private storage: StorageDataService, private _router: Router) { 
  this.url = StaticHelper.ReturnUrlByEnvironment();
}

updateLangUser(nombre: string, lang: string, user: UserPermission )
{
  let Token = this.autoCheckToken();
  let params =  JSON.stringify(user)
  let headers = new Headers({
    'Content-Type': 'application/json',
    'Authorization': Token
  });
  return this._http.put(this.url + 'users/'+nombre+'/idioma/'+lang , params, { headers: headers }).pipe(
    map(res => res)).subscribe(x => x)
}


autoCheckToken()
{
    //Recogemos los datos
    var Token = this.storage.TokenUser;
    var User = this.storage.UserData;
    var TokenStorage = localStorage.getItem("tokenuser");
    var UserStorage = { 'nombre': localStorage.getItem("user"), 'password': localStorage.getItem("passuser") }

    //Recoger del localstorage tambien
    //Si no tiene nada nos devuelve a login sino nos da el token
    if (Token == null || Token == undefined || User == null || User == undefined || Token == "" || User.nombre == "" || User.password == "") {
      TokenStorage = localStorage.getItem("tokenuser");
      UserStorage = { 'nombre': localStorage.getItem("user"), 'password': localStorage.getItem("passuser") };
      if (TokenStorage == null || TokenStorage == undefined || UserStorage == null || UserStorage == undefined || TokenStorage == "" || UserStorage.nombre == "" || UserStorage.password == "") {
        this._router.navigate(["/login"]);
      } else {
        return "Bearer " + TokenStorage;
      }
    } else {
      return "Bearer " + this.storage.TokenUser;
    }
}


}
