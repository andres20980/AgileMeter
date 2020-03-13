import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { AppComponent } from 'app/app.component';
import { Chart } from 'chart.js';


@Component({
  selector: 'app-evaluationchart',
  templateUrl: './evaluationchart.component.html',
  styleUrls: ['./evaluationchart.component.scss']
})
export class EvaluationchartComponent implements OnInit, AfterViewInit {


  @Input() nombreProyecto: string;
  @Input() nombreAssessment: string;
  @Input() result: any;
  public chartType: string = 'bar';
  public maxLevel: number;
  public chartLabels: Array<string>;
  public chartData: Array<number>;
  public arr = new Array();
  public ctx_datasets = new Array();
  public ctx_labels = new Array();
  public arrayScrum = new Array();
  public arrayDevops = new Array();
  public auxColors = ['rgba(255, 240, 155, 0.1)','rgba(137, 236, 160, 0.1)']
  public chartOptions: any
  public infoResult: Array<Object> = [];
  public previous: any;
  public post: any;
  
  constructor(private _appComponent: AppComponent) {
    

   this.arrayScrum = [
      {nombre: "EQUIPO", color: "red"},
      {nombre: "EVENTOS", color: "pink"},
      {nombre: "HERRAMIENTAS",color: "purple"},
      {nombre: "MINDSET", color: "green"},
      {nombre: "APLICACIÓN PRÁCTICA", color: "blue"}
    ]
  
    this.arrayDevops = [
      {nombre: "ORGANIZACION EQUIPOS", color: "red"},
      {nombre: "CICLO DE VIDA", color: "pink"},
      {nombre: "CONSTRUCCIÓN",color: "purple"},
      {nombre: "TESTING Y CALIDAD", color: "green"},
      {nombre: "DESPLIEGUE", color: "blue"},
      {nombre: "MONITORIZACION", color: "lavender"},
      {nombre: "APROVISIONAMIENTO", color: "orange"}
    ];

  }

  ngOnInit() {
    this._appComponent.pushBreadcrumb(this.nombreProyecto, "/evaluationresults");
    this.previous = this.result.map(x => x.sectionsInfo);
    this.ctx_labels = this.result.map(x => {return this.parseDate(x.fecha)})
    this.ctx_labels.reverse();
    this.ctx_labels.push("");
    this.ctx_labels.unshift("");
    console.log(this.previous)
    let levels = this.previous.flatMap(c => c).map(x => x.nivelAlcanzado)
    this.maxLevel = Math.max(...levels)
    this.post = this.previous.flatMap(c => c).reduce((x, y) => {
                this.arr.includes(y.nombre) ? false : x.push(this.extracData(y.nombre,this.previous))                       
                this.arr.push(y.nombre);
                return x },[])


      if(this.nombreAssessment === "SCRUM") {
        this.arrayScrum.forEach((v,c) => {
          this.ctx_datasets.push({
            type: "line",
            data: this.post[c],
            label: v.nombre,
            backgroundColor: v.color,
            fill: "false",
            lineTension: 0.1,
            borderColor: v.color,
            pointRadius: 2,
            pointHoverRadius: 4,
            borderWidth: 3
          })
        })
      }

      if(this.nombreAssessment === "DEVOPS") {
        this.arrayDevops.forEach((v,c) => {
          this.ctx_datasets.push({
            type: "line",
            data: this.post[c],
            label: v.nombre,
            backgroundColor: v.color,
            fill: "false",
            lineTension: 0.1,
            borderColor: v.color,
            pointRadius: 2,
            pointHoverRadius: 4,
            borderWidth: 3
          })
        })
      }



   this.createAux(levels);
  }

  createAux(levels: [])
  {
    let fillevels = levels.filter((a, b) => levels.indexOf(a) === b)
    let follow = [1,1,1,1,1,1]
    fillevels.map((col, i) => {
      this.ctx_datasets.push({type: "line",
              data: follow.map(x => x * (col * 100)), 
              label:"aux" + (col - 1),
              backgroundColor: this.auxColors[i],
              fill: "origin",
              lineTension: 0.1,
              borderColor: this.auxColors[i],
              pointRadius: 0,
              pointHoverRadius: 0,
              borderWidth: 0.1})
    })
  }

  
  ngAfterViewInit(){
    new Chart(document.getElementById("line-chart"), {
      type: 'line',
      data: {
        labels: this.ctx_labels,
        datasets: this.ctx_datasets
      },
      options: {
        tooltips: {
          callbacks: {
            label: function(tooltipItem, data) {
              let numfx
              if(tooltipItem.yLabel > 200){
                 numfx = tooltipItem.yLabel.toFixed(2) -200 + "%"
              } else if(tooltipItem.yLabel > 100){
                 numfx = tooltipItem.yLabel.toFixed(2) -100 + "%"
              } else {
                numfx = tooltipItem.yLabel.toFixed(2) + "%"
              }
              return data.datasets[tooltipItem.datasetIndex].label +" "+numfx
            }
          }
        },
        caleShowVerticalLines: false,
        showLines: true,
        legend: {
          labels: {
              filter: function(item, chart) {
                  // Logic to remove a particular legend item goes here
                  return !item.text.includes('aux');
              }
          },
         
        },
        scales: {
          yAxes: [{
            position: "left",
            id: "y-axis-0",
            ticks: {
              steps: 100 * 10 + 10,
              stepSize: 20,
              maxTicksLimit: 1 * 10 + 10,
              max: (this.maxLevel - 1) * 100 + 100,
              min: 0,
              callback: function(value, index, values) {
                if(false){
                  return "";
                }
                else{ 
                  if (Number(value) % 100 == 0 && Number(value) >= 100){

                    return "Nivel" +' ' + (Number(value) / 100 );
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
                if(true){
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
      }
    });
  }

  extracData(y: string, p: Array<any>)
  {
    let a = new Array();
    a.unshift('NaN')
    a.push(p.flatMap(c => c).filter(x => x.nombre === y).map(k => {return k.puntuacion + ((k.nivelAlcanzado - 1)* 100)}))
    a.push('NaN')
    return a.flatMap(x => x).reverse()
  }

  parseDate(date: string) : string
  {
    let ndate = date.indexOf('T');
    return  date.substr(0, ndate).split("-").reverse().join(" / ");
  }
}