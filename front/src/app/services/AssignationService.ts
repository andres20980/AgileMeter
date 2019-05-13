import { throwError as observableThrowError } from 'rxjs';
import { Injectable, isDevMode } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

import { map, catchError } from 'rxjs/operators';
import { AppComponent } from 'app/app.component';
import { ProyectoService } from './ProyectoService';
import { StaticHelper } from './Helper';


@Injectable()
export class AssignationService {
  public identity;
  public token;
  public url: string;

  constructor(private _http: Http,
    private _appComponent: AppComponent,
    private _proyectoService: ProyectoService) {
    
    //this.url = window.location.protocol +"//"+ window.location.hostname + ":60406/api/";    
    this.url = StaticHelper.ReturnUrlByEnvironment();
  }

  //Devuelve la asignacion de la ultima pregunta cuya respuesta haya sido modificada o respondida
  AssignationLastQuestionUpdated(evaluationId) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'asignaciones/evaluacion/' + evaluationId + '/last-question-updated', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Implementamos este metodo para permitir la recogida de los errores y su gestión
  errorHandler(error: Response) {
    return observableThrowError(error.status);
  }
}

