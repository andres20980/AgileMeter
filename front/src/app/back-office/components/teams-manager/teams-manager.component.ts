import { Component, OnInit, ViewChild, Input, ViewEncapsulation } from '@angular/core';
import { MatSort, MatTableDataSource, MatPaginator, Sort } from '@angular/material';
import { AppComponent } from 'app/app.component';
import { ProyectoService } from 'app/services/ProyectoService';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { Equipo } from 'app/Models/Equipo';
import { DataSource } from '@angular/cdk/table';
import { Team } from 'app/Models/Team';
import { EventEmitterService } from 'app/services/event-emitter.service';
import { TranslateService } from '@ngx-translate/core';

//Excel
import * as fs from 'file-saver';
import { Workbook } from 'exceljs';

@Component({
  selector: 'app-teams-manager',
  templateUrl: './teams-manager.component.html',
  styleUrls: ['./teams-manager.component.scss']
})
export class TeamsManagerComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  public ErrorMessage: string = null;
  public MensajeNotificacion: string = null;
  dataSource: MatTableDataSource<any>;
  displayedColumns = ['oficina', 'unidad', 'codigo', 'linea', "nombre", "projectSize", "acciones"];
  encapsulation: ViewEncapsulation.None;
  selectedTeam;
  public nTeams: number = 0;
  pa: any;
  public teamList: Equipo[];
  public teamsListString = new Array();
  public teamString: Team;


  constructor(
    private _proyectoService: ProyectoService,
    private modalService: NgbModal,
    private router: Router,
    private _eventService: EventEmitterService,
    private _translateService: TranslateService
  ) { }

  ngOnInit() {
    this.getTeams();
  }
  public getTeams() {
    this.teamsListString = [];
    this._proyectoService.GetAllNotTestProjects().subscribe(
      res => {
        //eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos 
        //this.teamList = res;        
        //this.teamsListString = this.getTeamsString(res);        
        //this.dataSource = new MatTableDataSource(this.teamsListString);

        //console.log(res);

        this.dataSource = new MatTableDataSource(res);
        this.dataSource.paginator = this.paginator;
        if ((res.length / this.paginator.pageSize) <= this.paginator.pageIndex) {
          this.paginator.pageIndex--;
        }
        this.dataSource.sort = this.sort;
      },
      error => {
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + " No se pudo encontrar la información solicitada.";
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

  // Metodo encargado de abrir la ventana confirmando la eliminacion del equipo
  public AbrirModal(content, row) {
    //var equipo = this.teamList.filter(t => t.id == row.id);
    //this.selectedTeam = equipo[0];
    this.selectedTeam = row;
    this.modalService.open(content).result.then(
      (closeResult) => {
        //Esto realiza la acción de cerrar la ventana
      }, (dismissReason) => {
        if (dismissReason == 'Finish') {
          //Si decide finalizarlo usaremos el metodo para finalizar la evaluación
          this.deleteTeam(row);
        }
      })
  }

  public refresh() {
    this.getTeams();
  }
  public getTeamsString(equipos: Equipo[]) {
    equipos.forEach(element => {
      this.teamString = new Team(
        element.id, element.nombre,
        element.oficinaEntity.oficinaNombre,
        element.unidadEntity.unidadNombre,
        element.lineaEntity.lineaNombre,
        element.projectSize
      );
      this.teamsListString.push(this.teamString);
    });
    return this.teamsListString;
  }

  public deleteTeam(team) {
    this._proyectoService.deleteTeam(team).subscribe(
      res => {
        this.refresh();
        //this.MensajeNotificacion = "Equipo eliminado correctamente";
        this._translateService.get('TEAM_MANAGER.NOTIFICATION_DELETE_TEAM').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, false);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
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
        //this.MensajeNotificacion = "Ups , tuvimos problemas técnicos para eliminar el equipo, disculpe las molestias";
        this._translateService.get('TEAM_MANAGER.NOTIFICATION_ERROR_DELETE_TEAM').subscribe(value => { this.MensajeNotificacion = value; });
        this._eventService.displayMessage(this.MensajeNotificacion, true);
        setTimeout(() => { this.MensajeNotificacion = null }, 4000);
      });
  }

  public modificarEquipo(row) {
    //this.selectedTeam = this.teamList.filter(team => team.id == row.id);    
    //this._proyectoService.modificarEquipo(this.selectedTeam[0]);
    //this._proyectoService.modificarEquipo(row);
    this._proyectoService.equipo = row;
    this.router.navigate(['backoffice/teamsmanager/addteam']);

  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  btnAddClick() {
    this._proyectoService.equipo = null;
    this.router.navigate(['backoffice/teamsmanager/addteam']);
  }

  public ExportToExcel(){
    var woorksheet = "",title="",oficina="",unidad="",proyecto="",nombre="", projectSize="";
    this._translateService.get('TEAM_MANAGER.EXCEL_WORKSHEET').subscribe(value => { woorksheet = value; });
    this._translateService.get('TEAM_MANAGER.EXCEL_TITLE').subscribe(value => { title = value; });
    this._translateService.get('TEAM_MANAGER.TABLE_COL_OFICCE').subscribe(value => { oficina = value; });
    this._translateService.get('TEAM_MANAGER.TABLE_COL_UNIT').subscribe(value => { unidad = value; });
    this._translateService.get('TEAM_MANAGER.TABLE_COL_PROJECT').subscribe(value => { proyecto = value; });
    this._translateService.get('TEAM_MANAGER.TABLE_COL_TEAM').subscribe(value => { nombre = value; });
    this._translateService.get('TEAM_MANAGER.TABLE_COL_SIZE').subscribe(value => { projectSize = value; });
    let workbook = new Workbook();
    
    let worksheet = workbook.addWorksheet(woorksheet);

    let titleRow = worksheet.addRow([title]);
    titleRow.font = { name: 'Arial', family: 4, size: 16, bold: true }
    worksheet.addRow([]);

    let header = [oficina, unidad , proyecto , nombre, projectSize ]
    //Add Header Row
    let headerRow = worksheet.addRow(header);
    
    // Cell Style : Fill and Border
    headerRow.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFEEEEEE' },
        bgColor: { argb: '110000' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
    })


    this.dataSource.filteredData.forEach(d => {
      worksheet.addRow([d.oficina, d.unidad, d.proyecto, d.nombre, d.projectSize]);
      }
    );

    worksheet.getColumn(1).width = 12;
    worksheet.getColumn(2).width = 12;
    worksheet.getColumn(3).width = 12;
    worksheet.getColumn(4).width = 12;
    var nombre:string;
    this._translateService.get('TEAM_MANAGER.EXCEL_DOCUMENT_NAME').subscribe(value => { nombre = value; });
    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, nombre+ '.xlsx');
    })
  }
}
