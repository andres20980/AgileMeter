import { Assessment } from './../Models/Assessment';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, Input, Output,ViewChild, EventEmitter, Renderer,OnChanges, SimpleChanges} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
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

export class SortedTableComponent implements OnInit{

  @Input() dataInput: any
  @Input() dataInputMerged: any
  @Input() recoverProject: string;
  @Input() enableScrumColums: boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('mySelectAssessment') mySelectAssessment: any;
  @ViewChild('mySelectTeam') mySelectTeam: any;
  @ViewChild('mySelectOffice') mySelectOffice: any;
  dataSource: MatTableDataSource<Evaluacion>;
  dataSourceMerge: MatTableDataSource<Evaluacion>;
  displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','puntuacion', 'notas', 'informe'];
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

  constructor(private _appComponent: AppComponent,  private _router: Router, private renderer: Renderer) { }

  ngOnInit()
  {
      this.fieldsTable = [
        ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
        ["userNombre", "EXCEL_USER",20,"", "String"],
        ["oficina", "EXCEL_OFFICE", 25,"", "String"],
        ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
        ["equipo", "EXCEL_PT_TEAM",  12,"0.00%", "Percentage"], 
        ["eventos", "EXCEL_PT_EVENTS",  12,"0.00%", "Percentage"], 
        ["herramientas", "EXCEL_PT_TOOLS", 12,"0.00%", "Percentage"], 
        ["mindset", "EXCEL_PT_MINDSET", 12,"0.00%", "Percentage"],
        ["aplicacion", "EXCEL_PT_APP", 12,"0.00%", "Percentage"],
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

    this.propagar.emit(false)
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
  }

  filterData(origen: string)
  {
    let oficinaSel = this.OficinaSeleccionada.length !== 0 ? this.OficinaSeleccionada : this.originListOficina;
    let equipoSel = this.EquipoSeleccionado.length !== 0 ? this.EquipoSeleccionado : this.origingListEquipos;
    let assessmentSel = this.assessmentSeleccionado.length !== 0 ? this.assessmentSeleccionado : this.originListaAssessment;

    let selected = {oficina: oficinaSel , team: equipoSel, assessment: assessmentSel};
    let nuevo = this.originDataSource.filter(x =>selected.oficina.includes(x.oficina) && selected.team.includes(x.nombre) && selected.assessment.includes(x.assessmentName));

    if(origen === 'oficina') {
      this.ListaDeEquipos = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      this.listaDeAssessment = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      if(this.ListaDeEquipos.length === 1 && this.EquipoSeleccionado.length > 1) {
        this.ListaDeEquipos = this.EquipoSeleccionado;
      }
    }
    if(origen === 'equipo') {
      this.ListaDeOficinas = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      this.listaDeAssessment = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      if(this.ListaDeOficinas.length === 1 && this.OficinaSeleccionada.length > 1) {
        this.ListaDeOficinas = this.OficinaSeleccionada;
      }
    }
    if(origen === 'assessment') {
      this.ListaDeOficinas = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      this.ListaDeEquipos = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    }

    this.dataSource.data = nuevo;

    if(this.OficinaSeleccionada.length === 0 && this.EquipoSeleccionado.length === 0 && this.assessmentSeleccionado.length === 0) {
      this.dataSource.data = this.originDataSource;
      this.ListaDeEquipos = this.originDataSource.reduce((x,y) => x.includes(y.nombre) ? x : [...x, y.nombre],[]);
      this.ListaDeOficinas = this.originDataSource.reduce((x,y) => x.includes(y.oficina) ? x : [...x, y.oficina],[]);
      this.listaDeAssessment = this.originDataSource.reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[])
      
    }
    this.showColumnAssessment();
    this.activateChartToParent();
  }

  showColumnAssessment()
  {
    if(this.assessmentSeleccionado.length === 1) {
      if( this.assessmentSeleccionado[0] === "SCRUM") {
        this.displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','equipo','eventos','herramientas','mindset','aplicacion','puntuacion', 'notas', 'informe'];
        this.scrumassmnt = true;
      } else if(this.assessmentSeleccionado[0] === "DEVOPS") {
        this.displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','orgequipo','ciclovida','construccion','testing','despligue','monitorizacion','aprovisionamiento','puntuacion', 'notas', 'informe'];
        this.devopsassmnt = true;
      }

    } else {
      this.displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','puntuacion', 'notas', 'informe'];
      this.scrumassmnt = false;
      this.devopsassmnt = false;
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
}
