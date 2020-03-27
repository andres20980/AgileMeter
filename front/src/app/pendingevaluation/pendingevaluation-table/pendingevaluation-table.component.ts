import { Component, OnInit, ViewChild, Input, Renderer, Output, EventEmitter, DoCheck, KeyValueDiffer, KeyValueDiffers } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatCellDef, MatSelect } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Router } from "@angular/router";
import { AppComponent } from 'app/app.component';
import { EvaluacionInfoWithProgress } from 'app/Models/EvaluacionInfoWithProgress';
import { EvaluacionService } from '../../services/EvaluacionService';
import { SectionService } from 'app/services/SectionService';
import { AssignationService } from 'app/services/AssignationService';
import { Evaluacion } from 'app/Models/Evaluacion';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SectionInfo } from 'app/Models/SectionInfo';
import { DatePipe } from '@angular/common';
import { Proyecto } from 'app/Models/Proyecto';
import { Assessment } from 'app/Models/Assessment';
import { ProyectoService } from 'app/services/ProyectoService';

@Component({
  selector: 'pendingevaluation-table',
  templateUrl: './pendingevaluation-table.component.html',
  styleUrls: ['./pendingevaluation-table.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
    
  ],
})
export class PendingevaluationTableComponent implements OnInit, DoCheck {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('mySelectOffice') mySelectOffice: any;
  @ViewChild('mySelectTeam') mySelectTeam: any;
  @ViewChild('mySelectAssessment') mySelectAssessment: any;
  @Input() listaDeEvaluaciones: any;
  @Input() pendientesSelect: any;
  @Output() reloadData = new EventEmitter<boolean>();
  dataSource: MatTableDataSource<EvaluacionInfoWithProgress>;
  userRole: number;
  evaluationProgress: number;
  selectedEvaluacionInfoWithProgress;
  displayedColumns = ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName', 'progress', 'actions'];
  public ListaDeDatos: Array<SectionInfo> = [];
  public Evaluacion: Evaluacion = null;
  public sectionId: number;
  public currentRows: number;
  public fieldsTable : any[];
  public objectTranslate : string;
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
  
  constructor(private _evaluacionService: EvaluacionService,
    private _assignationService: AssignationService,
    private _sectionService: SectionService,
    private _router: Router,
    private _appComponent: AppComponent,
    private modalService: NgbModal,
    private _proyectoService: ProyectoService,
    private renderer: Renderer,
    private key: KeyValueDiffers) {}

  ngOnInit() {
    this.fieldsTable = [
      ["fecha", "EXCEL_DATE", 12,"dd/mm/yyyy", "Date"],
      ["userNombre", "EXCEL_USER",20,"", "String"],
      ["oficina", "EXCEL_OFFICE", 25,"", "String"],
      ["nombre", "EXCEL_TEAM", 50,"##?##", "String"], 
      ["assessmentName", "EXCEL_ASSESSMENT", 20,"", "String"],
      ["progress", "EXCEL_PROGRESS", 12,"0.00%", "Percentage"]];
    this.objectTranslate = "PENDING_EVALUATION";
    this.LoadDataSource();
    this.originDataSource = this.dataSource.data;
    this.originListOficina = this.dataSource.data.map(x => x).reduce((x,y) => x.includes(y.oficina) ? x : [...x, y.oficina],[]);
    this.origingListEquipos = this.dataSource.data.map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
    this.originListaAssessment = this.dataSource.data.map(x => x).reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]);
    this.ListaDeOficinas= this.dataSource.data.map(x => x).reduce((x,y) => x.includes(y.oficina) ? x : [...x, y.oficina],[]);
    this.listaDeAssessment = this.dataSource.data.map(x => x).reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[]);
    this.ListaDeEquipos = this.dataSource.data.map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);

    console.log(this.pendientesSelect) // this.pendientesSelect['oficina'] && this.pendientesSelect['equipo'] && this.pendientesSelect['assessment']
    if(this.pendientesSelect){
      this.OficinaSeleccionada.push(this.pendientesSelect['oficina'])
      this.EquipoSeleccionado.push(this.pendientesSelect['equipo']);
      this.assessmentSeleccionado.push(this.pendientesSelect['assessment'])
      this.filterData('equipo')
    }
    
   }

  ngDoCheck()
  {
    if(this.listaDeEvaluaciones.length < this.currentRows) {
      this.dataSource = new MatTableDataSource(this.listaDeEvaluaciones);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.currentRows = this.listaDeEvaluaciones.length
    }
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  private LoadDataSource() {
    this.dataSource = new MatTableDataSource(this.listaDeEvaluaciones);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.currentRows = this.dataSource.data.length

    this.dataSource.filterPredicate = function (data, filter: string): boolean {
      let date = new Date(data.fecha);
      return data.nombre.toLowerCase().includes(filter)
        || data.assessmentName.toLowerCase().includes(filter)
        || data.oficina.toLowerCase().includes(filter)
        || (data.userNombre != null && data.userNombre.toLowerCase().includes(filter))
        || (data.progress != null && data.progress.toString().includes(filter))
        || ((date.getDate() < 10 ? "0" : "") + date.getDate() + "/" + (date.getMonth() < 10 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getFullYear()).includes(filter)
        ;
    };
  }

  public ContinueEvaluation(evaluation: EvaluacionInfoWithProgress) {
    this._evaluacionService.getEvaluacion(evaluation.id).subscribe(
      res => {
        this._appComponent._storageDataService.Evaluacion = res;
        this._appComponent._storageDataService.Evaluacion.assessmentName = evaluation.assessmentName;
        this._appComponent._storageDataService.AssessmentSelected = { 'assessmentId': evaluation.assessmentId, 'assessmentName': evaluation.assessmentName };
        this.GetAssignation(evaluation.id);
      }
    );
  }

    //Metodo encargado de eliminar la evaluacion pasandole una evaluacionId
    // TODO: controlar posibles errores
    public EvaluationDelete(evaluationId: number) {
      this._evaluacionService.EvaluationDelete(evaluationId).subscribe(res => res,
         error => error, 
         () => {
              this.currentRows = this.dataSource.data.length
              this.reloadData.emit(true);})
    }
    // Metodo encargado de abrir la ventana confirmando la eliminacion de la evaluacion
    public AbrirModal(content, row) {
      this.selectedEvaluacionInfoWithProgress = row;
      this.modalService.open(content).result.then(
        (closeResult) => {
          //Esto realiza la acción de cerrar la ventana
        }, (dismissReason) => {
          if (dismissReason == 'Finish') {
            //Si decide finalizarlo usaremos el metodo para finalizar la evaluación
            this.EvaluationDelete(row.id);
          }
        })
    }

    public GetAssignation(evaluationId: number) {
      this._assignationService.AssignationLastQuestionUpdated(evaluationId).subscribe(
        res => {
          this._appComponent._storageDataService.currentAssignation = res;
          this.sectionId = res.sectionId;
          this.GetAllSections(evaluationId, this._appComponent._storageDataService.AssessmentSelected.assessmentId);
        }
      );
    }

    public GetAllSections(evaluationId: number, assessmentId: number) {
      this._sectionService.getSectionInfo(evaluationId, assessmentId).subscribe(
        res => {
          this.ListaDeDatos = res;
          this._appComponent._storageDataService.Sections = this.ListaDeDatos;
          this._appComponent._storageDataService.SectionSelectedInfo = this.ListaDeDatos.filter(x => x.id == this.sectionId)[0];
  
          // //Se establece la siguiente sección validando si es la ultima
          // let index = this.ListaDeDatos.indexOf(this._appComponent._storageDataService.SectionSelectedInfo);
          // this._appComponent._storageDataService.nextSection = index != this.ListaDeDatos.length ? this.ListaDeDatos[index + 1] : null;
          // this._appComponent._storageDataService.prevSection = index != -1 ? this.ListaDeDatos[index - 1] : null;
  
          //this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);
          this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.Evaluacion.assessmentName, null);
          var pipe = new DatePipe('en-US');
          this._appComponent.pushBreadcrumb(pipe.transform(this._appComponent._storageDataService.Evaluacion.fecha, 'dd/MM/yyyy'), null);
          //this._appComponent.pushBreadcrumb("Secciones", "/evaluationsections");
          this._appComponent.pushBreadcrumb("BREADCRUMB.SECTIONS", "/evaluationsections");
  
          this._router.navigate(['/evaluationquestions']);
  
        }
      );
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
  
        this.ListaDeEquipos = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
        this.listaDeAssessment = this.originDataSource.filter(x => selected.oficina.includes(x.oficina)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
  
        selected.team = this.ListaDeEquipos;
        selected.assessment = this.listaDeAssessment
      }
  
      if(origen === 'equipo') {
         
        if(this.EquipoSeleccionado.length === 0) {
          // cuando no hay equipo seleccionado se queda a 0
          this.assessmentSeleccionado = []
        } else  {
          this.assessmentSeleccionado = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.assessmentName).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
          selected.assessment = [];
          selected.assessment = this.assessmentSeleccionado;
          if( this.assessmentSeleccionado.includes("SCRUM") && this.assessmentSeleccionado.includes("DEVOPS")) this.assessmentSeleccionado = [];
        }
  
        
       // this.OficinaSeleccionada = this.originDataSource.filter(x => selected.team.includes(x.nombre)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      }
      if(origen === 'assessment') {
        this.ListaDeOficinas = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.oficina).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
        this.ListaDeEquipos = this.originDataSource.filter(x => selected.assessment.includes(x.assessmentName)).map(x => x.nombre).reduce((x,y) => x.includes(y) ? x :  [...x, y],[]);
      }
      
       // viejo nuevo
       console.log(selected);
      let nuevo = this.originDataSource.filter(x =>selected.oficina.includes(x.oficina) && selected.team.includes(x.nombre) && selected.assessment.includes(x.assessmentName));
      this.dataSource.data = nuevo;
  
      if(this.OficinaSeleccionada.length === 0 && this.EquipoSeleccionado.length === 0 && this.assessmentSeleccionado.length === 0) {
        this.dataSource.data = this.originDataSource;
        this.ListaDeEquipos = this.originDataSource.reduce((x,y) => x.includes(y.nombre) ? x : [...x, y.nombre],[]);
        this.ListaDeOficinas = this.originDataSource.reduce((x,y) => x.includes(y.oficina) ? x : [...x, y.oficina],[]);
        this.listaDeAssessment = this.originDataSource.reduce((x,y) => x.includes(y.assessmentName) ? x : [...x, y.assessmentName],[])
        
      }
    }

  closingSelect(){
    this.mySelectTeam.close();
    this.mySelectOffice.close();
    this.mySelectAssessment.close();
  }
}
