import { Component, OnInit, ViewChild, SimpleChanges } from '@angular/core';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { EvaluacionFilterInfo } from 'app/Models/EvaluacionFilterInfo';
import { Proyecto } from 'app/Models/Proyecto';
import { AppComponent } from 'app/app.component';
import { Router } from '@angular/router';
import { EvaluacionService } from '../services/EvaluacionService';
import { AssignationService } from '../services/AssignationService';
import { Evaluacion } from 'app/Models/Evaluacion';
import { Observable } from 'rxjs/Rx';
import { Http } from '@angular/http';
import { map } from 'rxjs/operators';
import { interval ,  Subscription } from 'rxjs';
import { ProyectoService } from 'app/services/ProyectoService';
import { Role } from 'app/Models/Role';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { ChartsModule, BaseChartDirective } from 'ng2-charts/ng2-charts';
import { DatePipe } from '@angular/common';
import { SectionInfo } from 'app/Models/SectionInfo';
import { SectionService } from 'app/services/SectionService';
import { forEach } from '@angular/router/src/utils/collection';
import { SectionsLevel } from 'app/pdfgenerator/pdfgenerator.component';
import { Assessment } from 'app/Models/Assessment';
import { EvaluacionInfoWithProgress } from 'app/Models/EvaluacionInfoWithProgress';
import { EvaluacionInfoWithSections } from 'app/Models/EvaluacionInfoWithSections';
import {TranslateService} from '@ngx-translate/core';

export interface ComplianceLevels {

  assesmentId: number,
  name: string,
  sections:{
    sectionId: number, name: string, 
    levels:  {level: number, value: number}[]
  }[]

}
 
export interface AssesmentEv{
  id: number,
  name: string
}
 
@Component({
  selector: 'app-pendingevaluation',
  templateUrl: './pendingevaluation.component.html',
  styleUrls: ['./pendingevaluation.component.scss'],
  providers: [EvaluacionService, ProyectoService, SectionService, AssignationService]
})
export class PendingEvaluationComponent implements OnInit {
  public clicked: boolean = true;
  public EvaluacionFiltrar: EvaluacionFilterInfo = { 'nombre': '', 'estado': 'false', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': 0 };
  public Typing: boolean = false;
  public permisosDeUsuario: Array<Role> = [];
  public ListaDeEvaluacionesPaginada: Array<EvaluacionInfoWithProgress>;
  public nEvaluaciones: number = 0;
  public UserName: string = "";
  public Project: Proyecto = { 'id': null, 'nombre': '',codigo: null, 'fecha': null, numFinishedEvals:0, numPendingEvals: 0};
  public Mostrar = false;
  public ErrorMessage: string = null;
  public NumEspera = 750;
  public MostrarInfo = false;
  public Timeout: Subscription;
  public textoModal: string;
  public anadeNota = null;
  public fechaPicker: NgbDate;
  public MostrarTabla: boolean = true;
  public MostrarGrafica: boolean = false;
  public EvaluationsWithSectionInfo: Array<EvaluacionInfoWithSections>;



    //Datos de la barras
    public barChartType: string = 'line';
    public barChartLegend: boolean = true;
    public AgileComplianceTotal: number = 100;
    public ListaSeccionesAgileCompliance: number[] = [];
    public ListaPuntuacion: { label: string, backgroundColor: string, borderColor: string, data: Array<any>, fill: string, lineTension: number, pointRadius: number, pointHoverRadius: number, borderWidth: number }[] = [];
    public ListaNombres: string[] = [];
    public ComplianceLevels: ComplianceLevels;
    public MaxLevelReached: number;
    public barChartOptions: any;
    public ListaAssessments : AssesmentEv[] = [];
    public selectedAssessment: AssesmentEv;

  public Admin: boolean = false;
  public ListaDeProyectos: Array<Proyecto> = [];
  public ProyectoSeleccionado: boolean = false;


  //Para actualizar la grafica
  @ViewChild(BaseChartDirective) public chart: BaseChartDirective;



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
    //Recogemos los proyectos y realizamos comprobaciones
    var Role;
    this.Project = this._appComponent._storageDataService.UserProjectSelected;
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    this.UserName = this._proyectoService.UsuarioLogeado;
    //this.getUserRole();
    //Recogemos el rol del usuario
    this._proyectoService.getRolesUsuario().subscribe(
      res => {

        this.permisosDeUsuario = res;
        //Si no hay errores y son recogidos busca si tienes permisos de usuario
        for (let num = 0; num < this.permisosDeUsuario.length; num++) {
          if (this.permisosDeUsuario[num].role == "Administrador") {
            if (this.Project == null || this.Project == undefined || this.Project.id == -1) {
              this.Project = { id: 0, nombre: '',codigo: null, fecha: null, numFinishedEvals:0, numPendingEvals: 0};
              this.Admin = true;
            }
          }
        }

        //Comprueba que tenga  un proyecto seleccionado y si no es asi lo devuelve a home
        if (this.Project == null || this.Project == undefined) {
          this._router.navigate(['/home']);
        } else if (this.Project.id == -1 && !this.Admin) {
          this._router.navigate(['/home']);
        } else {
          this.MostrarInfo = true;
        }
        
        this.GetPaginacion();


      },
      error => {
        //Si el servidor tiene algún tipo de problema mostraremos este error
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + "No pudimos recoger los datos del usuario.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      },
      () => {
        //this.Restablecer();
      });

      //this._appComponent.pushBreadcrumb("Evaluaciones pendientes", "/pendingevaluations");
      this._appComponent.pushBreadcrumb("BREADCRUMB.PENDING_ASSESSMENTS", "/pendingevaluations");
      this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);

    if (this.Project.fecha != null) {
      //Para que no de error en modo development
      setTimeout(() => {
        this._appComponent.anadirUserProyecto(this.UserName, this._proyectoService.UserLongName, this.Project.nombre);
      },0);
    } else {
      //Para que no de error en modo development
      setTimeout(() => {
        this._appComponent.anadirUserProyecto(this.UserName, this._proyectoService.UserLongName, "Todos");
      },0);

    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.EvaluacionInfoWithProgress){
      this.GetPaginacion();
    }
  }

  //Da los datos a las diferentes listas que usaremos para las graficas
  public shareDataToChart() {
    this.ListaPuntuacion = [];
    this.ListaNombres = [];
    this.MaxLevelReached = 0;
    let listaSections : number[][] = [];
    let listaSectionLevels: SectionsLevel[][] = [];
    let index:number = 0;
    let colorList: string[] = ["#E74C3C", "#3498DB","#F1C40F" ,"#9B59B6",  "#F39C12", "#33CCCC", "#34495E"]
    let levelColorList : string[] = ["#FDB90040", "#78C00040", "#00c00940",  "#00c06340", "#00c09e40", "#00b5c040"]


    for(var i = 0; i <  this.EvaluationsWithSectionInfo.length + 1; i++) {
      listaSections[i] = [];
      if(i <  this.EvaluationsWithSectionInfo.length){
        listaSectionLevels[i] = this.getSectionLevels(this.EvaluationsWithSectionInfo[i].sectionsInfo);
      }
      else{
        listaSectionLevels[i] = [];
      }
     
     
      for(var j = 0; j< this.EvaluationsWithSectionInfo.length; j++) {
        listaSections[i][j] = 0;
      }
    }
    //Cogemos los datos a añadir
    for (var i = this.EvaluationsWithSectionInfo.length - 1; i >= 0; i--) {
      
      for(var j: number = 0; j < listaSections.length; j++) {
        if(j >= this.EvaluationsWithSectionInfo[0].sectionsInfo.length){
          listaSections[j][index] = this.EvaluationsWithSectionInfo[i].puntuacion;
        }
        else{         
          listaSections[j][index] = listaSectionLevels[i][j].percentOverLevel + listaSectionLevels[i][j].levelReached * 100;//this.EvaluationsWithSectionInfo[i].sectionsInfo[j].respuestasCorrectas;
          if(listaSectionLevels[i][j].levelReached > this.MaxLevelReached){
            this.MaxLevelReached = listaSectionLevels[i][j].levelReached;
          }
        }
        
      }
      index++;
      var pipe = new DatePipe('en-US');
      this.ListaNombres.push(pipe.transform(this.EvaluationsWithSectionInfo[i].fecha, 'dd/MM/yyyy'));
    }


    for(var j: number = 0; j <  this.EvaluationsWithSectionInfo[0].sectionsInfo.length + 1; j++) {

      if(j >= this.EvaluationsWithSectionInfo[0].sectionsInfo.length){
        this.ListaPuntuacion.push({
          data: listaSections[j], label: "Global", backgroundColor: "#2ECC71", fill: 'false', lineTension : 0.1,
          borderColor: "#2ECC71", pointRadius: 2, pointHoverRadius: 4, borderWidth: 3});
      }
      else{
        this.ListaPuntuacion.push({
          data: listaSections[j], label: this.EvaluationsWithSectionInfo[0].sectionsInfo[j].nombre, backgroundColor: colorList[j], fill: 'false', lineTension : 0.1,
          borderColor: colorList[j], pointRadius: 2, pointHoverRadius: 4, borderWidth: 3});
      }
    }

    for(var i: number = 0; i <= this.MaxLevelReached; i++) {
      let level: number[] = [];
      for(var j: number = 0; j < listaSections[0].length; j++) {
        level[j] = i * 100 + 100;
      }
      if(i == 0){
        this.ListaPuntuacion.push({
          data: level, label: 'aux' + i, backgroundColor: levelColorList[i], fill: 'origin', lineTension : 0.1,
          borderColor: levelColorList[i], pointRadius: 0, pointHoverRadius: 0, borderWidth: 0.1});
      }
      else{
        this.ListaPuntuacion.push({
          data: level, label: 'aux' + i, backgroundColor: levelColorList[i], fill: '-1', lineTension : 0.1,
          borderColor: levelColorList[i], pointRadius: 0, pointHoverRadius: 0, borderWidth: 0.1});
      }
    }

    this.setBarChartOptions();

    //Para actualizar la grafica una vez esté visible
    setTimeout(() => {

      if (this.chart && this.chart.chart && this.chart.chart.config) {
        this.chart.chart.config.data.labels = this.ListaNombres;
        this.chart.chart.config.data.datasets = this.ListaPuntuacion;
        this.chart.chart.config.data.options = this.barChartOptions;
        this.chart.chart.update();
      }
    }, 300);

  }

  public changeChartAssessment($event){
    let filter: EvaluacionFilterInfo = new EvaluacionFilterInfo("","","","","", this.selectedAssessment.id);
    this.GetChartData(filter);
  }

  public GetChartData(filter: EvaluacionFilterInfo){
    this._evaluacionService.GetEvaluationsWithSectionsInfo( this.Project.id, filter)
      .subscribe(
        res => {
          this.EvaluationsWithSectionInfo = res.evaluacionesResult;
          let i:number = 0; 
          this.EvaluationsWithSectionInfo.forEach(ev => {
            this._sectionService.getSectionInfo(ev.id,ev.assessmentId).subscribe(
              res =>{
                ev.sectionsInfo = res;
                i++;
                if(i == this.EvaluationsWithSectionInfo.length){
                  this.loadComplianceLevels(this.selectedAssessment.id)
                  //this.shareDataToChart();
                }
              }
            );
          });
         
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
        });
  }


  //Recarga los elementos en la pagina en la que se encuentra 
  public GetPaginacion() {
    this.Mostrar = false;
    this._evaluacionService.GetEvaluationsWithProgress(this.Project.id, this.EvaluacionFiltrar)
      .subscribe(
        res => {
          this.nEvaluaciones = res.numEvals;
          this.ListaDeEvaluacionesPaginada = res.evaluacionesResult; 
          this.Mostrar = true;
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
        });
      }

  

  public VolverInicio() {
    this._router.navigate(['/home']);
  }

  public getUserRole(){
    this._proyectoService.getRolesUsuario().subscribe(
      res => {
        var permisosDeUsuario = res;
        this._appComponent._storageDataService.Role = permisosDeUsuario.role;
      },
      error => {
        
      });
  }

  //Opciones para la grafica
  public setBarChartOptions(){
    this.barChartOptions = {
    scaleShowVerticalLines: false,
    showLines: true,
    legend: {
      labels: {
          filter: function(item, chart) {
              // Logic to remove a particular legend item goes here
              return !item.text.includes('aux');
          }
      }
  },
    tooltips: {
      cornerRadius: 2,
      filter: function (tooltipItem, data) {
        var label = data.datasets[tooltipItem.datasetIndex].label;
        if (label.includes('aux')) {
          return false;
        } else {
          return true;
        }
    },
    animation: false,
      callbacks: {
        label: function (tooltipItem, data) {
          // const datasetLabel = data.datasets[tooltipItem.datasetIndex].label || '';
          // console.log(data);
          // var labels = [];
          // for (let index = 0; index < this.ListaDeSectionInfo[tooltipItem.datasetIndex].length; index++) {
          //   const sectionInfo = this.ListaDeSectionInfo[tooltipItem.datasetIndex][index];
          //     labels.push(sectionInfo.nombre + ': ' + sectionInfo.respuestasCorrectas + '%');
          //   }
          let nivel: string = '%  del nivel mínimo';
          //if(Number(tooltipItem.yLabel) > 100){
            nivel = '%  del nivel ' +  Math.trunc(Number(tooltipItem.yLabel) / 100 + 1);
          //}
          return  data.datasets[tooltipItem.datasetIndex].label + ': ' + Math.round((tooltipItem.yLabel%100) * 10)/10 + nivel;
        },
        // footer: function(tooltipItem, data) {
        //   return " Total" + ':     ' + tooltipItem[0].yLabel + '%';
        // },
        // title: function(tooltipItem, data) {
        //   return " " + tooltipItem[0].xLabel;
        // },
      }
    },
    scales: {
      yAxes: [{
        ticks: {
          steps: this.MaxLevelReached * 10 + 10,
          stepValue: 10,
          maxTicksLimit: this.MaxLevelReached * 10 + 10,
          max: this.MaxLevelReached * 100 + 100,
          min: 0,
          callback: function(value, index, values) {
            if (Number(value) % 100 == 0 && Number(value) >= 100){
              // if(Number(value) == 100){
              //   return 'Nivel mín.';
              // }
              // else if(Number(value) > 100){
              //   return 'Nivel ' + (Number(value) / 100 - 1);
              // }
              return 'Nivel ' + (Number(value) / 100 );
          }
          else
            return Number(value)%100 + '%';
         }
        },
        gridLines: {
          display: true,
          //color: '#34f6c2'
        }
      }],
      xAxes: [{
        gridLines: {
          display:false
        }
      }]
    }
  };
  }

  private loadComplianceLevels(assessmentId: number){
    this.http.get('assets/compliance_levels.json').pipe(map(res => res.json()))
      .subscribe((assessments) => {
        for (var a of assessments) {
             if (a.assesmentId == assessmentId) {
               this.ComplianceLevels = a;
               this.shareDataToChart();
               return;
             }
        }
      });
  }

  private getSectionLevels(sections: Array<SectionInfo>): Array<SectionsLevel> {

    let ListaSectionLevels: Array<SectionsLevel> = [];
    let i: number = 0;
    for (var s of this.ComplianceLevels.sections) {
      let section: SectionInfo = sections[i];
      let levelReached: number = 0;
      let percentOverLevel: number = section.respuestasCorrectas;
      for (var l of s.levels) {
        if (percentOverLevel > l.value && s.levels.length - 1 > levelReached) {
          percentOverLevel = percentOverLevel - l.value;
          levelReached++;
        }
        else {
          if(s.levels.length > levelReached && percentOverLevel < s.levels[levelReached].value){
            percentOverLevel = Math.round((percentOverLevel / s.levels[levelReached].value * 100) * 10) / 10;
          }
          else{
            percentOverLevel = 100;
          }
          const sl: SectionsLevel = { levelReached, percentOverLevel };
          ListaSectionLevels.push(sl);
          break;
        }
      }
      i++;
    }
    return ListaSectionLevels;
}

}
