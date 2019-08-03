import { Component, OnInit, ViewChild, Input, ViewEncapsulation } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { AppComponent } from 'app/app.component';
import { RespuestaConNotas } from 'app/Models/RespuestaConNotas';
import { RespuestaConNotasTabla } from 'app/pdfgenerator/pdfgenerator.component';
import { RespuestasService } from 'app/services/RespuestasService';
import { Respuesta } from 'app/Models/Respuesta';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-section-results',
  templateUrl: './section-results.component.html',
  styleUrls: ['./section-results.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SectionResultsComponent implements OnInit {

  @ViewChild(MatSort) sort: MatSort;
  @Input() lSectionConAsignacionesDto: any; //IEnumerable<SectionConAsignacionesDto>
  userRole: number = this._appComponent._storageDataService.Role;
  dataSource: MatTableDataSource<any>;
  expandedElement: RespuestaConNotas;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['pregunta', 'estado', 'notas'];
  
  constructor(
    private _appComponent: AppComponent,
    private _respuestasService: RespuestasService,
    private _translateService: TranslateService,
    ){
    }
    saveNotas(model: RespuestaConNotas): void{
      let answer: Respuesta  = new Respuesta(model.id, model.estado, 0, 0, model.notas, model.notasAdmin,"");
      if(this.userRole == 2 || this.userRole == 3){//2 es admin, 3 es evaluador
        this._respuestasService.AlterRespuesta(answer).subscribe();
      }
    }

  ngOnInit() {
  }
  
  //Metodo para dar formato a la fecha introducida
  public parseDate(value: string): string{
    let date = new Date(value);
    console.log(date.getDay()+"/"+date.getMonth()+"/"+date.getFullYear());
    return date.getDay()+"/"+date.getMonth()+1+"/"+date.getFullYear();
  }

  public checkRespuestaCorrecta(row): string {
    let classString: string;
    let respuestaString: string = this.displayRespuesta(row);
    if (respuestaString == "Sí"){
      respuestaString = "Si"}

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
  
  displayRespuesta(row: RespuestaConNotasTabla): string {
    let respuesta: string = "";
    switch (row.estado) {
      case 0:
        respuesta = "NC";
        break
      case 1:        
        this._translateService.get('SECTION_RESULTS.YES').subscribe(value => { respuesta = value; });
        break;
      case 2:        
        this._translateService.get('SECTION_RESULTS.NO').subscribe(value => { respuesta = value; });
        break;

      default:
        break;
    }
    return respuesta;
  }

  //Metodo encargado de gestionar las notas de las secciones y modulos
  DisplayNotes(noteText: string): string{
    noteText = noteText || null;   
    var returnedText = "";
    this._translateService.get('SECTION_RESULTS.NO_NOTES').subscribe(value => { returnedText = value; });
    if (noteText != null){
      returnedText = noteText;
    }
    
    return returnedText;
  }

  //Metodo encargado de gestionar las notas de las preguntas
  DisplayQuestionNote(noteText: string): string{
    noteText = noteText || null;

    var returnedText = "";
    if (noteText != null){
      returnedText = noteText;
    }
    
    return returnedText;
  }
}
