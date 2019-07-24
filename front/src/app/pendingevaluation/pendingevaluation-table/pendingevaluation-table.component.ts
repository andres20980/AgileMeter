import { Component, OnInit, ViewChild, Input, SimpleChanges } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatCellDef } from '@angular/material';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { Router } from "@angular/router";
import { AppComponent } from 'app/app.component';
import { EvaluacionInfoWithProgress } from 'app/Models/EvaluacionInfoWithProgress';
import { EvaluacionService } from '../../services/EvaluacionService';
import { SectionService } from 'app/services/SectionService';
import { AssignationService } from 'app/services/AssignationService';
import { Evaluacion } from 'app/Models/Evaluacion';
import { NgbModal} from '@ng-bootstrap/ng-bootstrap';
import { PendingEvaluationComponent } from '../pendingevaluation.component';
import { SectionInfo } from 'app/Models/SectionInfo';
import { DatePipe } from '@angular/common';



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

export class PendingEvaluationTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @Input() ListaDeEvaluacionesPaginada: any;//Array<EvaluacionInfo>;
  public ErrorMessage: string = null;
  dataSource: MatTableDataSource<EvaluacionInfoWithProgress>;
  userRole: number;
  evaluationProgress : number;
  selectedEvaluacionInfoWithProgress;
  public ListaDeDatos: Array<SectionInfo> = [];
  public Evaluacion: Evaluacion = null;
  public sectionId: number;
  //expandedElement: Evaluacion;
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['fecha', 'userNombre', 'assessmentName', 'progress', 'actions'];

  constructor(
    private _evaluacionService: EvaluacionService,
    private _assignationService: AssignationService,
    private _sectionService: SectionService,
    private _router: Router,
    private _appComponent: AppComponent,
    private modalService: NgbModal,
    private parent: PendingEvaluationComponent   
    ){
    }

  ngOnInit() {
    this.LoadDataSource();
  }
  
  ngOnChanges(changes: SimpleChanges) {
    if (changes.ListaDeEvaluacionesPaginada){
      this.LoadDataSource();
    }
  }
  
  private LoadDataSource(){
    this.dataSource = new MatTableDataSource(this.ListaDeEvaluacionesPaginada);
    this.dataSource.sort= this.sort;
    this.dataSource.paginator = this.paginator;
    this.userRole = this._appComponent._storageDataService.Role;

      this.dataSource.filterPredicate = function(data, filter: string): boolean {
      let date = new Date(data.fecha);
      return data.nombre.toLowerCase().includes(filter) 
      ||  data.assessmentName.toLowerCase().includes(filter)
      ||  (data.userNombre != null && data.userNombre.toLowerCase().includes(filter) )
      ||  (data.progress != null && data.progress.toString().includes(filter))
      ||  ((date.getDate()<10?"0":"")+date.getDate()+"/"+(date.getMonth()<10?"0":"")+(date.getMonth()+1)+"/"+date.getFullYear()).includes(filter)
      ;
   };
  }

  applyFilter(filterValue: string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  public parseDate(value: string): string{
    let date = new Date(value);
    return date.getDay()+"/"+date.getMonth()+1+"/"+date.getFullYear();
  }

  //Metodo encargado de establecer la información de la evaluacion en StorageData
  public ContinueEvaluation(evaluation: EvaluacionInfoWithProgress){
        this._evaluacionService.getEvaluacion(evaluation.id).subscribe(
      res => {
        this._appComponent._storageDataService.Evaluacion = res;
        this._appComponent._storageDataService.Evaluacion.assessmentName = evaluation.assessmentName;
        this._appComponent._storageDataService.AssessmentSelected = {'assessmentId': evaluation.assessmentId, 'assessmentName': evaluation.assessmentName};
        this.GetAssignation(evaluation.id);
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la evaluación solicitada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      }
      );
  }
  //Metodo encargado de refrescar la tabla 
  public refresh(){
    this.parent.GetPaginacion();

  }


  //Metodo encargado de eliminar la evaluacion pasandole una evaluacionId
  public EvaluationDelete(evaluationId: number) {
    this._evaluacionService.EvaluationDelete(evaluationId).subscribe(
      res => {
        this.refresh();

      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo completar la actualización para esta evaluación.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      });
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
  // Metodo encargado de establecer la información necesaria sobre las secciones en el StorageData y redirigir a la siguiente vista
  public GetAllSections(evaluationId: number, assessmentId: number){
          this._sectionService.getSectionInfo(evaluationId, assessmentId).subscribe(
            res => {
              this.ListaDeDatos = res;
              this._appComponent._storageDataService.Sections = this.ListaDeDatos;
              this._appComponent._storageDataService.SectionSelectedInfo = this.ListaDeDatos.filter(x => x.id == this.sectionId)[0];

              //Se establece la siguiente sección validando si es la ultima
              let index = this.ListaDeDatos.indexOf(this._appComponent._storageDataService.SectionSelectedInfo);
              this._appComponent._storageDataService.nextSection = index != this.ListaDeDatos.length ? this.ListaDeDatos[index+1] : null;
              this._appComponent._storageDataService.prevSection = index != -1 ? this.ListaDeDatos[index-1] : null;

              //this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);
              this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.Evaluacion.assessmentName, null);
              var pipe = new DatePipe('en-US');
              this._appComponent.pushBreadcrumb(pipe.transform(this._appComponent._storageDataService.Evaluacion.fecha, 'dd/MM/yyyy'), null);
              //this._appComponent.pushBreadcrumb("Secciones", "/evaluationsections");
              this._appComponent.pushBreadcrumb("BREADCRUMB.SECTIONS", "/evaluationsections");              

              this._router.navigate(['/evaluationquestions']); 

            },
            error => {
              if (error == 404) {
                this.ErrorMessage = "Error: " + error + " No pudimos encontrar información de las secciones para esta evaluación.";
              } else if (error == 500) {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              } else if (error == 401) {
                this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
              } else {
                this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
              }
            }
          );
        }

  //Metodo encargado de establecer la información de la asignacion en StorageData
  public GetAssignation(evaluationId: number){
    this._assignationService.AssignationLastQuestionUpdated(evaluationId).subscribe(
      res => {
        this._appComponent._storageDataService.currentAssignation = res;
        this.sectionId = res.sectionId;
        this.GetAllSections(evaluationId, this._appComponent._storageDataService.AssessmentSelected.assessmentId);
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la asignación solicitada.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }  
      }
      );

  }
}
