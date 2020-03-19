
import { Component, OnInit, isDevMode, Input} from '@angular/core';
import { EvaluacionInfo } from 'app/Models/EvaluacionInfo';
import { EvaluacionService } from '../services/EvaluacionService';
import { EvaluacionFilterInfo } from 'app/Models/EvaluacionFilterInfo';
import { ProyectoService } from 'app/services/ProyectoService';
import { Proyecto } from 'app/Models/Proyecto';
import { AppComponent } from 'app/app.component';
import { Router } from '@angular/router';
import { mergeMap, filter, map } from 'rxjs/operators'
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-previousevaluation',
  templateUrl: './previousevaluation.component.html',
  styleUrls: ['./previousevaluation.component.scss'],
  providers: [EvaluacionService, ProyectoService]
})
export class PreviousevaluationComponent implements OnInit {
  public Project: Proyecto = { 'id': null, 'nombre': '', codigo: null, 'fecha': null, numFinishedEvals:0, numPendingEvals: 0, oficina:null, proyecto: ''};
  public ListaDeEvaluacionesPaginada: Array<EvaluacionInfo>;
  public ListaDeEvaluacionesMerge: Array<EvaluacionInfo>;
  public EvaluacionFiltrar: EvaluacionFilterInfo = { 'nombre': '', 'estado': 'true', 'fecha': '', 'userNombre': '', 'puntuacion': '', 'assessmentId': 0 , 'oficinas':[], equipos:[], 'idAssessment': [] };
  public sectionsInfoNombres$: Array<any>;
  public sectionsInfoPuntuacion$: Array<any>;
  public sectionsInfoProject$: Object;
  public uniqueSelectTeam: boolean = false;
  public activeTable: boolean = true;
  public activateChart: boolean = false;
  public ProjectSelectName: string;
  public AssessmentSelectName: string;
  public prevResult: any;
  public finishMerge: boolean = true;
  public enableColumns: boolean = false;

  constructor(private _proyectoService: ProyectoService, public evaluacionService: EvaluacionService, 
    private _router: Router, private _appComponent: AppComponent ) { 
  }

  ngOnInit() {
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    }
    this.GetPaginacion();
  }

  public GetPaginacion() {
    let filterEvaluation: EvaluacionFilterInfo = new EvaluacionFilterInfo("","","","","true",  1,[],[],[]);
      this.evaluacionService.GetEvaluationsWithSectionsInfo(2, filterEvaluation, 1)
        .subscribe(res => {
          console.log("wininfo");
          console.log(res);
          this.prevResult = res.evaluacionesResult;
          this.ListaDeEvaluacionesPaginada = res.evaluacionesResult.reduce((acc, item) => {
            item.oficina = item.oficina.trim()
            item.sectionsInfo.map(x => x.puntuacion = Number(x.puntuacion.toFixed(2)))
            return [...acc, item]
          },[]);
          this.sectionsInfoNombres$ = res.evaluacionesResult.sectionsInfo
          this.sectionsInfoPuntuacion$ = res.evaluacionesResult.sectionsInfo
        });
    // this.evaluacionService.getAllEvaluacionInfoFiltered(0, this.EvaluacionFiltrar)
    //   .subscribe(
    //     res => {
    //       this.ListaDeEvaluacionesPaginada = res.evaluacionesResult.reduce((acc, item) => {
    //       item.nombre = item.nombre.replace("##?##", " - ")
    //       return [...acc, item]
    //     },[])
    //     })
    }

    procesaPropagar(mensaje: any) {
    if(!mensaje) {
       this.uniqueSelectTeam = false;
         this.enableColumns = false;
        return
     } else {
      this.ProjectSelectName = mensaje[0].nombre;
      this.AssessmentSelectName = mensaje[0].assessmentName
      this.prevResult = mensaje;
      this.enableColumns = true;
      this.uniqueSelectTeam = true;
     }
    }


    procesaMerge(mensaje: any){
      this.ListaDeEvaluacionesMerge = [];
      this.finishMerge = false;
      let filterEvaluation: EvaluacionFilterInfo = new EvaluacionFilterInfo("","","","","true",  mensaje.idAssessment,[],[],[]);
      let obm = this.evaluacionService.getAllEvaluacionInfoFilteredToMerge(0, this.EvaluacionFiltrar)//.subscribe(s => console.log(s))
      let obm2 = this.evaluacionService.GetEvaluationsWithSectionsInfoToMerge(mensaje.idProyecto,filterEvaluation)//.subscribe(s => console.log(s))
    }
 
    setGreyOut(ent: string)
    {
      if(this.activateChart){
        if(ent === "tabla") {
         return 0.5
        }
        return 1
      }
       else {
       if(ent === "tabla") {
          return 1
        }
        this.setBrightness();
        return 0.5
       }

    }

    setBrightness()
    {
      if(this.uniqueSelectTeam){
        return  'brightness(100%)'
      } else {
        return  'brightness(50%)';
      }
    }

    activaGrafica()
    {
      if(this.uniqueSelectTeam){
        this.activateChart = true;
      } else {
        this.activateChart = false;
      }
    }

    desGrafica()
    {
      this.activateChart = false;
      this.activeTable = true;
    }
}
