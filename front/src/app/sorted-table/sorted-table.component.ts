import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import {animate, state, style, transition, trigger} from '@angular/animations';
import {PreviousevaluationComponent} from 'app/previousevaluation/previousevaluation.component'
import { AppComponent } from 'app/app.component';
import { Evaluacion } from 'app/Models/Evaluacion';

// export interface Evaluacion {
//   id: number,
//   fecha: string;
//   nombre: string,
//   userNombre: string;
//   puntuacion: number;
//   estado: boolean;
//   notasEv: string;
//   notasOb: string;
//   assessmentName: string;
// }

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
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @Input() dataInput: any;//Array<EvaluacionInfo>;
  dataSource: MatTableDataSource<Evaluacion>;
  userRole: string;
  expandedElement: Evaluacion;
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['fecha', 'userNombre', 'assessmentName', 'puntuacion', 'notas', 'informe'];

  ngOnInit() {
    this.dataSource = new MatTableDataSource(this.dataInput);
    this.dataSource.sort= this.sort;
    this.dataSource.paginator = this.paginator;
    this.userRole = this._appComponent._storageDataService.Role;
    this.prevEval.TableFilteredData = this.dataSource.filteredData;
    
    this.dataSource.filterPredicate = function(data, filter: string): boolean {
      let date = new Date(data.fecha);
      //console.log ((date.getDate()<10?"0":"")+date.getDate()+"/"+(date.getMonth()<10?"0":"")+(date.getMonth()+1)+"/"+date.getFullYear());
      return data.nombre.toLowerCase().includes(filter) 
      ||  data.assessmentName.toLowerCase().includes(filter)
      ||  data.userNombre.toLowerCase().includes(filter)
      ||  data.puntuacion.toString().concat("%").includes(filter)
      ||  (data.notasEvaluacion != null && data.notasEvaluacion.toLowerCase().includes(filter))
      ||  (data.notasObjetivos != null && data.notasObjetivos.toLowerCase().includes(filter))
      ||  ((date.getDate()<10?"0":"")+date.getDate()+"/"+(date.getMonth()<10?"0":"")+(date.getMonth()+1)+"/"+date.getFullYear()).includes(filter)
      ;
   };
  }

  applyFilter(filterValue: string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
    this.prevEval.TableFilteredData = this.dataSource.filteredData;
  }


  public parseDate(value: string): string{
    let date = new Date(value);
    console.log(date.getDay()+"/"+date.getMonth()+"/"+date.getFullYear());
    return date.getDay()+"/"+date.getMonth()+1+"/"+date.getFullYear();
  }
  
  constructor(
    public prevEval: PreviousevaluationComponent,
    private _appComponent: AppComponent
    ){
    }

  SaveDataToPDF(evaluacion: EvaluacionInfo): void {
    this.prevEval.SaveDataToPDF(evaluacion) ;
  }

  saveNotas(model: Evaluacion): void{
    if(this.userRole == "Administrador" || this.userRole == "Evaluador"){
      this.prevEval._evaluacionService.updateEvaluacion(model).subscribe(
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

}
