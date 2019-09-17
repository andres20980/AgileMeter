import { Component, OnInit, ViewChild } from '@angular/core';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { EvaluacionFilterInfo } from 'app/Models/EvaluacionFilterInfo';
import { Proyecto } from 'app/Models/Proyecto';
import { AppComponent } from 'app/app.component';
import { Router } from '@angular/router';
import { EvaluacionService } from '../services/EvaluacionService';
import { Evaluacion } from 'app/Models/Evaluacion';
import { Observable } from 'rxjs/Rx';
import { Http } from '@angular/http';
import { map } from 'rxjs/operators';
import { interval ,  Subscription } from 'rxjs';
import { ProyectoService } from 'app/services/ProyectoService';
import { Role } from 'app/Models/Role';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { ChartsModule, BaseChartDirective } from 'ng2-charts/ng2-charts';
import { DatePipe } from '@angular/common';
import { SectionInfo } from 'app/Models/SectionInfo';
import { SectionService } from 'app/services/SectionService';
import { EvaluacionInfoWithSections } from 'app/Models/EvaluacionInfoWithSections';
import { forEach } from '@angular/router/src/utils/collection';
import { SectionsLevel } from 'app/pdfgenerator/pdfgenerator.component';
import { Assessment } from 'app/Models/Assessment';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { MatTable, MatTableDataSource } from '@angular/material';
import { SortedTableComponent } from 'app/sorted-table/sorted-table.component';
import { TranslateService } from '@ngx-translate/core';

// import { setTimeout } from 'timers';

export interface ComplianceLevels {

  assesmentId: number,
  name: string,
  sections:{
    sectionId: number, name: string, 
    levels:  {level: number, value: number}[]
  }[]

}

export interface AssesmentEv{
  id: number,
  name: string
}

@Component({
  selector: 'app-previousevaluation',
  templateUrl: './previousevaluation.component.html',
  styleUrls: ['./previousevaluation.component.scss'],
  providers: [EvaluacionService, ProyectoService, SectionService]
})
export class PreviousevaluationComponent implements OnInit {
  public clicked: boolean = true;
  public EvaluacionFiltrar: EvaluacionFilterInfo = { 'nombre': '', 'estado': 'true', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': 0 , 'oficinas':[], equipos:[], 'idAssessment': [] };
  public Typing: boolean = false;
  public permisosDeUsuario: Array<Role> = [];
  public ListaDeEvaluacionesPaginada: Array<EvaluacionInfo>;
  public EvaluationsWithSectionInfo: Array<EvaluacionInfoWithSections>;
  public nEvaluaciones: number = 0;
  public UserName: string = "";
  public Project: Proyecto = { 'id': null, 'nombre': '', codigo: null, 'fecha': null, numFinishedEvals:0, numPendingEvals: 0, oficina:null, proyecto: ''};
  public Mostrar = false;
  public PageNow = 1;
  public NumMax = 0;
  public ErrorMessage: string = null;
  public NumEspera = 750;
  public MostrarInfo = false;
  public Timeout: Subscription;
  public textoModal: string;
  public anadeNota = null;
  public fechaPicker: NgbDate;
  public MostrarTabla: boolean = true;
  public MostrarGrafica: boolean = false;
  public TableFilteredData: Evaluacion[];
  //@ViewChild(SortedTableComponent) table: SortedTableComponent;

  public Admin: boolean = false;
  public ListaDeProyectos: Array<Proyecto> = [];
  public ProyectoSeleccionado: boolean = false;


  //Datos de la barras
  public barChartType: string = 'line';
  public barChartLegend: boolean = false;
  public AgileComplianceTotal: number = 100;
  public ListaSeccionesAgileCompliance: number[] = [];
  public ListaPuntuacion: { type: string, label: string, backgroundColor: string, borderColor: string, data: Array<any>, fill: string, lineTension: number, pointRadius: number, pointHoverRadius: number, borderWidth: number }[] = [];
  public ListaNombres: string[] = [];
  public ComplianceLevels: ComplianceLevels;
  public MaxLevelReached: number;
  public barChartOptions: any;
  public ListaAssessments : AssesmentEv[] = [];
  public selectedAssessment: AssesmentEv;
  public legend: any;
  public allLegendsHidden: boolean = false;

  //Para actualizar la grafica
  @ViewChild(BaseChartDirective) public chart: BaseChartDirective;


  constructor(
    private _appComponent: AppComponent,
    private _router: Router,
    public _evaluacionService: EvaluacionService,
    private _proyectoService: ProyectoService,
    private _sectionService: SectionService,
    private modalService: NgbModal,
    private http: Http,
    private _translateService: TranslateService
  ) { }

  ngOnInit() {
    //Recogemos los proyectos y realizamos comprobaciones
    var Role;
    //this.Project = this._appComponent._storageDataService.UserProjectSelected;
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    this.UserName = this._proyectoService.UsuarioLogeado;
    //this.getUserRole();
    //Recogemos el rol del usuario
    this._proyectoService.getRolesUsuario().subscribe(
      res => {

        this.permisosDeUsuario = res;
        //console.log(res);
        //Si no hay errores y son recogidos busca si tienes permisos de usuario
        for (let num = 0; num < this.permisosDeUsuario.length; num++) {
          if (this.permisosDeUsuario[num].role == "Administrador") {
            if (this.Project == null || this.Project == undefined || this.Project.id == -1) {
              this.Project = { id: 0, nombre: '',codigo:null, fecha: null, numFinishedEvals:0, numPendingEvals: 0, oficina:null, proyecto: ''};
              this.Admin = true;
            }
          }
        }
        /*obsoleto en el listar evaluaciones
        //Comprueba que tenga  un proyecto seleccionado y si no es asi lo devuelve a home
        if (this.Project == null || this.Project == undefined) {
          this._router.navigate(['/home']);
        } else if (this.Project.id == -1 && !this.Admin) {
          this._router.navigate(['/home']);
        } else {
          this.MostrarInfo = true;
        }*/
        
        this.GetPaginacion(); //TODO
        //this.changeChartAssessment();

      },
      error => {
        //Si el servidor tiene algún tipo de problema mostraremos este error
        if (error == 404) {
          this.ErrorMessage = "Error: " + error + "No pudimos recoger los datos del usuario.";
        } else if (error == 500) {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        } else if (error == 401) {
          this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
        } else {
          this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
        }
      },
      () => {
        //this.Restablecer();
      });

      //this._appComponent.pushBreadcrumb("Evaluaciones finalizadas", "/finishedevaluations");
      this._appComponent.pushBreadcrumb("BREADCRUMB.FINISHED_EVALUATIONS", "/finishedevaluations");
      this._appComponent.pushBreadcrumb(this._appComponent._storageDataService.UserProjectSelected.nombre, null);

    if (this.Project.fecha != null) {
      //Para que no de error en modo development
      setTimeout(() => {
        this._appComponent.anadirUserProyecto(this.UserName, this._proyectoService.UserLongName, this.Project.nombre);
      },0);
    } else {
      //Para que no de error en modo development
      setTimeout(() => {
        this._appComponent.anadirUserProyecto(this.UserName, this._proyectoService.UserLongName, "Todos");
      },0);

    }
  }

  

  //Este metodo devuelve el número de paginas máximo que hay
  public CalcularPaginas() {
    var NumeroDePaginas = Math.floor((this.nEvaluaciones / 10) * 100) / 100;
    if (NumeroDePaginas % 1 != 0) {
      this.NumMax = Math.floor(NumeroDePaginas) + 1;
    } else {
      this.NumMax = NumeroDePaginas;
    }
  }

  //Método para activar la gráfica
  public ActivarGrafica()
  {
    var copiaLista = Array<EvaluacionInfo>();

    if (this.ListaDeEvaluacionesPaginada != undefined)
    {
      this.ListaDeEvaluacionesPaginada.forEach(element => {
        if (!copiaLista.find(c => c.proyectoId == element.proyectoId))
        {
          copiaLista.push(element);
        }
      });
    }
    return (copiaLista.length > 1? false:true);
  }

  //Restablece los datos de la busqueda
  // public Restablecer() {
  //   if (this.clicked) {
  //     this.EvaluacionFiltrar.fecha = "";
  //     this.EvaluacionFiltrar.nombre = "";
  //     this.EvaluacionFiltrar.userNombre = "";
  //     this.EvaluacionFiltrar.estado = "";
  //     this.EvaluacionFiltrar.puntuacion = "";
  //     this.ProyectoSeleccionado = false;

  //     this.GetPaginacion();
  //     this.clicked = false;
  //   }
  //   else {
  //     this.clicked = true;

  //     if (this.ListaDeProyectos.length == 0) {

  //       //Segun el tipo de rol que tengas te permitira tener todos los proyectos o solo los tuyos
  //       //El servicio se encargara de enviar una respuesta con el listado de proyecto
  //       //El usuario necesario ya tendria que haber sido cargado en el logueo
  //       if (!this.Admin) {
  //         //Aqui se entra solo si no tienes permisos de administrador dandote los proyectos que te tocan
  //         this._proyectoService.getProyectosDeUsuario().subscribe(
  //           res => {
  //             this.ListaDeProyectos = res;
  //           },
  //           error => {
  //             //Si el servidor tiene algún tipo de problema mostraremos este error
  //             if (error == 404) {
  //               this.ErrorMessage = "Error: " + error + " El usuario o proyecto autenticado no existe.";
  //             } else if (error == 500) {
  //               this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //             } else if (error == 401) {
  //               this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
  //             } else {
  //               this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //             }
  //           });
  //       } else {
  //         //Aqui entra si eres administrador dandote todos los proyectos
  //         this._proyectoService.getAllProyectos().subscribe(
  //           res => {
  //             this.ListaDeProyectos = res;

  //           },
  //           error => {
  //             //Si el servidor tiene algún tipo de problema mostraremos este error
  //             if (error == 404) {
  //               this.ErrorMessage = "Error: " + error + " El usuario o proyecto autenticado no existe.";
  //             } else if (error == 500) {
  //               this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //             } else if (error == 401) {
  //               this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
  //             } else {
  //               this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //             }
  //           });
  //       }

  //     }

  //   }
  // }

  //Para el desplegable de elegir proyecto
  // public SeleccionDeProyecto(index: number) {

  //   //Ningun proyecto elegido
  //   if (isNaN(index)) {
  //     this.ProyectoSeleccionado = false;
  //     this.EvaluacionFiltrar.nombre = "";
  //   }
  //   //Elegido algun proyecto
  //   else {
  //     this.ProyectoSeleccionado = true;
  //     this.EvaluacionFiltrar.nombre = this.ListaDeProyectos[index].nombre;
  //   }

  //   this.TryHttpRequest(false);
  // }

  //Este metodo devuelve la transforma la lista de evaluaciones dada en una lista paginada
  // public paginacionLista(pageNumber: number) {
  //   var Skip = pageNumber * 10;
  //   var ListaPaginada = new Array<EvaluacionInfo>();
  //   var contador = Skip;
  //   while (ListaPaginada.length != 10 && contador < this.ListaDeEvaluacionesPaginada.length) {
  //     ListaPaginada.push(this.ListaDeEvaluacionesPaginada[contador]);
  //     contador++;
  //   }
  //   this.ListaDeEvaluacionesPaginada = ListaPaginada;
  // }

 
  //Guarda los datos en el storage y cambia de ruta hacia la generación de grafica
  public SaveDataToPDF(evaluacion: EvaluacionInfo) {
    this._appComponent._storageDataService.EvaluacionToPDF = evaluacion;  
    
    this._appComponent.pushBreadcrumb(evaluacion.assessmentName, null);
    var pipe = new DatePipe('en-US');
    this._appComponent.pushBreadcrumb(pipe.transform(evaluacion.fecha, 'dd/MM/yyyy'), null);
    //this._appComponent.pushBreadcrumb("Resultados", "/evaluationresults");
    this._appComponent.pushBreadcrumb("BREADCRUMB.RESULTS", "/evaluationresults");
    this._router.navigate(['/evaluationresults']);
  }

  //Filtra por evaluaciones completas completas o ninguna
  // public ChangeFiltro(estado: string) {
  //   this.PageNow = 1;
  //   this.EvaluacionFiltrar.estado = estado;
  //   this.GetPaginacion();
  // }

  //Al presionar el boton va avanzado y retrocediendo
  // public NextPreviousButton(Option: boolean) {
  //   if (Option && this.PageNow < this.NumMax) {
  //     this.paginacionLista(this.PageNow++);
  //     this.GetPaginacion();
  //   } else if (!Option && this.PageNow > 1) {
  //     this.PageNow--;
  //     var CualToca = this.PageNow - 1;
  //     this.paginacionLista(CualToca);
  //     this.GetPaginacion();
  //   }
  // }

  /*Obsoleto con el listar
  //Este metodo es llamado cuando cambias un valor de filtrado y en 500 milisegundos te manda a la primera pagina y recarga el componente con
  //los nuevos elementos
  public TryHttpRequest(timeout: boolean) {
    if (this.Timeout != null && !this.Timeout != undefined) {
      this.Timeout.unsubscribe();
    }

    //Cuando se escribe nombre
    if (timeout) {
      this.Timeout = interval(500)
        .subscribe(i => {
          this.PageNow = 1, this.GetPaginacion(),
            this.Timeout.unsubscribe()
        });
    }

    //Cuando se cambia de proyecto
    else {

      this.PageNow = 1;
      this.GetPaginacion();
    }

  }
*/
  //Recarga los elementos en la pagina en la que se encuentra 
  public GetPaginacion() {
    this.Mostrar = false;
    // this._evaluacionService.getEvaluacionInfoFiltered(this.PageNow - 1, this.Project.id, this.EvaluacionFiltrar)
    //console.log(this.EvaluacionFiltrar);
    this._evaluacionService.getAllEvaluacionInfoFiltered(this.PageNow - 1, this.EvaluacionFiltrar)
      .subscribe(
        res => {
          this.nEvaluaciones = res.numEvals;
          this.ListaDeEvaluacionesPaginada = res.evaluacionesResult;

          this.Mostrar = true; 

          if(this.ListaDeEvaluacionesPaginada.length > 0){        
            this.ListaDeEvaluacionesPaginada.forEach(ev => {
              if(this.ListaAssessments.find(a => a.id == ev.assessmentId) == null){
                let id: number = ev.assessmentId;
                let name: string = ev.assessmentName;
                let a: AssesmentEv= { id, name};
                this.ListaAssessments.push(a);
              }
            });

            //Temporalmente asignamos el primer proyecto de la tabla sin filtrar que cumple que su assessment es scrum
            this.Project.id = this.ListaDeEvaluacionesPaginada.find(ev => ev.assessmentId == 1).proyectoId;

            this.selectedAssessment = this.ListaAssessments[0];
          }

          if(this.selectedAssessment != null && this.selectedAssessment != undefined){
          //Filtro de la grafica para traer las evaluaciones
           let filter: EvaluacionFilterInfo = new EvaluacionFilterInfo("","","","","true", this.selectedAssessment.id,[],[],[]);
          this.GetChartData(filter);
          }         
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " No pudimos recoger la información de las evaluaciones, lo sentimos.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        });

  }

  public GetChartData(filter: EvaluacionFilterInfo){
    this._evaluacionService.GetEvaluationsWithSectionsInfo(this.Project.id, filter)
      .subscribe(
        res => {
          this.Mostrar = true; 
          this.EvaluationsWithSectionInfo = res.evaluacionesResult;
          if (this.EvaluationsWithSectionInfo !== undefined && this.EvaluationsWithSectionInfo.length > 0)
          {
            this.shareDataToChart();
          }         
        },
        error => {
          if (error == 404) {
            this.ErrorMessage = "Error: " + error + " No pudimos recoger la información de las evaluaciones, lo sentimos.";
          } else if (error == 500) {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          } else if (error == 401) {
            this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
          } else {
            this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
          }
        });
  }

  public ExportToExcel(){
    var woorksheet = "",title="",date="",user="",assessment="",score="";
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_WORKSHEET').subscribe(value => { woorksheet = value; });
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_TITLE').subscribe(value => { title = value; });
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_DATE').subscribe(value => { date = value; });
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_USER').subscribe(value => { user = value; });
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_ASSESSMENT').subscribe(value => { assessment = value; });
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_SCORE').subscribe(value => { score = value; });
    let workbook = new Workbook();
    
    let worksheet = workbook.addWorksheet(woorksheet);

    let titleRow = worksheet.addRow([title +" "+ this.Project.nombre]);
    titleRow.font = { name: 'Arial', family: 4, size: 16, bold: true }
    worksheet.addRow([]);

    let header = [date,user , assessment , score ]
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


    this.TableFilteredData.forEach(d => {
      worksheet.addRow([new Date(d.fecha), d.userNombre, d.assessmentName, d.puntuacion+'%']);
      }
    );

    worksheet.getColumn(1).width = 12;
    worksheet.getColumn(2).width = 12;
    worksheet.getColumn(3).width = 12;
    worksheet.getColumn(4).width = 12;
    var nombre:string;
    this._translateService.get('PREVIOUS_EVALUATION.EXCEL_DOCUMENT_NAME').subscribe(value => { nombre = value; });
    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, nombre+  this.Project.nombre+'.xlsx');
    })
  }

  //Modal de notas evaluacion y objetivos
  //tipo=0 -> Evaluacion
  //tipo=1 -> Objetivos
  // public AbrirModal(content, numeroEv, tipo) {

  //   this.anadeNota = null;

  //   if (tipo == 0) {
  //     if (this.ListaDeEvaluacionesPaginada[numeroEv].notasEvaluacion != null) {
  //       this.textoModal = this.ListaDeEvaluacionesPaginada[numeroEv].notasEvaluacion;
  //     } else {
  //       this.textoModal = "";
  //     }
  //   } else if (tipo == 1) {
  //     if (this.ListaDeEvaluacionesPaginada[numeroEv].notasObjetivos != null) {
  //       this.textoModal = this.ListaDeEvaluacionesPaginada[numeroEv].notasObjetivos;
  //     } else {
  //       this.textoModal = "";
  //     }
  //   }

  //   this.modalService.open(content).result.then(
  //     (closeResult) => {
  //       //Si cierra, no se guarda

  //     }, (dismissReason) => {
  //       if (dismissReason == 'Guardar') {

  //         this.Mostrar = false;

  //         if (tipo == 0) {
  //           if (this.textoModal != "") {
  //             this.ListaDeEvaluacionesPaginada[numeroEv].notasEvaluacion = this.textoModal;
  //           } else {
  //             this.ListaDeEvaluacionesPaginada[numeroEv].notasEvaluacion = null;
  //           }
  //         } else {
  //           if (this.textoModal != "") {
  //             this.ListaDeEvaluacionesPaginada[numeroEv].notasObjetivos = this.textoModal;
  //           } else {
  //             this.ListaDeEvaluacionesPaginada[numeroEv].notasObjetivos = null;
  //           }
  //         }

  //         var evalu = new Evaluacion(
  //           this.ListaDeEvaluacionesPaginada[numeroEv].id,
  //           this.ListaDeEvaluacionesPaginada[numeroEv].fecha,
  //           this.ListaDeEvaluacionesPaginada[numeroEv].estado,
  //           this.ListaDeEvaluacionesPaginada[numeroEv].notasOb,
  //           this.ListaDeEvaluacionesPaginada[numeroEv].notasEv,
  //           this.ListaDeEvaluacionesPaginada[numeroEv].puntuacion,
  //         );

  //         this._evaluacionService.updateEvaluacion(evalu)
  //           .subscribe(
  //             res => {
  //               this.anadeNota = "Se guardó la nota correctamente";
  //             },
  //             error => {
  //               if (error == 404) {
  //                 this.ErrorMessage = "Error: " + error + " No pudimos recoger la información de las evaluaciones, lo sentimos.";
  //               } else if (error == 500) {
  //                 this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //               } else if (error == 401) {
  //                 this.ErrorMessage = "Error: " + error + " El usuario es incorrecto o no tiene permisos, intente introducir su usuario nuevamente.";
  //               } else {
  //                 this.ErrorMessage = "Error: " + error + " Ocurrio un error en el servidor, contacte con el servicio técnico.";
  //               }
  //             },
  //             () => {
  //               this.Mostrar = true;
  //             });


  //       }
  //       //Else, Click fuera, no se guarda
  //     })
  // }

  public VolverInicio() {
    this._router.navigate(['/home']);
  }

  //Para cambiar la fecha y que aparezca en el formato correcto en la caja tenemos que liar to esto
  // public changeDate() {
  //   if (this.fechaPicker.day < 10) {
  //     this.EvaluacionFiltrar.fecha = "0" + this.fechaPicker.day + "/";
  //   } else {
  //     this.EvaluacionFiltrar.fecha = this.fechaPicker.day + "/";
  //   }

  //   if (this.fechaPicker.month < 10) {
  //     this.EvaluacionFiltrar.fecha = this.EvaluacionFiltrar.fecha + "0" + this.fechaPicker.month + "/" + this.fechaPicker.year;
  //   } else {
  //     this.EvaluacionFiltrar.fecha = this.EvaluacionFiltrar.fecha + this.fechaPicker.month + "/" + this.fechaPicker.year;
  //   }

  //   this.PageNow = 1;
  //   this.GetPaginacion();

  // }

  //Limpiamos la caja de fecha
  // public limpiar() {
  //   this.EvaluacionFiltrar.fecha = "";

  //   this.PageNow = 1;
  //   this.GetPaginacion();
  // }

  public changeChartAssessment(){
    let filter: EvaluacionFilterInfo = new EvaluacionFilterInfo("","","","","true", this.selectedAssessment.id,[],[],[]);
    this.GetChartData(filter);
  }


  //Da los datos a las diferentes listas que usaremos para las graficas
  public shareDataToChart() {
    this.ListaPuntuacion = [];
    this.ListaNombres = [];
    this.MaxLevelReached = 0;
    let listaSections : number[][] = [];
    let listaGlobal : number[] = [];
    let listaSectionLevels: SectionsLevel[][] = [];
    let index:number = 0;
    let colorList: string[] = ["#E74C3C", "#3498DB","#F1C40F" ,"#9B59B6", "#ef56b4", "#33CCCC", "#F39C12",  "#34495E"]
    let levelColorList : string[] = ["#c1de5d40", "#37bf5940", "#0fb3d440",  "#00c06340", "#00c09e40", "#00b5c040"]


    for(var i = 0; i <  this.EvaluationsWithSectionInfo.length; i++) { //this.EvaluationsWithSectionInfo.length + 1
      if(i <  this.EvaluationsWithSectionInfo.length){
        let sl: Array<SectionsLevel> = new Array<SectionsLevel>();
        for(var j = 0; j < this.EvaluationsWithSectionInfo[i].sectionsInfo.length; j++) {

            sl.push({levelReached: this.EvaluationsWithSectionInfo[i].sectionsInfo[j].nivelAlcanzado,
            percentOverLevel: this.EvaluationsWithSectionInfo[i].sectionsInfo[j].puntuacion});
           
        }
        listaSectionLevels[i] = sl;
      }
      else{
        listaSectionLevels[i] = [];
      }
    }

    for(var i = 0; i <  this.EvaluationsWithSectionInfo[0].sectionsInfo.length; i++) { //this.EvaluationsWithSectionInfo[0].sectionsInfo.length + 1
      listaSections[i] = [];
      for(var j = 0; j< this.EvaluationsWithSectionInfo.length; j++) {
        listaSections[i][j] = 0;
      }
    }



    //Cogemos los datos a añadir
    for (var i = this.EvaluationsWithSectionInfo.length - 1; i >= 0; i--) {
      listaGlobal[i] = this.EvaluationsWithSectionInfo[i].puntuacion;
      for(var j: number = 0; j < listaSections.length; j++) {
        // if(j >= this.EvaluationsWithSectionInfo[0].sectionsInfo.length){
        //   listaSections[j][index] = this.EvaluationsWithSectionInfo[i].puntuacion;
        // }
        // else{         
          listaSections[j][index] = listaSectionLevels[i][j].percentOverLevel + (listaSectionLevels[i][j].levelReached -1) * 100;//this.EvaluationsWithSectionInfo[i].sectionsInfo[j].respuestasCorrectas;
          if(listaSectionLevels[i][j].levelReached > this.MaxLevelReached){
            this.MaxLevelReached = listaSectionLevels[i][j].levelReached -1;
          }
        //}

      }
      index++;
      var pipe = new DatePipe('en-US');
      this.ListaNombres.push(pipe.transform(this.EvaluationsWithSectionInfo[i].fecha, 'dd/MM/yyyy'));
    }

    this.ListaNombres.unshift("");
    this.ListaNombres.push("");
    listaGlobal.unshift(NaN);
    listaGlobal.push(NaN);
    for(var j: number = 0; j < listaSections.length; j++) {
      listaSections[j].unshift(NaN);
      listaSections[j].push(NaN);
    }


    for(var j: number = 0; j <  this.EvaluationsWithSectionInfo[0].sectionsInfo.length; j++) { //this.EvaluationsWithSectionInfo[0].sectionsInfo.length + 1

      // if(j >= this.EvaluationsWithSectionInfo[0].sectionsInfo.length){
      //   this.ListaPuntuacion.push({
      //     data: listaSections[j], label: "Global", backgroundColor: "#2ECC71", fill: 'false', lineTension : 0.1,
      //     borderColor: "#2ECC71", pointRadius: 2, pointHoverRadius: 4, borderWidth: 3});
      // }
      // else{
        this.ListaPuntuacion.push({ type: "line",
          data: listaSections[j], label: this.EvaluationsWithSectionInfo[0].sectionsInfo[j].nombre, backgroundColor: colorList[j], fill: 'false', lineTension : 0.1,
          borderColor: colorList[j], pointRadius: 2, pointHoverRadius: 4, borderWidth: 3});
      //}
    }
    
    

    for(var i: number = 0; i <= this.MaxLevelReached; i++) {
      let level: number[] = [];
      for(var j: number = 0; j < listaSections[0].length +1; j++) {
        level[j] = i * 100 + 100;
      }
      if(i == 0){
        this.ListaPuntuacion.push({ type: "line",
          data: level, label: 'aux' + i, backgroundColor: levelColorList[i], fill: 'origin', lineTension : 0.1,
          borderColor: levelColorList[i], pointRadius: 0, pointHoverRadius: 0, borderWidth: 0.1});
      }
      else{
        this.ListaPuntuacion.push({ type: "line",
          data: level, label: 'aux' + i, backgroundColor: levelColorList[i], fill: '-1', lineTension : 0.1,
          borderColor: levelColorList[i], pointRadius: 0, pointHoverRadius: 0, borderWidth: 0.1});
      }
    }
    //console.log(this.ListaPuntuacion);
    this.setBarChartOptions();

    //Para actualizar la grafica una vez esté visible
    setTimeout(() => {

      if (this.chart && this.chart.chart && this.chart.chart.config) {
        this.chart.chart.config.type = "bar";
        this.chart.chart.config.data.labels = this.ListaNombres;
        this.chart.chart.config.data.datasets = this.ListaPuntuacion;
        this.chart.chart.config.data.datasets.push({type: 'bar', yAxisID: "y-axis-1", data: listaGlobal.reverse(), label: "Global", backgroundColor: "#2ECC71AA", borderColor: "#2ECC71", hoverBackgroundColor: "#2ECC71", borderWidth:"2", hidden:"true"});
        this.chart.chart.config.data.options = this.barChartOptions;
        this.chart.chart.update();

        this.legend = this.chart.chart.generateLegend();
        //console.log(this.chart.chart.generateLegend());
      }
    }, 300);

  }

  public toggleChartData(i: number){
    this.chart.chart.config.data.datasets[i].hidden = !this.chart.chart.config.data.datasets[i].hidden;

    this.allLegendsHidden = true;
    for(var j: number = 0; j < this.legend.length; j++) {
      this.allLegendsHidden = this.allLegendsHidden &&  (this.chart.chart.config.data.datasets[j].hidden);
    }
    this.chart.chart.options.scales.yAxes[0].gridLines.display  = !this.allLegendsHidden;
    this.chart.chart.options.scales.yAxes[1].gridLines.display = this.allLegendsHidden;

    for(var j: number = 0; j < this.chart.chart.config.data.datasets.length; j++) {
      if(this.chart.chart.config.data.datasets[j].label.includes('aux')){
        this.chart.chart.config.data.datasets[j].hidden = this.allLegendsHidden;
      }
    }

    this.chart.chart.update();
  }

  public toggleGlobalData(){
    this.chart.chart.config.data.datasets[this.chart.chart.config.data.datasets.length - 1].hidden = !this.chart.chart.config.data.datasets[this.chart.chart.config.data.datasets.length - 1].hidden;
    this.chart.chart.update();
  }

  //Opciones para la grafica
  public setBarChartOptions(){
    var level ;
    this._translateService.get('PREVIOUS_EVALUATION.LEVEL').subscribe(value => { level = value; });
    var ofLevel;
    this._translateService.get('PREVIOUS_EVALUATION.OF_LEVEL').subscribe(value => { ofLevel = value; });
    var self = this;
    this.barChartOptions = {
    scaleShowVerticalLines: false,
    showLines: true,
    legend: {
      labels: {
          filter: function(item, chart) {
              // Logic to remove a particular legend item goes here
              return !item.text.includes('aux');
          },
          usePointStyle: true
      },
     
  },
  legendCallback: function(chart) {
    let legend : any[] = [];
    for (var i=0; i<chart.data.datasets.length; i++) {  
      if(!chart.data.datasets[i].label.includes('aux') && !chart.data.datasets[i].label.includes('Global')) {  
          legend.push({label: chart.data.datasets[i].label, color: chart.data.datasets[i].backgroundColor});
      }
    }
    return legend;

},
    tooltips: {
      cornerRadius: 2,
      filter: function (tooltipItem, data) {
        var label = data.datasets[tooltipItem.datasetIndex].label;
        //console.log(data.datasets[tooltipItem.datasetIndex]);
        if (label.includes('aux') && !label.includes('Global')) {
          return false;
        } else {
          return true;
        }
    },
    animation: false,
      callbacks: {
        label: function (tooltipItem, data) {
          // const datasetLabel = data.datasets[tooltipItem.datasetIndex].label || '';
          // console.log(data);
          // var labels = [];
          // for (let index = 0; index < this.ListaDeSectionInfo[tooltipItem.datasetIndex].length; index++) {
          //   const sectionInfo = this.ListaDeSectionInfo[tooltipItem.datasetIndex][index];
          //     labels.push(sectionInfo.nombre + ': ' + sectionInfo.respuestasCorrectas + '%');
          //   }
          // let nivel: string = '%  del nivel mínimo';
          //if(Number(tooltipItem.yLabel) > 100){
            
          //}
          if(data.datasets[tooltipItem.datasetIndex].label.includes('Global')){
            return  data.datasets[tooltipItem.datasetIndex].label + ': ' + Math.round((tooltipItem.yLabel) * 10)/10 + '%';
          }
          else{
          if (Number(tooltipItem.yLabel) % 100 == 0 && Number(tooltipItem.yLabel) >= 100){
            let nivel: string = '%  '+ ofLevel +' '+  Math.trunc(Number(tooltipItem.yLabel) / 100);
            return  data.datasets[tooltipItem.datasetIndex].label + ': 100' + nivel;
          }
          else{
            let nivel: string = '%  '+ ofLevel +' ' +  Math.trunc(Number(tooltipItem.yLabel) / 100 + 1);
            return  data.datasets[tooltipItem.datasetIndex].label + ': ' + Math.round((tooltipItem.yLabel%100) * 10)/10 + nivel;
          }
          }
        },
        // footer: function(tooltipItem, data) {
        //   return " Total" + ':     ' + tooltipItem[0].yLabel + '%';
        // },
        // title: function(tooltipItem, data) {
        //   return " " + tooltipItem[0].xLabel;
        // },
      }
    },
    scales: {
      yAxes: [{
        position: "left",
        id: "y-axis-0",
        ticks: {
          steps: this.MaxLevelReached * 10 + 10,
          stepValue: 10,
          maxTicksLimit: this.MaxLevelReached * 10 + 10,
          max: this.MaxLevelReached * 100 + 100,
          min: 0,
          callback: function(value, index, values) {
            if(self.allLegendsHidden){
              return "";
            }
            else{              
              if (Number(value) % 100 == 0 && Number(value) >= 100){
                return level+' ' + (Number(value) / 100 );
              }
              else{
                return Number(value)%100 + '%';
              }
            }
         }
        },
        gridLines: {
          display: true
        }
      },
      {
        position: "right",
        id: "y-axis-1",
        ticks: {
          steps: 10,
          stepValue: 10,
          min: 0,
          max: 100,
          callback: function(value, index, values) {
            if(this.chart.chart.config.data.datasets[this.chart.chart.config.data.datasets.length - 1].hidden){
              return "";
            }
            else{
              return Number(value) + '%';
            }
          }
        },
        gridLines: {
          display: false
        }
      }],
      xAxes: [{
        gridLines: {
          display:false,
        },
        maxBarThickness : 40
      }]
    }
  };
  }
   
  //Colores para la grafica
  public chartColors: Array<any> = [
    { // first color
      backgroundColor: 'rgba(92, 183, 92, 0.0)',
      borderColor: 'rgba(92, 183, 92, 0.0)',
      pointBackgroundColor: 'rgba(92, 183, 92, 0.0)',
      pointBorderColor: '#fff0',
      pointHoverBackgroundColor: '#fff0',
      pointHoverBorderColor: 'rgba(92, 183, 92, 0.0)'
    }];

  //Estos son los datos introducidos en la grafica para que represente sus formas
  public barChartData: any[] = [
    { data: this.ListaPuntuacion, label: 'Puntuacion' }
  ];

  public getUserRole(){
    this._proyectoService.getRolesUsuario().subscribe(
      res => {
        var permisosDeUsuario = res;
        this._appComponent._storageDataService.Role = permisosDeUsuario.role;
      },
      error => {
        
      });
  }
}
