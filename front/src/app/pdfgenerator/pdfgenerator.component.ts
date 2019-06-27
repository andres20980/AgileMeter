import { Component, OnInit, ElementRef } from '@angular/core';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { Proyecto } from 'app/Models/Proyecto';
import { SectionInfo } from 'app/Models/SectionInfo';
import { RespuestaConNotas } from 'app/Models/RespuestaConNotas';
import { AsignacionConNotas } from 'app/Models/AsignacionConNotas';
import { AppComponent } from 'app/app.component';
import { Router } from '@angular/router';
import { SectionService } from 'app/services/SectionService';
import { EvaluacionService } from '../services/EvaluacionService';
import { DatePipe } from '@angular/common';
import { ProyectoService } from 'app/services/ProyectoService';
import { Http } from '@angular/http';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { map } from 'rxjs/operators';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { RespuestasService } from '../services/RespuestasService';
import { Respuesta } from '../Models/Respuesta';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ViewChild, Input } from '@angular/core';

import * as jsPDF from 'jspdf';
import * as html2canvas from 'html2canvas';
import * as imgTransform from 'img-transform';

import { forEach } from '@angular/router/src/utils/collection';
import { concat } from 'rxjs-compat/operator/concat';
import { NgCircleProgressModule, CircleProgressOptions } from 'ng-circle-progress';
import { PreviousevaluationComponent } from 'app/previousevaluation/previousevaluation.component';
import { Evaluacion } from 'app/Models/Evaluacion';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { SectionResultsComponent } from './section-results/section-results.component';


export interface RespuestaConNotasTabla {
  id: number,
  estado: number,
  pregunta: string,
  correcta: string,
  notas: string,
  notasAdmin: string,
  section: string,
  asignacion: string
}

export interface SectionsLevel{
  levelReached:number,
  percentOverLevel: number
}



@Component({
  selector: 'app-pdfgenerator',
  templateUrl: './pdfgenerator.component.html',
  styleUrls: ['./pdfgenerator.component.scss'],
  providers: [SectionService, ProyectoService, DatePipe, RespuestasService, CircleProgressOptions, EvaluacionService, PreviousevaluationComponent],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
  ]
})
export class PdfgeneratorComponent implements OnInit {
  // public ListaDeDatos: Array<SectionInfo> = [];
  public ListaSectionLevels: Array<SectionsLevel> = [];
  public ListaSectionConAsignaciones : any;
  public UserName: string = "";
  public Project: Proyecto = null;
  public Evaluacion: EvaluacionInfo;
  public Mostrar = false;
  public ErrorMessage = null;
  public AdminOn: boolean = false;
  public UserRole: string = ""; 

  //Datos de la barras
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;
  public AgileComplianceTotal: number = 100;
  public ListaSeccionesAgileCompliance: number[] = [];
  public ListaPuntuacion: number[] = [];
  public ListaNombres: string[] = [];


  //Para las notas
  public mostrarCheckboxes: boolean = true;
  public mostrarNotasEv: boolean = false;
  public mostrarNotasOb: boolean = false;
  public mostrarNotasSec: boolean = false;
  public mostrarNotasAsig: boolean = false;
  public mostrarNotasPreg: boolean = false;
  public ListaDeRespuestas: Array<RespuestaConNotas> = [];
  //public respuestasSource: MatTableDataSource<RespuestaConNotasTabla>;
  public ListaDeAsignaciones: Array<AsignacionConNotas> = []; 
  public cargandoNotas: boolean = false;
  public textoModal: string;
  public anadeNota: string = null;
  public MostrarPreguntas: boolean = true;
  public MostrarComentarios: boolean = false;

  //CircleProgress
  public selectedSection: any = null;

  //Excel
  public generatingExcel : boolean = false;

  //@ViewChild(SectionResultsComponent) sectionResult: SectionResultsComponent;

  scroll(el: HTMLElement) {
   
    setTimeout( () => {  el.scrollIntoView({behavior:'smooth'}); }, 200 );
}

  public formatSubtitle  = (sc: any) : string => {
    if(sc.nivelAlcanzado == -1){
      return "del nivel mínimo"
    }
    else{
      return "   Nivel  " + Math.trunc(sc.nivelAlcanzado);
    } 
  }

  public formatTotalSubtitle  = (sc: any) : string => {
    var minLevel = 99;
    sc.forEach(element => {
      if(element.nivelAlcanzado < minLevel)
      {
        minLevel = element.nivelAlcanzado
      }
    });
    //return "   Nivel  " + Math.trunc(minLevel);
    return "  del Nivel máximo"
  }

  public formatLevel  = (sc: any) : string => {
    return "lvl"+ (sc.nivelAlcanzado -1);
  }

  public formatTotalLevel  = (sc: any) : string => {
    var minLevel = 99;
    sc.forEach(element => {
      if(element.nivelAlcanzado < minLevel)
      {
        minLevel = element.nivelAlcanzado
      }
    });
    return "lvl"+ (minLevel -1);
  }

  //Metodo encargado de calcular el valor Total segun el peso de los diferentes Niveles.

  public getTotalPercent = (sc: any) : number => {
    var sumSections = 0;
    sc.forEach(element => {

      let sectionTotalValue;

      if (element.nivelAlcanzado == 1){
        sectionTotalValue = (element.pesoNivel1/100)* element.puntuacion;
        sectionTotalValue = (sectionTotalValue * element.peso)/100;

      }
      if (element.nivelAlcanzado == 2){
        sectionTotalValue = element.pesoNivel1 + ((element.pesoNivel2/100)* element.puntuacion);
        sectionTotalValue = (sectionTotalValue * element.peso)/100;

      }

      if (element.nivelAlcanzado == 3){
        sectionTotalValue = element.pesoNivel1 + element.pesoNivel2 + ((element.pesoNivel3/100)* element.puntuacion);
        sectionTotalValue = (sectionTotalValue * element.peso)/100;

      }
      sumSections += sectionTotalValue;

    });

    return sumSections;
  }
  public getTotalColor  = (sc: any) : string => {
    if(sc.nivelAlcanzado == 1){
      return "#000000";
    }
    else  if(sc.nivelAlcanzado == 2){
      return "#000000";
    }
    else  if(sc.nivelAlcanzado == 3){
      return "#000000";
    }
    return "#000000"
  }

  public getLevelColorOuter  = (sc: any) : string => {
    if(sc.nivelAlcanzado == 1){
      return "#c1de5d";
    }
    else  if(sc.nivelAlcanzado == 2){
      return "#37bf59";
    }
    else  if(sc.nivelAlcanzado == 3){
      return "#0fb3d4";
    }
    return "#000000"
  }

  
  public getLevelColorInner  = (sc: any) : string => {
    if(sc.nivelAlcanzado == 1){
      return "#c1de5d30";
    }
    else  if(sc.nivelAlcanzado == 2){
      return "#37bf5930";
    }
    else  if(sc.nivelAlcanzado == 3){
      return "#0fb3d430";
    }
    return "#000000"
  }

  public getPercent = (nota: number, compliance: number) : number => {
    return (nota/compliance - Math.trunc(nota/compliance)) *100;
  }


  //PDF
  public cargandoPDF: boolean = false;
  public total: number = -1;
  public totalCompletado: number = -2;
  public resultados = [];
  public continuar: boolean = true;
  public primeraVez: boolean = true;
  public doc = null;
  public iteracionResultados: number = 0;

  constructor(
    private _proyectoService: ProyectoService,
    private _appComponent: AppComponent,
    private _respuestasService: RespuestasService,
    private _router: Router,
    private _sectionService: SectionService,
    private prevEval: PreviousevaluationComponent,
    private http: Http,
    private datePipe: DatePipe,
    private modalService: NgbModal) {

    //Recupera los datos y los comprueba
    this.Project = this._appComponent._storageDataService.UserProjectSelected;
    this.Evaluacion = this._appComponent._storageDataService.EvaluacionToPDF;
    this._proyectoService.verificarUsuario();
    this.UserName = this._proyectoService.UsuarioLogeado;

    if (this.Evaluacion == null || this.Evaluacion == undefined || this.Project == null || this.Project == undefined) {

      this._router.navigate(['/home']);

    } else if (this.Evaluacion.id == null) {

      this._router.navigate(['/home']);

    }

    var ArrayRoles = [];
    this._proyectoService.getRolesUsuario().subscribe(
      res => {
        this.AdminOn = false;
        ArrayRoles = res;
        //this.UserRole = res[0]; //there is only a role for each user
        //Si no hay errores y son recogidos busca si tienes permisos de usuario
        for (let num = 0; num < ArrayRoles.length; num++) {
          if (ArrayRoles[num].role == "Administrador") {
            this.AdminOn = true;
          }
        }
        if (this.Project.id == null && !this.AdminOn) {
          this._router.navigate(['/home']);
        }
      },
      error => {
        this._router.navigate(['/home']);
      });


  }

  // private getSectionLevels() {
  //   this.http.get('assets/compliance_levels.json').pipe(map(res => res.json()))
  //     .subscribe((assessments) => {
  //       for (var a of assessments) {
  //         if (a.assesmentId == this.Evaluacion.assessmentId) {
  //           let i: number = 0;
  //           for (var s of a.sections) {
  //             let section: SectionInfo = this.ListaDeDatos[i];
  //             let levelReached: number = 0;
  //             let percentOverLevel: number = section.respuestasCorrectas;
  //             for (var l of s.levels) {
  //               if (percentOverLevel > l.value && s.levels.length - 1 > levelReached) {
  //                 percentOverLevel = percentOverLevel - l.value;
  //                 levelReached++;
  //               }
  //               else {
  //                 if(s.levels.length > levelReached && percentOverLevel < s.levels[levelReached].value){
  //                   percentOverLevel = percentOverLevel / s.levels[levelReached].value * 100;
  //                 }
  //                 else{
  //                   percentOverLevel = 100;
  //                 }
  //                 const sl: SectionsLevel = { levelReached, percentOverLevel };
  //                 this.ListaSectionLevels.push(sl);

  //                 break;
  //               }
  //             }
  //             i++;
  //           }
  //         }
  //       }
  //     });
  // }




  ngOnInit() {
    this.UserRole = this._appComponent._storageDataService.Role;
    //Recoge los datos de las secciones
    if (this.Evaluacion != null && this.Evaluacion != undefined) {
      
      //if (this._appComponent._storageDataService.AssessmentSelected === undefined) { this._appComponent._storageDataService.AssessmentSelected.assessmentId = 1 }
      // this._sectionService.getSectionInfo(this.Evaluacion.id,this.Evaluacion.assessmentId).subscribe( //this._appComponent._storageDataService.AssessmentSelected.assessmentId
      //   res => {
      //     // this.ListaDeDatos = res;
       
      //     //this.getSectionLevels();
      //     //this.cambiarMostrarNotasPreg();
      //     //this.shareDataToChart();

      //   },
      //   error => {
      //     if (error == 404) {
      //       this.ErrorMessage = "Error: " + error + "No pudimos recoger los datos de la sección lo sentimos.";
      //     } else if (error == 500) {
      //       this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
      //     } else if (error == 401) {
      //       this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
      //     } else {
      //       this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
      //     }
      //   }
      // );

      this._sectionService.GetPreguntasNivelOrganizadas(this.Evaluacion.id,this.Evaluacion.assessmentId).subscribe(
        res => {         
         this.ListaSectionConAsignaciones = res;      
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + "No pudimos recoger los datos de las preguntas.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        }
      );

    } else {
      this._router.navigate(['/home']);
    }

    //Para que no de error en modo development
    setTimeout(() => {
      this._appComponent.anadirUserProyecto(null,null, this.Evaluacion.nombre);
    });
  }

  // public GetCaptures(id: string): any{
  //   var elemento = document.getElementById(id);
  //   html2canvas(elemento).then(canvas => {
  //     //document.body.appendChild(canvas);
  //     return canvas.toDataURL("image/png");
  //             });
  // }

  public ExportToExcel(){
    this.generatingExcel = true;
    let workbook = new Workbook();

    var correct = workbook.addImage({
      base64: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAFoSURBVHja7NivTsQwHMDx7w5e4AQJ4kJIgIQ/AcsbnMEgwICAYDA8AAKHQOAwJOAgIRMoDJo7EKA4RUh4AEDgSYBifqK5rF23tewS+jNL2zT9rGv7a5YopRikaDBgEUERFEGhY7i/IrlfCTneEDAHvALvAGrxsrYZmgGegR7wBhzU+cnmgTtgQqvbBbbqAC0BN0Azo205dw15jjZwZXnxz7+coTZwbRlDAYdFQCPAguyMEJhN4MEVtC/bsgc8AbMBMGeuu2wd2NPKU8AtMO0BA3BqwphAqxl1TaCTgxp3wJwA20VTx4dlTXWASQOmWxVjAh0BPxZUVwD9mFZVjAn0KOvIdNke1VBeMbaDMZXnBZBktLfk9G34xOSd1HmoMUtf69YuC3JBmTBrWl/vF7RUBlChMUVymQuqMqZocrWhvGDKZPsU2AC+tLpvuWhVxpS9D50DL8COlI8l13mJJP5siKAI+m+g3wEASotSPg/rK5YAAAAASUVORK5CYII=",
      extension: 'png',
    });

    var incorrect = workbook.addImage({
      base64: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAH+SURBVHja7NfBS1RRFAbw5xhGBOJCAzH6mghaBK2MDyrDICRD/xHDcNEmKGgzY7toG7SITIkStHIrGOSydg1RYuEmhfhoEVTETJv74CHz7tx73gwEzeLw4M275/0G7j3vnKTRaCT/UiRdUBf034BEpHFIxGkR5zL3YuOwiFMizqf3LKCSS7Ai4p2IZyIqkZCSiHERz0W8FbEs4rYVdFzEaxENF7/ctRoBGhOxlcnx010nLaAzInYzybIRgjoh4qWIepP1ixYQMv8oFjUlYtuz9rEFdEDEC0/SPFRZxIZnzRcRV62nbEhELRDVJ+KKw9Q9z9+xbuo0hgNRoyLee57ZEzEvYqQoKBT1SsTvnN9+iJjL5iwKCkXlxX0RR9oNsqB2RFxzBTLpBChFbQWCKiJ6muVpJygRcTkQlJujXaAeERdEPAkE3ew06JKIz5EbutopEESsGU9ZtZ2gXhHTIj61eGktBlUEdFLEZotv01xERTeDekVcFLEu4o/nJbci61TFCiqLeONJ/E3EXRFHDcVzytqgfc1J+F3EbIvi6UMtWUDHPB/LeyIGWxRPH+qRdQ+t7uuHP4qYEXEwsKdOUfVMX/5BxIT1lA041AMRT0XcMIxAw25yeShiQcT1osd+QES/CO5vISJi0M1mZ10XWrhSlwoMiU1zdGf7LqhT8XcA/RsjSgptej8AAAAASUVORK5CYII=",
      extension: 'png',
    });

    var nc = workbook.addImage({
      base64: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAGiSURBVHja7Ni/S1VhHMfx1zHLIdCmgpAuIbq4FUTUErW0hFCY1D/g0hzhjSAKN4eWpv6BC5WLgw1pQwQFuYiD0WKYeAni2iZeuS7PXeRI+px75BnuZzuH8zy8+Zzvr+fJWq2WlNQjMXWB/qfeMjev1Or7X13CUvthbeJcOUCVWj3DVVzGDhbxI+fTJZzHRtkOncIbDKEPDaxgBnMBsq2N44ihExgMMHAG1/Ee7zB63EGdBag83cE8xlLKskHUcDultO/DawwXTvuc9I3VRUzjAZqpFMYx3EipUp/E49RaxxVcSAmoHzfLBDrqUJWV7dBuxJqzZQFleILTqcxDU3gWse5PGd3+KV5ExtyvTjtUjYSBf/gY7dD+ya5SqxeBge8HORTTy6p4WfBXv+pUUFfxvCDMJ3woOsL2hNQu6szfsM92UYeu4VFBmCYm8bUTdegnvkVW5PZgfx9vO1UYN4NDsxEwc2F0nT1sbBxW63iIu/gcjjoHqYEvGMc9LOd0+/w+FHP7EcrACG6Fg99A6Glb+I0FrB61vkUDdW8/ukApA+0NAOX3VWtrBPEFAAAAAElFTkSuQmCC",
      extension: 'png',
    });

    let worksheet = workbook.addWorksheet('Resultados',{ views: [{showGridLines: false }], properties: {tabColor:{argb:'75c222'}} });

    let titleRow = worksheet.addRow(['', 'Resultados de la evaluación del ' + this.datePipe.transform(this.Evaluacion.fecha, 'dd-MM-yyyy') + ' del equipo ' +  this.Project.nombre]);
    titleRow.font = { name: 'Arial', family: 4, size: 16, color: { argb: '555555' }, bold: true }
    worksheet.addRow([]);

    //// logo ////
    worksheet.getRow(1).height = 45; 
    var l = document.getElementById("logo");
    html2canvas(l, {logging:false}).then(canvas => {

      var logo = workbook.addImage({
        base64: canvas.toDataURL("image/png"),
        extension: 'png',
      });
     
      worksheet.addImage(logo, {
        tl: { col: (17.3), row: 0 },
        br: { col: (22), row: 1 }
    
    });
  });

    //// GRADIENT LINE ////

    worksheet.mergeCells(2, 1, 2, this.ListaSectionConAsignaciones.length * 3 + 7);
    var cellLine = worksheet.getCell(2, 1);
    cellLine.fill = {
      type: 'gradient',
      gradient: 'angle',
      degree: 0,
      stops: [
        {position:0, color:{argb:'87a900'}},
        {position:0.3333, color:{argb:'59971c'}},
        {position:0.6666, color:{argb:'2a8a41'}},
        {position:1, color:{argb:'317daf'}}
      ]
    };
    worksheet.getRow(2).height = 3;

    ////  ASSESSMENT NAME  ////
    worksheet.mergeCells(4, 2, 4, this.ListaSectionConAsignaciones.length * 3 + 4);
    var cellAss = worksheet.getCell(4, 2);
    cellAss.fill = {
      type: 'pattern',
      pattern: 'solid',
      fgColor: { argb: '5c981b' },
      bgColor: { argb: '5c981b' }
    };
    cellAss.font = {
      name: 'Arial',
      color: { argb: 'ffffff' },
      family: 2,
      size: 14,
      bold: true
    };
    cellAss.alignment = { vertical: 'top', horizontal: 'center', wrapText: true };
    cellAss.value = this.Evaluacion.assessmentName ;

    //// SECTIONS ////
      this.ListaSectionConAsignaciones.forEach((section, index) => {
      var elemento = document.getElementById(section.nombre);
      html2canvas(elemento, {logging:false}).then(canvas => {

        var imageId2 = workbook.addImage({
          base64: canvas.toDataURL("image/png"),
          extension: 'png',
        });
       
        worksheet.addImage(imageId2, {
          tl: { col: (index * 3 + 1), row: 4 },
          br: { col: (index * 3 + 4), row: 14 }
        });

        var h = section.nombre.length > 19 ? 16: 15;

        worksheet.mergeCells(15, index * 3 + 2, h, index * 3 + 4);

        var cell = worksheet.getCell(15, index * 3 + 2);

        cell.fill = {
              type: 'pattern',
              pattern: 'solid',
              fgColor: { argb: '03a8c0' },
              bgColor: { argb: '03a8c0' }
            }
        cell.font = {
          name: 'Arial',
          color: { argb: 'ffffff' },
          family: 2,
          size: 12,
          bold: true
        };
        cell.alignment = { vertical: 'top', horizontal: 'center', wrapText: true };
        cell.border = {
          top: {style:'thin', color: {argb:'dfdfdf'}},
          left: {style:'thin', color: {argb:'dfdfdf'}},
          bottom: {style:'thin', color: {argb:'dfdfdf'}},
          right: {style:'thin', color: {argb:'dfdfdf'}}
        };
        
        cell.value = section.nombre;

        //// SECTION TAB ////
        let worksheetSec = workbook.addWorksheet(section.nombre,{ views: [{showGridLines: false }], properties: {tabColor:{argb:'03bfda'}} });
        worksheetSec.getColumn(2).width = 4;
        worksheetSec.getColumn(19).width = 4;

        //// SECTION TITLE ////
        worksheetSec.getRow(1).height = 38;
        worksheetSec.mergeCells(1, 2, 1, 17); 
        var cellTitleSec = worksheetSec.getCell(1, 2);
        cellTitleSec.font = {
          name: 'Arial',
          color: { argb: '03a8c0' },
          family: 4,
          size: 20,
          bold: true
        };
        cellTitleSec.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true };
        cellTitleSec.value = section.nombre;

        /////// NOTAS SECCION /////
        worksheetSec.mergeCells(2, 2, 2, 17); 
      
        var cellNotasSec = worksheetSec.getCell(2, 2);
        cellNotasSec.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: '5c981b' },
          bgColor: { argb: '5c981b' }
        };
        cellNotasSec.font = cell.font;
        cellNotasSec.alignment = { vertical: 'top', horizontal: 'left', wrapText: true, indent:2 };
        cellNotasSec.border = {
          top: {style:'medium', color: {argb:'5c981b'}},
          left: {style:'medium', color: {argb:'5c981b'}},
          bottom: {style:'medium', color: {argb:'5c981b'}},
          right: {style:'medium', color: {argb:'5c981b'}}
        };
        cellNotasSec.value = "Notas";

        worksheetSec.mergeCells(3, 2, 8, 17); 

        var cellNotasSecC = worksheetSec.getCell(3, 2);
        cellNotasSecC.font = {
          name: 'Arial',
          color: { argb: '555555' },
          family: 2,
          size: 11,
          bold: false
        };
        cellNotasSecC.alignment = cellNotasSec.alignment;
        cellNotasSecC.border = cellNotasSec.border;
        cellNotasSecC.value = section.notas;


        //// MODULOS ////

        section.asignaciones.forEach((modulo, index) => {
          
          let offset = 12;
          if(index > 0){
            for(var i = 0; i < index; i++){
              offset += section.asignaciones[i].preguntas.length + 14;
            }
          }
          worksheetSec.mergeCells(offset, 2, offset, 19); 
          var cellMod = worksheetSec.getCell(offset, 2);
          cellMod.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: '03a8c0' },
            bgColor: { argb: '03a8c0' }
          };
          cellMod.font = cell.font;
          cellMod.alignment = { vertical: 'top', horizontal: 'left', wrapText: true, indent: 2 };
          cellMod.border = {
            top: {style:'medium', color: {argb:'03a8c0'}},
            left: {style:'medium', color: {argb:'03a8c0'}},
            bottom: {style:'medium', color: {argb:'03a8c0'}},
            right: {style:'medium', color: {argb:'03a8c0'}}
          };
          cellMod.value =  modulo.nombre;

          this.createOuterBorder(offset+1, 2, offset + modulo.preguntas.length + 11, 19, worksheetSec, '03a8c0');

          //// NOTAS MODULO ////
          worksheetSec.mergeCells(offset+2, 3, offset+2, 18); 
          var cellNotasMod = worksheetSec.getCell(offset+2, 3);
          cellNotasMod.fill = cellNotasSec.fill ;
          cellNotasMod.font = cellNotasSec.font;
          cellNotasMod.alignment = cellNotasSec.alignment;
          cellNotasMod.border = cellNotasSec.border;
          cellNotasMod.value = "Notas";

          worksheetSec.mergeCells(offset+3, 3, offset+8, 18);
          var cellNotasModC = worksheetSec.getCell(offset+3, 3);
          cellNotasModC.font = cellNotasSecC.font;
          cellNotasModC.alignment = cellNotasSec.alignment;
          cellNotasModC.border = cellNotasSec.border;
          cellNotasModC.value = modulo.notas;


          //// PREGUNTAS ////

          //// Pregunta ////
          worksheetSec.getRow(offset+10).height = 32;
          worksheetSec.mergeCells(offset+10, 3, offset+10, 11);
          var cellPreg = worksheetSec.getCell(offset+10, 3);
          cellPreg.font = {
            name: 'Arial',
            color: { argb: '555555' },
            family: 2,
            size: 11,
            bold: true
          };
          cellPreg.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true, indent: 2 };
          cellPreg.border = {
            bottom: {style:'thin', color: {argb:'5c981b'}}
          };
          cellPreg.value = "Pregunta";

          //// Respuesta ////
          worksheetSec.mergeCells(offset+10, 12, offset+10, 13);
          var cellRes = worksheetSec.getCell(offset+10, 12);
          cellRes.font = cellPreg.font;
          cellRes.alignment = cellPreg.alignment;
          cellRes.border = cellPreg.border;
          cellRes.value = "Respuesta";


          //// Notas ////
          worksheetSec.mergeCells(offset+10, 14, offset+10, 18);
          var cellNot = worksheetSec.getCell(offset+10, 14);
          cellNot.font = cellPreg.font;
          cellNot.alignment = cellPreg.alignment;
          cellNot.border = cellPreg.border;
          cellNot.value = "Notas";

          //// table content //
          modulo.preguntas.forEach((pregunta, index) => {
            //preg
            worksheetSec.getRow(offset+11 + index).height = 32;
            worksheetSec.mergeCells(offset+11 + index, 3, offset+11 + index, 11);
            var cellPregC = worksheetSec.getCell(offset+11 + index, 3);
            cellPregC.font = {
              name: 'Arial',
              color: { argb: '555555' },
              family: 2,
              size: 10,
              bold: false
            };
            cellPregC.alignment = cellPreg.alignment;
            cellPregC.border = {
              bottom: {style:'thin', color: {argb:'5c981b'}}
            };
            cellPregC.value = pregunta.pregunta;

            //resp
            worksheetSec.mergeCells(offset+11 + index, 12, offset+11 + index, 13);
            var cellResC = worksheetSec.getCell(offset+11 + index, 12);
            cellResC.font =cellPregC.font;
            cellResC.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true, indent: 3 };
            cellResC.border = cellPregC.border;
            cellResC.value = this.displayRespuesta(pregunta);

            let check = this.checkRespuestaCorrecta(pregunta);
            if(check == 'respuesta-correcta'){
              worksheetSec.addImage(correct, {
                tl: { col: 12, row: offset+10.4+index },
                ext: { width: 26, height: 26 },
                editAs: 'oneCell'
              });
            }
            else if(check == 'respuesta-incorrecta'){
              worksheetSec.addImage(incorrect, {
                tl: { col: 12, row: offset+10.4+index },
                ext: { width: 26, height: 26 },
                editAs: 'oneCell'
              });
            }
            else if(check == 'respuesta-no-contestada'){
              worksheetSec.addImage(nc, {
                tl: { col: 12, row: offset+10.4+index },
                ext: { width: 26, height: 26 },
                editAs: 'oneCell'
              });
            }
            

            //notas
            worksheetSec.mergeCells(offset+11 + index, 14, offset+11 + index, 18);
            var cellNotC = worksheetSec.getCell(offset+11 + index, 14);
            cellNotC.font =cellPregC.font;
            cellNotC.alignment = cellPregC.alignment;
            cellNotC.border = cellPregC.border;
            cellNotC.value = pregunta.notas;

          });

        });
        

        if(index == this.ListaSectionConAsignaciones.length - 1){
          var elementoT = document.getElementById("total");
          html2canvas(elementoT, {logging:false}).then(canvas => {
      
            var imageId = workbook.addImage({
              base64: canvas.toDataURL("image/png"),
              extension: 'png',
            });
      
            worksheet.addImage(imageId, {
              tl: { col: (this.ListaSectionConAsignaciones.length * 3 + 1), row: 4 },
              br: { col: (this.ListaSectionConAsignaciones.length * 3 + 4), row: 14 }
            });

            worksheet.mergeCells(15, this.ListaSectionConAsignaciones.length * 3 + 2, 15, this.ListaSectionConAsignaciones.length * 3 + 4); 
      
            var cell = worksheet.getCell(15, this.ListaSectionConAsignaciones.length * 3 + 2);

            cell.fill = {
                  type: 'pattern',
                  pattern: 'solid',
                  fgColor: { argb: '03a8c0' },
                  bgColor: { argb: '03a8c0' }
            };
            cell.font = {
              name: 'Arial',
              color: { argb: 'ffffff' },
              family: 2,
              size: 12,
              bold: true
            };
            cell.alignment = { vertical: 'top', horizontal: 'center', wrapText: true };
            cell.border = {
              top: {style:'thin', color: {argb:'dfdfdf'}},
              left: {style:'thin', color: {argb:'dfdfdf'}},
              bottom: {style:'thin', color: {argb:'dfdfdf'}},
              right: {style:'thin', color: {argb:'dfdfdf'}}
            };
            
            cell.value = "VALORACIÓN GLOBAL";


            /////// NOTAS EVALUACIÓN /////
            worksheet.mergeCells(18, 2, 18, 11); 
      
            var cellNotasEv = worksheet.getCell(18, 2);
            cellNotasEv.fill = {
              type: 'pattern',
              pattern: 'solid',
              fgColor: { argb: '5c981b' },
              bgColor: { argb: '5c981b' }
            };
            cellNotasEv.font = cell.font;
            cellNotasEv.alignment = { vertical: 'top', horizontal: 'left', wrapText: true, indent: 2 };
            cellNotasEv.border = {
              top: {style:'medium', color: {argb:'5c981b'}},
              left: {style:'medium', color: {argb:'5c981b'}},
              bottom: {style:'medium', color: {argb:'5c981b'}},
              right: {style:'medium', color: {argb:'5c981b'}}
            };
            cellNotasEv.value = "Notas Evaluación";

            worksheet.mergeCells(19, 2, 26, 11); 

            var cellNotasEvC = worksheet.getCell(19, 2);
            cellNotasEvC.font = {
              name: 'Arial',
              color: { argb: '555555' },
              family: 2,
              size: 11,
              bold: false
            };
            cellNotasEvC.alignment = cellNotasEv.alignment;
            cellNotasEvC.border = cellNotasEv.border;
            cellNotasEvC.value = this.Evaluacion.notasEvaluacion;


            /////// NOTAS OBJETIVOS /////
            worksheet.mergeCells(18, 13, 18, 22); 
      
            var cellNotasOb = worksheet.getCell(18, 13);
            cellNotasOb.fill = cellNotasEv.fill;
            cellNotasOb.font = cell.font;
            cellNotasOb.alignment = cellNotasEv.alignment;
            cellNotasOb.border = cellNotasEv.border;
            cellNotasOb.value = "Notas Objetivos";

            worksheet.mergeCells(19, 13, 26, 22); 

            var cellNotasObC = worksheet.getCell(19, 13);
            cellNotasObC.font = cellNotasEvC.font;
            cellNotasObC.alignment = cellNotasEv.alignment;
            cellNotasObC.border = cellNotasEv.border;
            cellNotasObC.value = this.Evaluacion.notasObjetivos;

            workbook.xlsx.writeBuffer().then((data) => {
              this.generatingExcel = false;
              let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
              fs.saveAs(blob, 'Resultados_del_equipo_'+  this.Project.nombre+'.xlsx');

            });
          });
        }
      });
        
    });

  }



  createOuterBorder = (startR, startC, endR, endC, worksheet, color) => {

    //const startColNumber = ExcelUtils.alphaToNum(start.column);
    //const endColNumber = ExcelUtils.alphaToNum(end.column);
    //const colRange = startC - endR + 1;
    const borderStyle = {
      style: 'medium',
      color: {argb: color}
    };
    const leftBorder = { left: borderStyle };
    const rightBorder = { right: borderStyle };
    const topBorder = { top: borderStyle };
    const bottomBorder = { bottom: borderStyle};
    const lefttop = { left: borderStyle, top: borderStyle};
    const righttop ={ right: borderStyle, top: borderStyle};
    const letfbottom ={ left: borderStyle, bottom: borderStyle};
    const rightbottom ={ right: borderStyle, bottom: borderStyle};


    for (let i = startR; i <= endR; i++) {
      const leftBorderCell = worksheet.getCell(i, startC);
      const rightBorderCell = worksheet.getCell(i, endC);
      leftBorderCell.border = leftBorder;
      rightBorderCell.border = rightBorder;
    }
  
    for (let i = startC; i <= endC; i++) {

      const topBorderCell = worksheet.getCell(startR, i);
      const bottomBorderCell = worksheet.getCell(endR, i);
      topBorderCell.border = topBorder;
      bottomBorderCell.border =  bottomBorder;
    }

    worksheet.getCell(startR, startC).border = lefttop;
    worksheet.getCell(startR, endC).border = righttop;
    worksheet.getCell(endR, startC).border = letfbottom;
    worksheet.getCell(endR, endC).border = rightbottom;
  };

  //Da los datos a las diferentes listas que usaremos para las graficas
  // public shareDataToChart() {
  //   for (var i = 0; i < this.ListaDeDatos.length; i++) {
  //     this.ListaNombres.push(this.ListaDeDatos[i].nombre);
  //     this.ListaPuntuacion.push(this.ListaDeDatos[i].respuestasCorrectas);
  //   }
  //   this.Mostrar = true;
  // }

  // //Opciones para la grafica
  // public barChartOptions: any = {
  //   scaleShowVerticalLines: true,
  //   scales: {
  //     yAxes: [{
  //       ticks: {
  //         steps: 10,
  //         stepValue: 10,
  //         max: 100,
  //         min: 0,
  //       }
  //     }]
  //   }
  // };

  // //Colores para la grafica
  // public chartColors: Array<any> = [
  //   { // first color
  //     backgroundColor: 'rgba(92, 183, 92, 0.5)',
  //     borderColor: 'rgba(92, 183, 92, 0.5)',
  //     pointBackgroundColor: 'rgba(92, 183, 92, 0.5)',
  //     pointBorderColor: '#fff',
  //     pointHoverBackgroundColor: '#fff',
  //     pointHoverBorderColor: 'rgba(92, 183, 92, 0.5)'
  //   },
  //   { // second color
  //     backgroundColor: 'rgba(52, 122, 183, 0.5)',
  //     borderColor: 'rgba(52, 122, 183, 0.5)',
  //     pointBackgroundColor: 'rgba(52, 122, 183, 0.5)',
  //     pointBorderColor: '#fff',
  //     pointHoverBackgroundColor: '#fff',
  //     pointHoverBorderColor: 'rgba(52, 122, 183, 0.5)'
  //   }];

  // //Estos son los datos introducidos en la grafica para que represente sus formas
  // public barChartData: any[] = [
  //   { data: this.ListaSeccionesAgileCompliance, label: 'Agile Compliance' },
  //   { data: this.ListaPuntuacion, label: 'Puntuación' }
  // ];

  // //Genera un pdf a partir de una captura de pantalla
  // //Esta funcion se encarga de coger todo lo que se quiera de pantalla y guardarlo en un vector
  // //Este proceso no es instantaneo, así que se va llamando a la funcion createPDF cada vez hasta que están todos completados
  // public downloadPDF() {
  //   this.anadeNota = null;

  //   this.cargandoPDF = true;

  //   var cajas = ["tablaPuntuaciones", "Grafica", "notasEvaluacion", "notasObjetivos", "notasSecciones", "notasAsignaciones", "notasPreguntas"];

  //   this.resultados = [];

  //   var referencia = this;

  //   this.total = 0;

  //   this.totalCompletado = 0;

  //   cajas.forEach((caja, index) => {

  //     var elemento = document.getElementById(caja);
  //     this.resultados.push(null);

  //     if (elemento != null) {
  //       setTimeout(function () {

  //         html2canvas(elemento).then(canvas => {

  //           referencia.resultados[index] = canvas;

  //           referencia.totalCompletado++;

  //           referencia.createPDF();

  //         });

  //       }, 500);


  //       this.total++;
  //     }

  //   });


  // }

  // //Funcion para coger los elementos que se han guardado en un vector y crear con ellos un pdf
  // public createPDF() {

  //   //Cuando se han cargado todos los elementos en el vector
  //   if (this.total === this.totalCompletado) {

  //     //Cuando se quiere crear un nuevo pdf
  //     if (this.primeraVez) {

  //       this.doc = new jsPDF('p', 'mm', 'A4');

  //       this.primeraVez = false;
  //     }

  //     //Suma de la altura
  //     var alturaTotal = 0;

  //     //Dimensiones de la pagina
  //     var tamanioPag = 200;

  //     var tamanioRestante = tamanioPag;

  //     //Usa dimensiones en mm, asi que tenemos que calcular cuanto mide
  //     var unMmEnPx = parseInt(window.getComputedStyle(document.getElementById("my_mm")).height.toString().split('px')[0]);


  //     //Por cada imagen
  //     //Esta funcion vuelve a ser llamada al dividir imágenes, por eso tendremos que llevar la cuenta de por donde nos quedamos
  //     for (var j = this.iteracionResultados; j < this.resultados.length && this.continuar; j++) {

  //       var imagen = this.resultados[j];

  //       this.iteracionResultados++;

  //       if (imagen != null) {

  //         var tamanioImagen = imagen.height / unMmEnPx;

  //         //Si la imagen es mas grande que la pagina se llama a otra funcion que la dividira
  //         if (tamanioImagen >= tamanioPag) {

  //           this.continuar = false;

  //           this.imagenGrande(imagen, 0, tamanioPag, tamanioImagen);


  //         } else {

  //           //Añadimos la imagen a la pagina existente o creamos una nueva página
  //           tamanioRestante -= tamanioImagen + 20;

  //           if (tamanioRestante < 0) {
  //             this.doc.addPage();
  //             alturaTotal = 0;
  //             tamanioRestante = tamanioPag;
  //           }

  //           this.doc.addImage(imagen.toDataURL("image/png", 1.0), 'PNG', 15, alturaTotal + 20, 0, 0, '', 'FAST');

  //           alturaTotal += tamanioImagen;
  //         }


  //       }

  //     }

  //     //Si se ha terminado de crear el pdf
  //     if (this.continuar) {
  //       var date = this.datePipe.transform(this.Evaluacion.fecha, 'dd-MM-yyyy');
  //       var nombre = this.Evaluacion.nombre;
  //       this.doc.save(nombre + '.' + date + '.' + 'AgileMeter.pdf');


  //       this.totalCompletado = -1;
  //       this.total = -2;

  //       this.cargandoPDF = false;
  //       this.continuar = true;
  //       this.primeraVez = true;
  //       this.iteracionResultados = 0;
  //     }

  //   }
  // }

  // //Cuando hay una imagen que ocupa mas de una pagina se llama a esta funcion, que divide el pdf y la imagen en varias páginas
  // //Es una funcion recursiva que se llama a si misma para ir cortando la imagen
  // //Esto se hace porque es la unica forma de conservar el orden al ser un proceso asíncrono
  // public imagenGrande(imagen, iteracion, tamanioPag, tamanioImg) {

  //   //Las dimensiones para añadir imagenes son muy raras
  //   //El tamaño de la imagen lo da mal
  //   //Todo lo que se ve ha sido sacado por prueba y error (hay que mejorarlo)
  //   var sumar = 1;

  //   if (iteracion > 0) {
  //     sumar = -40;
  //   }


  //   var relacion = tamanioImg / tamanioPag;
  //   var totalIteraciones = Math.floor(tamanioImg / tamanioPag);

  //   //Numero de iteraciones
  //   if (totalIteraciones == 1 && relacion >= 1.1) {
  //     totalIteraciones++;
  //   }

  //   //Cuanto sumar a la imagen por arriba para continuar
  //   var top = 950;

  //   if (iteracion == totalIteraciones - 1 && iteracion != 0) {

  //     top = Math.ceil((((tamanioImg % tamanioPag) * 950 / tamanioPag) + 30 * totalIteraciones) % 950);

  //   }

  //   //Transformamos la i
  //   imgTransform(imagen.toDataURL("image/png", 1.0)).crop(2000, top, 0, (950 * iteracion) + sumar).done(dataUrl => {

  //     this.doc.addPage();

  //     this.doc.addImage(dataUrl, 'PNG', 15, 20, 0, 0, '', 'FAST');

  //     //Si se ha terminado, se llama a la funcion anterior para continuar con la creacion del pdf
  //     if (iteracion == totalIteraciones - 1) {

  //       this.continuar = true;
  //       this.createPDF();

  //     } else {

  //       this.imagenGrande(imagen, iteracion + 1, tamanioPag, tamanioImg);
  //     }

  //   });

  // }

  saveNotas(model: Evaluacion): void{
    if(this.UserRole == "Administrador" || this.UserRole == "Evaluador"){
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

  //Para no mostrar la pantalla de cargando
  public apagarCargar() {
    this.cargandoPDF = false;
  }

  //Para mostrar o no las notas de evaluacion
  public cambiarMostrarNotasEv() {
    if (this.Evaluacion.notasEvaluacion != null && this.Evaluacion.notasEvaluacion != "") {
      this.mostrarNotasEv = !this.mostrarNotasEv;
    }
  }

  //Para mostrar o no las notas de objetivos
  public cambiarMostrarNotasOb() {
    if (this.Evaluacion.notasObjetivos != null && this.Evaluacion.notasObjetivos != "") {
      this.mostrarNotasOb = !this.mostrarNotasOb;
    }
  }

  //Para mostrar o no las notas de seccion
  public cambiarMostrarNotasSec() {
    if (this.Evaluacion.flagNotasSec) {
      this.mostrarNotasSec = !this.mostrarNotasSec;
    }
  }

  //Para mostrar o no las notas de preguntas
  // public cambiarMostrarNotasPreg() {

  //   //No se ha hecho la peticion al servidor aun
  //   if (!this.mostrarNotasPreg && this.ListaDeRespuestas.length == 0) {
  //     this.cargandoNotas = true;

  //     this._sectionService.getRespuestasConNotas(this.Evaluacion.id,this._appComponent._storageDataService.EvaluacionToPDF.assessmentId).subscribe(
  //       res => {
  //         this.ListaDeRespuestas = res;
  //         this.cambiarMostrarNotasAsig();
          
  //         //this.cargandoNotas = false;
  //         //this.mostrarNotasPreg = true;
  //       },
  //       error => {
  //         if (error == 404) {
  //           this.ErrorMessage = "Error: " + error + "No pudimos recoger los datos de las preguntas.";
  //         } else if (error == 500) {
  //           this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //         } else if (error == 401) {
  //           this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
  //         } else {
  //           this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //         }
  //       }
  //     );


  //   }
  //   else {
  //     this.mostrarNotasPreg = !this.mostrarNotasPreg;
  //   }
  // }

  checkRespuestaCorrecta(row): string {
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


  //   return "material-icons " + classString;
  // }

  displayRespuesta(row: RespuestaConNotasTabla): string {
    let respuesta: string = "";
    switch (row.estado) {
      case 0:
        respuesta = "NC";
        break
      case 1:
        respuesta = "Sí";
        break;
      case 2:
        respuesta = "No";
        break;

      default:
        break;
    }
    return respuesta;
  }


  //Para mostrar o no las notas de asignacion
  // public cambiarMostrarNotasAsig() {

  //     //No se ha hecho la peticion al servidor aun
  //     if (!this.mostrarNotasAsig && this.ListaDeAsignaciones.length == 0) {
  //       this.cargandoNotas = true;

  //       this._sectionService.getAsignConNotas(this.Evaluacion.id).subscribe(
  //         res => {
  //           this.ListaDeAsignaciones = res;
            
  //           this.ListaDeDatos.forEach(element => {
  //             if(element.notas != null && element.notas.trim() != ""){
  //               this.ListaDeAsignaciones.unshift(new AsignacionConNotas(element.nombre, "-", element.notas));
  //             }
              
  //           });
  //         },
  //         error => {
  //           if (error == 404) {
  //             this.ErrorMessage = "Error: " + error + "No pudimos recoger los datos de las preguntas.";
  //           } else if (error == 500) {
  //             this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //           } else if (error == 401) {
  //             this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
  //           } else {
  //             this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //           }
  //         }
  //       );

  //     }
  //     else {
  //       this.mostrarNotasAsig = !this.mostrarNotasAsig;
  //     }
    
  // }

  // //Para volver a la pantalla de evaluaciones
  // public Volver(lugar) {
  //   this._router.navigate([lugar]);
  // }

  // //Para abrir el modal de notas de admin
  // public AbrirModal(content, i) {

  //   this.anadeNota = null;

  //   if (this.ListaDeRespuestas[i].notasAdmin != null) {
  //     this.textoModal = this.ListaDeRespuestas[i].notasAdmin;
  //   } else {
  //     this.textoModal = "";
  //   }

  //   this.modalService.open(content).result.then(
  //     (closeResult) => {
  //       //Si cierra, no se guarda

  //     }, (dismissReason) => {
  //       if (dismissReason == 'Guardar') {

  //         if (this.textoModal != "") {
  //           this.ListaDeRespuestas[i].notasAdmin = this.textoModal;
  //         } else {
  //           this.ListaDeRespuestas[i].notasAdmin = null;
  //         }

  //         var resp = new Respuesta(this.ListaDeRespuestas[i].id, this.ListaDeRespuestas[i].estado,
  //           1, 1, this.ListaDeRespuestas[i].notas, this.ListaDeRespuestas[i].notasAdmin,this.UserName);


  //         this._respuestasService.AlterRespuesta(resp).subscribe(
  //           res => {

  //             this.anadeNota = "Nota añadida correctamente";
  //           },
  //           error => {

  //             if (error == 404) {
  //               this.ErrorMessage = "Error: " + error + "No pudimos realizar la actualización de la respuesta, lo sentimos.";
  //             } else if (error == 500) {
  //               this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //             } else if (error == 401) {
  //               this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
  //             } else {
  //               this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //             }
  //           });

  //       }
  //       //Else, Click fuera, no se guarda
  //     })
  // }

}
