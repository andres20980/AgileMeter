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
  displayedColumns = ['oficina', 'unidad', 'linea', "nombre", "projectSize", "acciones"];
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
        this.MensajeNotificacion = "Equipo eliminado correctamente";
        this._eventService.displayMessage(this.MensajeNotificacion,false);
        setTimeout(() => { this.MensajeNotificacion = null }, 2000);
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
        this.MensajeNotificacion = "Ups , tuvimos problemas técnicos para eliminar el equipo, disculpe las molestias";
        this._eventService.displayMessage(this.MensajeNotificacion,true);
        setTimeout(() => { this.MensajeNotificacion = null }, 2000);
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
}
