import { getTestBed } from '@angular/core/testing';
import { Component, OnInit, ViewChild, SimpleChanges} from '@angular/core';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { EvaluacionFilterInfo } from 'app/Models/EvaluacionFilterInfo';
import { Proyecto } from 'app/Models/Proyecto';
import { AppComponent } from 'app/app.component';
import { Router } from '@angular/router';
import { EvaluacionService } from '../services/EvaluacionService';
import { AssignationService } from '../services/AssignationService';
import { Evaluacion } from 'app/Models/Evaluacion';
import { Http } from '@angular/http';
import { ProyectoService } from 'app/services/ProyectoService';
import { Role } from 'app/Models/Role';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { DatePipe } from '@angular/common';
import { SectionInfo } from 'app/Models/SectionInfo';
import { SectionService } from 'app/services/SectionService';
import { forEach } from '@angular/router/src/utils/collection';
import { SectionsLevel } from 'app/pdfgenerator/pdfgenerator.component';
import { EvaluacionInfoWithProgress } from 'app/Models/EvaluacionInfoWithProgress';
import { EvaluacionInfoWithSections } from 'app/Models/EvaluacionInfoWithSections';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-pendingevaluation',
  templateUrl: './pendingevaluation.component.html',
  styleUrls: ['./pendingevaluation.component.scss'],
  providers: [EvaluacionService, ProyectoService, SectionService, AssignationService]
})
export class PendingevaluationComponent implements OnInit {
  public clicked: boolean = true;
  public EvaluacionFiltrar: EvaluacionFilterInfo = { 'nombre': '', 'estado': 'false', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': 0, 'oficinas':[], equipos:[], 'idAssessment': [] };
  public Typing: boolean = false;
  public permisosDeUsuario: Array<Role> = [];
  public listaDeEvaluacionesPaginada: Array<EvaluacionInfoWithProgress>;
  public nEvaluaciones: number = 0;
  public UserName: string = "";
  public Project: Proyecto = { 'id': null, 'nombre': '',codigo: null, 'fecha': null, numFinishedEvals:0, numPendingEvals: 0, oficina:null, proyecto: ''};
  public Mostrar = false;
  public ErrorMessage: string = null;
  public activeReload: boolean;
  public MostrarInfo = false;
  public textoModal: string;
  public anadeNota = null;
  public fechaPicker: NgbDate;
  public MostrarTabla: boolean = false;
  public MostrarGrafica: boolean = false;
  public EvaluationsWithSectionInfo: Array<any>;


    //Datos de la barras
    public barChartType: string = 'line';
    public barChartLegend: boolean = true;
    public AgileComplianceTotal: number = 100;
    public ListaSeccionesAgileCompliance: number[] = [];
    public ListaPuntuacion: { label: string, backgroundColor: string, borderColor: string, data: Array<any>, fill: string, lineTension: number, pointRadius: number, pointHoverRadius: number, borderWidth: number }[] = [];
    public ListaPending: Object
    public barChartOptions: any;
    public ListaAssessments : string[] = [];
    public selectedAssessment: string[] = [];

  public Admin: boolean = false;
  public ListaDeProyectos: Array<Proyecto> = [];
  public ProyectoSeleccionado: boolean = false;


  constructor(
    private _appComponent: AppComponent,
    private _router: Router,
    public _evaluacionService: EvaluacionService,
    private _proyectoService: ProyectoService,
    private _sectionService: SectionService,
    private modalService: NgbModal,
    private http: Http,
    private _translateService : TranslateService
  ) { }

  ngOnInit() {
    
    if(this._appComponent._storageDataService.AssessmentSelected != null){
      if(this._appComponent._storageDataService.ProjectSelected && this._appComponent._storageDataService.AssessmentsSelected &&this._appComponent._storageDataService.OfficeSelected){
        this.ListaPending = {oficina: this._appComponent._storageDataService.OfficeSelected, equipo: this._appComponent._storageDataService.ProjectSelected.proyecto + " - "+ this._appComponent._storageDataService.ProjectSelected.nombre, assessment: this._appComponent._storageDataService.AssessmentSelected.assessmentName}
        } else {
          this.ListaPending = null
        }
    }
    

    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    this.getEvaluations();

  }

    reloadDataInput(active: boolean)
    {
      if(active) {
        this.getEvaluations(active)
      }
    }

    getEvaluations(active = false)  {
    this._evaluacionService.GetAllEvaluationsWithProgress(this.EvaluacionFiltrar)
      .subscribe(
        res => {
          this.nEvaluaciones = res.numEvals;
          this.listaDeEvaluacionesPaginada = res.evaluacionesResult.reduce((acc, item) => {
            item.nombre = item.nombre.replace("##?##", " - ")
            return [...acc, item]
          },[])

        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " No pudimos recoger la información de las evaluaciones, lo sentimos.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        }, () => {
          this.MostrarTabla = true
        })
        this._appComponent.pushBreadcrumb("BREADCRUMB.PENDING_ASSESSMENTS", "/finishpending");
    }
  }