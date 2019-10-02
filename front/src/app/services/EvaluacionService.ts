
import { throwError as observableThrowError, Observable, of } from 'rxjs';
import { Injectable, Inject, isDevMode } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';



import { Evaluacion } from 'app/Models/Evaluacion';
import { EvaluacionCreate } from 'app/Models/EvaluacionCreate';
import { EvaluacionFilterInfo } from 'app/Models/EvaluacionFilterInfo';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { map, tap, catchError } from 'rxjs/operators';
import { User } from 'app/Models/User';
import { AppComponent } from 'app/app.component';
import { ProyectoService } from './ProyectoService';
import { EvaluacionInfoWithProgress } from 'app/Models/EvaluacionInfoWithProgress';
import { StaticHelper } from './Helper';

@Injectable()
export class EvaluacionService {
  public identity;
  public token;
  public url: string;
  public UsuarioLogeado: string;

  constructor(private _http: Http,
    private _appComponent: AppComponent,
    private _proyectoService: ProyectoService) {

    //this.url = window.location.protocol +"//"+ window.location.hostname + ":60406/api/";    
    this.url = StaticHelper.ReturnUrlByEnvironment();

    var local = localStorage.getItem("user");
    var storage = this._appComponent._storageDataService.UserData;
    if (local != null && local != undefined) {
      this.UsuarioLogeado = localStorage.getItem("user");
    }
    else if (storage != undefined && storage != null) {
      this.UsuarioLogeado = this._appComponent._storageDataService.UserData.nombre;
    }
  }

  //Este metodo recoge todas las evaluaciones de la base de datos
  getEvaluaciones() {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge una evaluacion si existe mediante una id de evaluacion
  getEvaluacion(id) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones/' + id, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge una evaluacion con datos extendidos si existe mediante una id de evaluacion
  GetEvaluationInfoFromIdEvaluation(idEvaluation) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones/evaluacion/' + idEvaluation + '/evaluationInfo', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge una evaluacion con datos extendidos si existe mediante una id de proyecto
  getEvaluacionInfo(id) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones/proyecto/' + id + '/info', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Nos permite recoger información de las envaluaciones filtrada y paginada
  getEvaluacionInfoFiltered(NumPag: number, idProject: number, EvaluacionFiltrar: EvaluacionFilterInfo) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(EvaluacionFiltrar);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones/proyecto/' + idProject + '/info/page/' + NumPag, params, { headers: headers }).pipe(
      map(res => res.json()),
      // tap(r => console.log("OBSERVAAAAAAAAAAAAABLE",r)),
      catchError(this.errorHandler));
  }

  //Nos permite recoger información de todas las envaluaciones filtrada y paginada
  getAllEvaluacionInfoFiltered(NumPag: number, EvaluacionFiltrar: EvaluacionFilterInfo) {
    var codigoIdioma = this._appComponent._storageDataService.codigoIdioma;
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(EvaluacionFiltrar);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones/'+ this.UsuarioLogeado +'/proyecto/all/info/page/' + NumPag +'/lan/' + codigoIdioma, params, { headers: headers }).pipe(
      map(res => res.json()),
      // tap(r => console.log("OBSERVAAAAAAAAAAAAABLE",r)),
      catchError(this.errorHandler));
  }

  //Nos permite recoger información de las evaluaciones filtrada para la gráfica
  GetEvaluationsWithSectionsInfo(idProject: number, EvaluacionFiltrar: EvaluacionFilterInfo) {
    var codigoIdioma = this._appComponent._storageDataService.codigoIdioma;
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(EvaluacionFiltrar);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones/proyecto/' + idProject + '/sectionsinfo/' + codigoIdioma, params, { headers: headers }).pipe(
      map(res => res.json()),
      // tap(r => console.log("OBSERVAAAAAAAAAAAAABLE",r)),
      catchError(this.errorHandler));
  }

  //Nos permite recoger información de las evaluaciones filtrada para la gráfica
  GetEvaluationsWithProgress(idProject: number, EvaluacionFiltrar: EvaluacionFilterInfo) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(EvaluacionFiltrar);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones/proyecto/' + idProject + '/progress/', params, { headers: headers }).pipe(
      map(res => res.json()),
      // tap(r => console.log("OBSERVAAAAAAAAAAAAABLE",r)),
      catchError(this.errorHandler));
  }

  //Nos permite recoger información de todas las evaluaciones de todos los equipos asignados a un usuario filtrada para la gráfica
  GetAllEvaluationsWithProgress(EvaluacionFiltrar: EvaluacionFilterInfo) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(EvaluacionFiltrar);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones/alluserprojects/' + this.UsuarioLogeado + '/progress/', params, { headers: headers }).pipe(
      map(res => res.json()),
      catchError(this.errorHandler));
  }


  //Este metodo recoge una evaluacion de un proyecto si existe mediante una id de proyecto
  getEvaluacionFromProject(id) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token,
    });
    return this._http.get(this.url + 'evaluaciones/proyecto/' + id, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Este metodo recoge el número de evaluaciones de un proyecto o todos los proyectos
  getNumEvals(id) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones/proyecto/' + id + '/num', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Devuelve una evaluacion si existe una que no se completo en ese proyecto
  getIncompleteEvaluacionFromProject(id) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones/proyecto/' + id + '/continue', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Devuelve una evaluacion si existe una que no se completo en ese proyecto
  getIncompleteEvaluacionFromProjectAndAssessment(projectId, assessmentId) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + `evaluaciones/proyecto/${projectId}/assessment/${assessmentId}/continue`, { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Nos permite incluir una evaluacion en la base de datos
  addEvaluacion(evaluacion: EvaluacionCreate) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(evaluacion);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones', params, { headers: headers }).pipe(
      map(res => res.json()),
      catchError(this.errorHandler));
  }

  //Nos permite realizar un update de una evaluacion en la base de datos
  updateEvaluacion(evaluacion: Evaluacion) {
    //evaluacion.userNombre = this._proyectoService.UsuarioLogeado;
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(evaluacion);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });

    return this._http.put(this.url + 'evaluaciones', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));
  }

  //Implementamos este metodo para permitir la recogida de los errores y su gestión
  errorHandler(error: Response) {
    return observableThrowError(error.status);
  }

  //Metodo encargado de calcular el porcentaje respondido de la evaluacion
  CalculateEvaluationProgress(idEvaluacion, idAssessment) {
    let Token = this._appComponent.ComprobarUserYToken();
    let headers = new Headers({
      'Authorization': Token
    });
    return this._http.get(this.url + 'evaluaciones/proyecto/' + idEvaluacion + '/assessment/' + idAssessment + '/totalprogress', { headers: headers }).pipe(
      map((response: Response) => response.json()),
      catchError(this.errorHandler));
  }

  //Metodo encargado de borrar una evaluacion
  EvaluationDelete(evaluationId: number) {
    let Token = this._appComponent.ComprobarUserYToken();
    let params = JSON.stringify(evaluationId);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': Token
    });
    return this._http.post(this.url + 'evaluaciones/evaluacion/delete/', params, { headers: headers }).pipe(
      map(res => res),
      catchError(this.errorHandler));


  }
}

