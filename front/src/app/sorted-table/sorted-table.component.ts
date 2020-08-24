import { AssessmentColumns } from './../Models/AssessmentColumns';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, Input, Output, ViewChild, ViewChildren, EventEmitter, Renderer} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Evaluacion } from 'app/Models/Evaluacion';
import { AppComponent } from 'app/app.component';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { EvaluacionService } from '../services/EvaluacionService';
import { EnumRol } from 'app/Models/EnumRol';

@Component({
  selector: 'sorted-table',
  templateUrl: './sorted-table.component.html',
  providers: [EvaluacionService],
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
  dataSourceMerge: MatTableDataSource<Evaluacion>;
  assessmentColumns = new AssessmentColumns();
  displayedColumns: Array<any> = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','puntuacion', 'notas', 'informe'];
  displayedColumnsScrum = this.assessmentColumns.displayedScrumColumns
  displayedColumnsDevops = this.assessmentColumns.displayedDevopsColumns
  displayedColumnsKanban = this.assessmentColumns.displayedKanbanColumns
  displayedColumnsRemote = this.assessmentColumns.displayedRemoteColumns

  public excelScrum: any[];
  public excelDevops: any[];
  public excelKanban: any[];
  public excelRemote: any[];
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
  @Output() propagar = new EventEmitter<any>();
  public UserRole: number = 0;
  public rol: EnumRol = new EnumRol();

  public columnInfoAssmnt: Array<any>
  public sectionColumn: boolean = false;

  constructor(private _appComponent: AppComponent,  private _router: Router, private renderer: Renderer, private evaluacion: EvaluacionService) { 
    this.fieldsTable = [
      ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
      ["userNombre", "EXCEL_USER",20,"", "String"],
      ["oficina", "EXCEL_OFFICE", 25,"", "String"],
      ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
      ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
      ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];

      this.excelScrum = this.assessmentColumns.excelScrum
      this.excelDevops = this.assessmentColumns.excelDevops
      this.excelKanban = this.assessmentColumns.execelKanban
      this.excelRemote = this.assessmentColumns.execelRemote
  }


  ngOnInit()
  {

    this.UserRole = this._appComponent._storageDataService.Role;
    // this.fieldsTable = [
    //     ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
    //     ["userNombre", "EXCEL_USER",20,"", "String"],
    //     ["oficina", "EXCEL_OFFICE", 25,"", "String"],
    //     ["nombre", "EXCEL_TEAM", 50,"##?##", "String"],
    //     ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
    //     ["puntuacion", "EXCEL_SCORE", 12,"0.00%", "Percentage"]];
      
    this.objectTranslate = "PREVIOUS_EVALUATION";

    this.GetPagination();
    this._appComponent.pushBreadcrumb("BREADCRUMB.FINISHED_EVALUATIONS", "/finishedevaluations");
    this.originDataSource = this.dataInput
    this.originListOficina = this.dataInput.map(x => x.oficina).reduce((x,y) => x.includes(y) ? x : [...x, y],[]).sort();
    this.origingListEquipos = this.dataInput.map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    this.originListaAssessment = this.dataInput.map(x => x).reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]).sort();
    this.ListaDeOficinas= this.dataInput.map(x => x.oficina).reduce((x,y) => x.includes(y) ? x : [...x, y],[]).sort();
    this.listaDeAssessment = this.dataInput.map(x => x).reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]).sort();
    this.ListaDeEquipos = this.dataInput.map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]).sort();     

    if(this.nombreEquipo) {
      this.EquipoSeleccionado.push(this.nombreEquipo);
      this.assessmentSeleccionado.push(this.nombreAssessment);
      this.filterData('equipo');
    } 

    //this.propagar.emit(false)

    this.dataSource.filterPredicate = function (data, filter: string): boolean {
      let date = new Date(data.fecha);
      return data.nombre.toLowerCase().includes(filter)
        || data.assessmentName.toLowerCase().includes(filter)
        || data.oficina.toLowerCase().includes(filter)
        || data.sectionsInfo.find(x => x.puntuacion.toString().toLowerCase().includes(filter))
        || (data.userNombre != null && data.userNombre.toLowerCase().includes(filter))
        || ((date.getDate() < 10 ? "0" : "") + date.getDate() + "/" + (date.getMonth() < 10 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getFullYear()).includes(filter)
        ;
    };


  }

  saveNotas(model: Evaluacion): void {
    if (this.UserRole == this.rol.Administrador || this.UserRole == this.rol.Evaluador) {
      this.evaluacion.updateEvaluacion(model, false).subscribe(
        res => {
          // console.log("success");
        },
        error => {
          // console.log("error");
        },
        () => {
          // this.Mostrar = true;
        });
    }
  }

  public GetPagination()
  {
    this.dataSource = new MatTableDataSource(this.dataInput);
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
  }

  filterData(origen: string)
  {
  
    let oficinaSel = this.OficinaSeleccionada.length !== 0 ? this.OficinaSeleccionada : this.originListOficina;
    let equipoSel = this.EquipoSeleccionado.length !== 0 ? this.EquipoSeleccionado : this.origingListEquipos;
    let assessmentSel = this.assessmentSeleccionado.length !== 0 ? this.assessmentSeleccionado : this.originListaAssessment;

    let selected = {oficina: oficinaSel , team: equipoSel, assessment: assessmentSel};
    
   
    
    if(origen === 'oficina') {

      //selected.team = [];
      if(this.EquipoSeleccionado.length >= 1) this.EquipoSeleccionado = [];
      if(this.assessmentSeleccionado.length >= 1) this.assessmentSeleccionado = [];

      this.ListaDeEquipos = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]).sort();;
      this.listaDeAssessment = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]).sort();;

      selected.team = this.ListaDeEquipos;
      selected.assessment = this.listaDeAssessment
    }

    if(origen === 'equipo') {
       
       
      if(this.EquipoSeleccionado.length === 0) {
        // cuando no hay equipo seleccionado se queda a 0
        this.assessmentSeleccionado = [];
      } else  {
        this.assessmentSeleccionado = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]).sort();;
        selected.assessment = [];
        if(this.assessmentSeleccionado.length == 1){
          selected.assessment = this.assessmentSeleccionado;
        }else
        {
          selected.assessment = this.assessmentSeleccionado;
          this.assessmentSeleccionado = [];
        }
      }

     // this.OficinaSeleccionada = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    }
    if(origen === 'assessment') {
      this.ListaDeOficinas = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]).sort();
      this.ListaDeEquipos = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]).sort();;
    }
    
     // viejo nuevo
    let nuevo = this.originDataSource.filter(x =>selected.oficina.includes(x.oficina) && selected.team.includes(x.nombre) && selected.assessment.includes(x.assessmentName));
    this.dataSource.data = nuevo;

    if(this.OficinaSeleccionada.length === 0 && this.EquipoSeleccionado.length === 0 && this.assessmentSeleccionado.length === 0) {
      this.dataSource.data = this.originDataSource;
      this.ListaDeEquipos = this.originDataSource.reduce((x,y) => x.includes(y.nombre) ? x : [...x, y.nombre],[]).sort();;
      this.ListaDeOficinas = this.originDataSource.reduce((x,y) => x.includes(y.oficina) ? x : [...x, y.oficina],[]).sort();
      this.listaDeAssessment = this.originDataSource.reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]).sort();
      
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
      } else if(this.assessmentSeleccionado[0] === "DEVOPS") {
        this.displayedColumns = this.displayedColumnsDevops;
      }else if(this.assessmentSeleccionado[0] === "KANBAN") {
        this.displayedColumns = this.displayedColumnsKanban;
      }else if(this.assessmentSeleccionado[0] === "REMOTO") {
        this.displayedColumns = this.displayedColumnsRemote;
      }

     //this.displayedColumns = this.assessmentColumns.columnExcel(this.assessmentSeleccionado[0])
      this.columnInfoAssmnt = this.assessmentColumns.columnSection(this.assessmentSeleccionado[0])

      this.addColumnExcel(this.assessmentSeleccionado[0]);

    } else {
      this.displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','puntuacion', 'notas', 'informe'];
      this.popColumnExcel();
      this.sectionColumn = false;
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
    // if (!(this.scrumassmnt||this.devopsassmnt||this.kanbanassmnt || this.remoteassmnt)) {
    if (!(this.sectionColumn)) {
      this.fieldsTable.map((x, i) => {
        if(x[0].includes("assessmentName")) {
          if(assessment === "SCRUM") {
            this.fieldsTable.splice(i, 0, ...this.excelScrum);
            //this.scrumassmnt = true;
          }
          if(assessment === "DEVOPS") {
            this.fieldsTable.splice(i, 0, ...this.excelDevops)
            //this.devopsassmnt = true;
          }
          if(assessment === "KANBAN") {
            this.fieldsTable.splice(i, 0, ...this.excelKanban)
            //this.kanbanassmnt = true;
          }
          if(assessment === "REMOTO") {
            this.fieldsTable.splice(i, 0, ...this.excelRemote)
            //this.remoteassmnt = true;
          }
        }

        this.sectionColumn = true;
       
    })
  }
    
  }

  popColumnExcel()
  {
    this.fieldsTable.map((x, i) => {
      if(x[0]=="equipo") this.fieldsTable.splice(i, this.excelScrum.length)
      if(x[0]=="orgequipo") this.fieldsTable.splice(i, this.excelDevops.length) 
      if(x[0]=="kbnequipo") this.fieldsTable.splice(i, this.excelKanban.length)
      if(x[0]=="rmtespacio") this.fieldsTable.splice(i, this.excelRemote.length) 
    })
  }
}
