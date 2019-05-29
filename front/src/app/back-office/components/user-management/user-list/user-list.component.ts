import { ProyectoService } from 'app/services/ProyectoService';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatSort, MatTableDataSource, MatPaginator } from '@angular/material';
import { UserService } from 'app/services/UserService';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

import { UserWithRole } from 'app/Models/UserWithRole';
import { UserString } from 'app/Models/UserString';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  public ErrorMessage: string = null;

  dataSource: MatTableDataSource<any>;
  displayedColumns = ['nombre', 'nombreCompleto', 'nombreRole', 'acciones'];
  encapsulation: ViewEncapsulation.None

  public userList: UserWithRole[];
  public userListString = new Array();
  public userString: UserString;
  selectedUser;
  selectedUsuarioInfoWithProgress;

  constructor(
    private _userService: UserService,
    private modalService: NgbModal,
    private _router: Router,
    private _proyectoService: ProyectoService,
  ) { }

  ngOnInit() {
    this.getUsers();
  }
  public btnEditClick(row) {
    this.selectedUser = this.userList.filter(u => u.nombre == row.nombre);
    this._userService.user = this.selectedUser[0];
    this._router.navigate(['backoffice/usermanagement/addUser']);
  };

  public btnAddClick() {
    this._userService.user = null;
    this._router.navigateByUrl('/backoffice/usermanagement/addUser');
  };

  //Método para el filtrado
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  //Metodo encargado de eliminar la evaluacion pasandole una evaluacionId
  public UsuarioDelete(usuario: string) {
    this.selectedUser = this.userList.filter(u => u.nombre == usuario);
    if (this.selectedUser.length > 0) {
      //Actualizamos el campo activo a false para hacer un borrado logico
      this.selectedUser[0].activo = false;
      this.selectedUser[0].password = null;
      this._userService.updateUser(this.selectedUser[0]).subscribe(
        res => {
          this.getUsers();
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " No se pudo completar la actualización para este usuario.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        });
    }
  }

  // Metodo encargado de abrir la ventana confirmando la eliminacion de la evaluacion
  public AbrirModal(content, row) {
    console.log(row);
    this.selectedUsuarioInfoWithProgress = row;
    this.modalService.open(content).result.then(
      (closeResult) => {
        //Esto realiza la acción de cerrar la ventana
      }, (dismissReason) => {
        if (dismissReason == 'Finish') {
          //Si decide finalizarlo usaremos el metodo para finalizar la evaluación
          this.UsuarioDelete(row.nombre);
          if (this._proyectoService.UsuarioLogeado == row.nombre) {
            this._router.navigate(['login']);
            localStorage.removeItem("user");
            localStorage.removeItem("userlongname");
          }
        }
      })
  }

  public getUsers() {
    this._userService.getUsers().subscribe(
      res => {
        //console.log(res);
        this.userList = [];
        this.userListString = [];

        this.userList = res;
        this.userListString = this.getUsersString(res);

        this.dataSource = new MatTableDataSource(this.userListString);
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

  public getUsersString(equipos: UserWithRole[]) {
    equipos.forEach(element => {
      this.userString = new UserString(
        element.nombre,
        element.nombreCompleto,
        element.role.role, //Nombre del role
        element.activo,
      );
      this.userListString.push(this.userString);
    });
    return this.userListString;
  }

}

