import { Component, OnInit, ViewChild, Input, ViewEncapsulation } from '@angular/core';
import { MatSort, MatTableDataSource, MatPaginator } from '@angular/material';
import { AppComponent } from 'app/app.component';
import { ProyectoService } from 'app/services/ProyectoService';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teams-manager',
  templateUrl: './teams-manager.component.html',
  styleUrls: ['./teams-manager.component.scss']
})
export class TeamsManagerComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  public ErrorMessage: string = null;
  dataSource: MatTableDataSource<any>;
  displayedColumns = ['officeName', 'unityName', 'linea', "teamName", "projectSize","acciones"];  
  encapsulation: ViewEncapsulation.None;
  selectedTeam;  
  public nTeams: number = 0;
  pa: any;  

  constructor(
    private _proyectoService: ProyectoService,
    private modalService: NgbModal,
    private router:Router,
  ) { }

  ngOnInit() {
    this.getTeams();
  }
  public getTeams(){
    this._proyectoService.GetAllNotTestProjects().subscribe(
      res => {
        this.dataSource = new MatTableDataSource(res);
        this.dataSource.paginator = this.paginator;
        if((res.length/this.paginator.pageSize) <= this.paginator.pageIndex ){
          this.paginator.pageIndex--; 
        }
        this.dataSource.sort= this.sort; 
                 
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

 public refresh(){
    this.getTeams();
  }
    
  public deleteTeam(team) {
    this._proyectoService.deleteTeam(team).subscribe(
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

  public modificarEquipo(row){
    var x = document.getElementById("addteam");
    if( x.style.display == "block"){
      x.style.display = "none";
    }
    this.router.navigate(['backoffice/addteam/'+row.id]);
  }
}
