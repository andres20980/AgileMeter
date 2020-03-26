import { map } from 'rxjs/operators';
import { Assessment } from './../Models/Assessment';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, Input, Output, ViewChild, ViewChildren, EventEmitter, ElementRef, Renderer, OnChanges, SimpleChanges} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, SELECT_PANEL_INDENT_PADDING_X } from '@angular/material';
import { Evaluacion } from 'app/Models/Evaluacion';
import { AppComponent } from 'app/app.component';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'sorted-table',
  templateUrl: './sorted-table.component.html',
  styleUrls: ['./sorted-table.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
  ],
})

export class SortedTableComponent implements OnInit {

  @Input() dataInput: any
  @Input() dataInputMerged: any
  @Input() nombreEquipo: string;
  @Input() nombreAssessment: string;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChildren('mySelectAssessment') generalAssessment
  @ViewChild('mySelectAssessment') mySelectAssessment;
  @ViewChild('mySelectTeam') mySelectTeam: any;
  @ViewChild('mySelectOffice') mySelectOffice: any;
  dataSource: MatTableDataSource<Evaluacion>;
  dataSourceMerge: MatTableDataSource<Evaluacion>;s
  displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','puntuacion', 'notas', 'informe'];
  displayedColumnsScrum =  ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','equipo','eventos','herramientas','mindset','aplicacion','puntuacion', 'notas', 'informe'];
  displayedColumnsDevops =  ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','orgequipo','ciclovida','construccion','testing','despliegue','monitorizacion','aprovisionamiento','puntuacion', 'notas', 'informe'];

  public excelScrum: any[];
  public excelDevops: any[];
  public ListaDeOficinas: string[] = [];
  public OficinaSeleccionada: string[] = [];
  public ListaDeEquipos: string[] = [];
  public EquipoSeleccionado: string[] = [];
  public assessmentSeleccionado: string[] = [];
  public listaDeAssessment: string[] = [];
  public originDataSource: any;
  public originListOficina: string[] = [];
  public origingListEquipos: string[] = [];
  public originListaAssessment: string[] = [];
  public fieldsTable : any[];
  public objectTranslate : string;
  public scrumassmnt: boolean = false;
  public devopsassmnt: boolean = false;
  @Output() propagar = new EventEmitter<any>();
  @Output() propagar2 = new EventEmitter<any>();

  constructor(private _appComponent: AppComponent,  private _router: Router, private renderer: Renderer) { 
    this.fieldsTable = [
      ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
      ["userNombre", "EXCEL_USER",20,"", "String"],
      ["oficina", "EXCEL_OFFICE", 25,"", "String"],
      ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
      ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
      ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];

      this.excelScrum = [
      ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
      ["userNombre", "EXCEL_USER",20,"", "String"],
      ["oficina", "EXCEL_OFFICE", 25,"", "String"],
      ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
      ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
      ["equipo", "EXCEL_PT_SCRUM.TEAM", 20,"", "String"],
      ["eventos", "EXCEL_PT_SCRUM.EVENTS",20,"", "String"], 
      ["herramientas", "EXCEL_PT_SCRUM.TOOLS",20,"", "String"],
      ["mindset", "EXCEL_PT_SCRUM.MINDSET", 20,"", "String"],
      ["aplicacion", "EXCEL_PT_SCRUM.APP",20,"", "String"],
      ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]]

      this.excelDevops =  [
      ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
      ["userNombre", "EXCEL_USER",20,"", "String"],
      ["oficina", "EXCEL_OFFICE", 25,"", "String"],
      ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
      ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
      ["orgequipo", "EXCEL_PT_DEVOPS.ORG_TEAM", 20,"", "String"],
      ["ciclovida", "EXCEL_PT_DEVOPS.LIFECYCLE", 20,"", "String"],
      ["construccion", "EXCEL_PT_DEVOPS.BUILDING", 20,"", "String"],
      ["testing", "EXCEL_PT_DEVOPS.TESTING", 20,"", "String"],
      ["'despliegue", "EXCEL_PT_DEVOPS.DEPLOYMENT", 20,"", "String"],
      ["monitorizacion", "EXCEL_PT_DEVOPS.MONITORING", 20,"", "String"],
      ["aprovisionamiento", "EXCEL_PT_DEVOPS.", 20,"", "String"],
      ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];
  }



  ngOnInit()
  {
      this.fieldsTable = [
        ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
        ["userNombre", "EXCEL_USER",20,"", "String"],
        ["oficina", "EXCEL_OFFICE", 25,"", "String"],
        ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
        ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
        ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];
      
  this.objectTranslate = "PREVIOUS_EVALUATION";

    this.GetPagination();
    this._appComponent.pushBreadcrumb("BREADCRUMB.FINISHED_EVALUATIONS", "/finishedevaluations");
    this.originDataSource = this.dataInput
    this.originListOficina = this.dataInput.map(x => x.oficina).reduce((x,y) => x.includes(y) ? x : [...x, y],[]);
    this.origingListEquipos = this.dataInput.map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    this.originListaAssessment = this.dataInput.map(x => x).reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]);
    this.ListaDeOficinas= this.dataInput.map(x => x.oficina).reduce((x,y) => x.includes(y) ? x : [...x, y],[]);
    this.listaDeAssessment = this.dataInput.map(x => x).reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]);
    this.ListaDeEquipos = this.dataInput.map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      
    if(this.nombreEquipo) {
      this.EquipoSeleccionado.push(this.nombreEquipo);
      this.assessmentSeleccionado.push(this.nombreAssessment);
      this.filterData('equipo');
    } 

    //this.propagar.emit(false)


  }

  public GetPagination()
  {
    this.dataSource = new MatTableDataSource(this.dataInput);
    console.log(this.dataSource);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public SaveDataToPDF(evaluacion: EvaluacionInfo)
  {
    this._appComponent._storageDataService.EvaluacionToPDF = evaluacion;
    this._appComponent.pushBreadcrumb(evaluacion.nombre, null);
    this._appComponent.pushBreadcrumb(evaluacion.assessmentName, null);
    let pipe = new DatePipe('en-US');
    this._appComponent.pushBreadcrumb(pipe.transform(evaluacion.fecha, 'dd/MM/yyyy'), null);
    this._appComponent.pushBreadcrumb("BREADCRUMB.RESULTS", "/evaluationresults");
    this._router.navigate(['/evaluationresults']);
  }

  closingSelect(){
    this.mySelectTeam.close();
    this.mySelectOffice.close();
    this.mySelectAssessment.close();
  }

  applyFilter(filterValue: string)
  {
    this.dataSource.filter = filterValue.trim().toLowerCase();
    console.log(this.dataSource)
  }

  filterData(origen: string)
  {
  
    let oficinaSel = this.OficinaSeleccionada.length !== 0 ? this.OficinaSeleccionada : this.originListOficina;
    let equipoSel = this.EquipoSeleccionado.length !== 0 ? this.EquipoSeleccionado : this.origingListEquipos;
    let assessmentSel = this.assessmentSeleccionado.length !== 0 ? this.assessmentSeleccionado : this.originListaAssessment;

    let selected = {oficina: oficinaSel , team: equipoSel, assessment: assessmentSel};
    
   
    
    if(origen === 'oficina') {
      this.ListaDeEquipos = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      this.listaDeAssessment = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    }

    if(origen === 'equipo') {
      // Si pusla equipo nos da igual lo que tenga (por el momento)
       //selected.oficina = [];
      //   alert(this.EquipoSeleccionado.length)
       if(this.EquipoSeleccionado.length === 0)
       {  
        //  selected.oficina = [];
        //  this.OficinaSeleccionada = [];

       } else {
        this.assessmentSeleccionado = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
        this.OficinaSeleccionada= this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
        selected.oficina = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[])
       }

      
     // this.OficinaSeleccionada = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    }
    // if(origen === 'assessment') {
    //   this.ListaDeOficinas = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    //   this.ListaDeEquipos = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    // }
   
     // viejo nuevo
    let nuevo = this.originDataSource.filter(x =>selected.oficina.includes(x.oficina) && selected.team.includes(x.nombre) && selected.assessment.includes(x.assessmentName));
    this.dataSource.data = nuevo;

    console.log("autofilter", this.OficinaSeleccionada, this.EquipoSeleccionado, this.assessmentSeleccionado, "lis", this.listaDeAssessment)
    if(this.OficinaSeleccionada.length === 0 && this.EquipoSeleccionado.length === 0 && this.assessmentSeleccionado.length === 0) {
      this.dataSource.data = this.originDataSource;
      this.ListaDeEquipos = this.originDataSource.reduce((x,y) => x.includes(y.nombre) ? x : [...x, y.nombre],[]);
      this.ListaDeOficinas = this.originDataSource.reduce((x,y) => x.includes(y.oficina) ? x : [...x, y.oficina],[]);
      this.listaDeAssessment = this.originDataSource.reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[])
      
    }

    this.showColumnAssessment();
    this.activateChartToParent();
  }


  autoticFilter(origin: string)
  {   
    if(this.listaDeAssessment.length === 1)this.assessmentSeleccionado = this.listaDeAssessment

    if(origin === "assessment" && this.listaDeAssessment.length === 1) this.assessmentSeleccionado = [];
    return;
  }

  showColumnAssessment()
  {
    if(this.assessmentSeleccionado.length === 1) {
      if( this.assessmentSeleccionado[0] === "SCRUM") {
        this.displayedColumns = this.displayedColumnsScrum;
        this.fieldsTable = this.excelScrum
        this.scrumassmnt = true;
      } else if(this.assessmentSeleccionado[0] === "DEVOPS") {
        this.displayedColumns = this.displayedColumnsDevops;
        this.fieldsTable = this.excelDevops
        this.devopsassmnt = true;
      }

      this.addColumnExcel(this.assessmentSeleccionado[0]);
      
    } else {
      this.displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','puntuacion', 'notas', 'informe'];
      this.fieldsTable = [["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
      ["userNombre", "EXCEL_USER",20,"", "String"],
      ["oficina", "EXCEL_OFFICE", 25,"", "String"],
      ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
      ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
      ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];
      this.scrumassmnt = false;
      this.devopsassmnt = false;
      this.popColumnExcel()
      this.propagar.emit(false);
    } 
  }

  activateChartToParent()
  {
    if(this.EquipoSeleccionado.length === 1 && this.assessmentSeleccionado.length === 1) {
      this.propagar.emit(this.dataSource.data)
    } else {
      this.propagar.emit(false);
    }
  }

  addColumnExcel(assessment: string)
  {
    this.fieldsTable.map((x, i) => {
      if(x[0].includes("assessmentName")) {
        if(assessment === "SCRUM") this.fieldsTable.splice(i, 0, ...this.excelScrum)
        if(assessment === "DEVOPS") this.fieldsTable.splice(i, 0, ...this.excelDevops)
      }
    })
  }

  popColumnExcel()
  {
    this.fieldsTable.map((x, i) => {
      if(x[0].includes("equipo")) this.fieldsTable.splice(i, this.excelScrum.length)
      if(x[0].includes("orgequipo"))this.fieldsTable.splice(i, this.excelDevops.length)
    })
  }
}
