import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import {animate, state, style, transition, trigger} from '@angular/animations';
import {PreviousevaluationComponent} from 'app/previousevaluation/previousevaluation.component'
import { AppComponent } from 'app/app.component';
import { RespuestaConNotas } from 'app/Models/RespuestaConNotas';
import { RespuestaConNotasTabla } from 'app/pdfgenerator/pdfgenerator.component';
import { RespuestasService } from 'app/services/RespuestasService';
import { Respuesta } from 'app/Models/Respuesta';
import { zip } from 'rxjs';

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
  selector: 'preguntas-table',
  templateUrl: './preguntas-table.component.html',
  styleUrls: ['./preguntas-table.component.scss'],
  animations: [
    trigger('detailExpand', [
    state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
    state('expanded', style({ height: '*' })),
    transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
  ],
})

export class PreguntasTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @Input() dataInput: any;//Array<EvaluacionInfo>;
  dataSource: MatTableDataSource<RespuestaConNotas>;
  userRole: string;
  expandedElement: RespuestaConNotas;
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['section', 'asignacion', 'pregunta', 'estado', 'notas'];

  ngOnInit() {
    let currentClass =  this;
    this.dataSource = new MatTableDataSource(this.dataInput);
    this.dataSource.sort= this.sort;
    this.dataSource.paginator = this.paginator;
    this.userRole = this._appComponent._storageDataService.Role;
    
    this.dataSource.filterPredicate = function(data, filter: string): boolean {

      var resp = currentClass.displyRespuestaCorrecta(data);
      var incomp = "incompleta";
      var filterResp = resp.toLowerCase().startsWith(filter) || incomp.includes(resp);

      return data.section.toLowerCase().includes(filter) 
      ||  data.asignacion.toLowerCase().includes(filter)
      ||  (data.pregunta.toLowerCase().includes(filter) && !data.pregunta.toLowerCase().includes("correcta") && !data.pregunta.toLowerCase().includes("incorrecta")&& !data.pregunta.toLowerCase().includes("incompleta") && !data.pregunta.toLowerCase().includes("no contestada"))
      ||  (data.notas != null && data.notas.toLowerCase().includes(filter) && !data.pregunta.toLowerCase().includes("correcta") && !data.pregunta.toLowerCase().includes("incorrecta")&& !data.pregunta.toLowerCase().includes("incompleta") && !data.pregunta.toLowerCase().includes("no contestada"))
      ||  (data.notasAdmin != null && data.notasAdmin.toLowerCase().includes(filter) && !data.pregunta.toLowerCase().includes("correcta") && !data.pregunta.toLowerCase().includes("incorrecta")&& !data.pregunta.toLowerCase().includes("incompleta") && !data.pregunta.toLowerCase().includes("no contestada"))
      ||  filterResp
      ||  currentClass.displayRespuesta(data).toLowerCase().includes(filter);
      ;
   };

   this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
    if (sortHeaderId == "estado") {
      if(data[sortHeaderId] == "0"){
        return "2";
      }
      else if((data[sortHeaderId] == "1" && data["correcta"]=="Si") || (data[sortHeaderId] == "2" && data["correcta"]=="No") || data["correcta"]== null){
        return "1";
      }
      else{
        return "3";
      }
    } 
    return data[sortHeaderId];
  };
  
  }

  applyFilter(filterValue: string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }


  public parseDate(value: string): string{
    let date = new Date(value);
    console.log(date.getDay()+"/"+date.getMonth()+"/"+date.getFullYear());
    return date.getDay()+"/"+date.getMonth()+1+"/"+date.getFullYear();
  }

  public checkRespuestaCorrecta(row): string {
    let classString: string;
    let respuestaString: string = this.displayRespuesta(row);

    //Si (habilitante)
    if (row.correcta == null) {
      //Contestado -> Si
      switch (row.estado) {
        case 0:
          classString = "respuesta-no-contestada";
          break
        case 1:
          classString = "respuesta-correcta";
          break
        case 2:
          classString = "respuesta-incorrecta";
          break
      }
    } else {
      if (respuestaString == row.correcta) {
        classString = "respuesta-correcta";
      } else {
        //No contestada
        if (row.estado == 0) {
          classString = "respuesta-no-contestada";
        } else {
          classString = "respuesta-incorrecta";
        }
      }
    }
    return classString;
  }

  //solo para el filtro
  public displyRespuestaCorrecta(row): string {
    let classString: string;
    let respuestaString: string = this.displayRespuesta(row);

    //Si (habilitante)
    if (row.correcta == null) {
      //Contestado -> Si
      switch (row.estado) {
        case 0:
          classString = "no contestada";
          break
        case 1:
          classString = "correcta";
          break
        case 2:
          classString = "incorrecta";
          break
      }
    } else {
      if (respuestaString == row.correcta) {
        classString = "correcta";
      } else {
        //No contestada
        if (row.estado == 0) {
          classString = "no contestada";
        } else {
          classString = "incorrecta";
        }
      }
    }
    return classString;
  }

  displayRespuesta(row: RespuestaConNotasTabla): string {
    let respuesta: string = "";
    switch (row.estado) {
      case 0:
        respuesta = "NC";
        break
      case 1:
        respuesta = "Si";
        break;
      case 2:
        respuesta = "No";
        break;

      default:
        break;
    }
    return respuesta;
  }
  
  constructor(
    private _appComponent: AppComponent,
    private _respuestasService: RespuestasService
    ){
    }

    saveNotas(model: RespuestaConNotas): void{
      let resp: Respuesta  = new Respuesta(model.id, model.estado, 0, 0, model.notas, model.notasAdmin,"");
      if(this.userRole == "Administrador" || this.userRole == "Evaluador"){
        this._respuestasService.AlterRespuesta(resp).subscribe(
          res => {
          },
          error => {
          });
      }
    }


}
