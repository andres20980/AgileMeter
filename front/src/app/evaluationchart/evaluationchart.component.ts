import { style } from '@angular/animations';
import { Component, OnInit, Input, AfterViewInit ,ViewChild, ViewChildren, ElementRef } from '@angular/core';
import { AppComponent } from 'app/app.component';
import { Chart } from 'chart.js';
import { ThrowStmt } from '@angular/compiler';


@Component({
  selector: 'app-evaluationchart',
  templateUrl: './evaluationchart.component.html',
  styleUrls: ['./evaluationchart.component.scss']
})
export class EvaluationchartComponent implements OnInit, AfterViewInit {

  canvas: any;
  ctx: any;
  @ViewChildren('mychart') mychart: ElementRef;
  @Input() nombreProyecto: string;
  @Input() nombreAssessment: string;
  @Input() result: any;
  public chartType: string = 'bar';
  public maxLevel: number;
  public chartLabels: Array<string>;
  public chartData: Array<number>;
  public chart: Chart;
  public arr = new Array();
  public ctx_datasets = new Array();
  public ctx_labels = new Array();
  public arrayScrum = new Array();
  public arrayDevops = new Array();
  public auxColors = [ "#c1de5d40","#37bf5940","#0fb3d440"] //rgba(135,169,0,0.1)
  public chartOptions: any
  public infoResult: Array<Object> = [];
  public previous: any;
  public post: any;
  public levels: any;
  public hiddenGlobal: boolean;
  public genericResult: Array<number>
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

    this.hiddenGlobal = true;
    this._appComponent.pushBreadcrumb(this.nombreProyecto, "/evaluationresults");
    this.previous = this.result.map(x => x.sectionsInfo);
    this.genericResult = this.result.map(x => x.puntuacion)
    this.ctx_labels = this.result.map(x => {return this.parseDate(x.fecha)})
    this.ctx_labels.reverse();
    this.ctx_labels.push("");
    this.ctx_labels.unshift("");
    console.log(this.previous)
    this.levels = this.previous.flatMap(c => c).map(x => x.nivelAlcanzado)
    this.maxLevel = Math.max(...this.levels)
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
     
   this.ctx_datasets.push({type: 'bar', yAxisID: "y-axis-1", data: [NaN,...this.genericResult,NaN], label: "Global", backgroundColor: "#2ECC71AA", padding: 200, borderColor: "#2ECC71", hoverBackgroundColor: "#2ECC71", borderWidth:"2", hidden:"false"})

   this.createAux(this.levels);
   
  }


  createAux(levels: [])
  {
    // let fillevels = levels.filter((a, b) => levels.indexOf(a) === b)
    // let follow = [1,1,1,1,1,1,1]
    // fillevels.forEach((col, i) => {
    //   console.log(follow.map(x => x * (col * 100)))
    //   if(i === 0) {
    //     this.ctx_datasets.push({
    //       type: "line",
    //       data: follow.map(x => x * (col * 100)), 
    //       label:"aux1",// + (col - 1),
    //       backgroundColor: this.auxColors[0],
    //       fill: "origin",
    //       lineTension: 0.1,
    //       borderColor: this.auxColors[0],
    //       pointRadius: 0,
    //       pointHoverRadius: 0,
    //       borderWidth:0.1
    //     })
    //   } else {        
    //     this.ctx_datasets.push({
    //     type: "line",
    //     data: follow.map(x => x * (col * 100)), 
    //     label:"aux1",// + (col - 1),
    //     backgroundColor: this.auxColors[i],
    //     fill: "-1",
    //     lineTension: 0.1,
    //     borderColor: this.auxColors[i],
    //     pointRadius: 0,
    //     pointHoverRadius: 0,
    //     borderWidth:0.1
    //   }) 
    // }
    //   })
        
        this.ctx_datasets.push({
          type: "line",
          data:[100,100,100,100,100,100,100], //follow.map(x => x * (col * 100)), 
          label:"aux1",// + (col - 1),
          backgroundColor: this.auxColors[0],
          fill: "origin",
          lineTension: 0.1,
          borderColor: this.auxColors[0],
          pointRadius: 0,
          pointHoverRadius: 0,
          borderWidth:0.1
        })


        this.ctx_datasets.push({
          type: "line",
          data: [200,200,200,200,200,200,200],//follow.map(x => x * (col * 100)), 
          label:"aux1",// + (col - 1),
          backgroundColor: this.auxColors[1],
          fill: "-1",
          lineTension: 0.1,
          borderColor: this.auxColors[1],
          pointRadius: 0,
          pointHoverRadius: 0,
          borderWidth:0.1
        }) 


        
        this.ctx_datasets.push({
          type: "line",
          data: [300,300,300,300,300,300,300],//follow.map(x => x * (col * 100)), 
          label:"aux2",// + (col - 1),
          backgroundColor: this.auxColors[2],
          fill: "-1",
          lineTension: 0.1,
          borderColor: this.auxColors[2],
          pointRadius: 0,
          pointHoverRadius: 0,
          borderWidth:0.1
        }) 

}
    
  

  
  ngAfterViewInit(){
    const self = this
    this.chart =  new Chart(document.getElementById("line-chart"), {
      type: 'bar',
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
          align:"start",

          labels: {
            padding: 40,
            usePointStyle: true,
            filter: function(item, chart) {
                  // Logic to remove a particular legend item goes her
                  // const myImage = new Image(50, 50);
                  // myImage.src = "https://banner2.cleanpng.com/20180329/gve/kisspng-computer-icons-chart-icon-design-diagram-finance-5abc92600af0c5.8926903915223076800448.jpg"
                  if(item.text.includes('Global')){
                    //self.hiddeGlobal(item.hidden)
                  }
                 if(item.text.includes('Global')){ 
                   item.pointStyle = "cirlce"
                  }
                   else {
                    item.pointStyle = "rect"
                    item.lineWidth = 10
                   }
                  return !item.text.includes('aux');
              },
          },
        
         
        },
        responsive: true,
        legendCallback: function(chart) {

          console.log(chart)
          let legend : any[] = [];
          for (var i=0; i<chart.data.datasets.length; i++) {  
            if(!chart.data.datasets[i].label.includes('aux') && !chart.data.datasets[i].label.includes('Global')) {  
                legend.push({label: chart.data.datasets[i].label, color: chart.data.datasets[i].backgroundColor});
            }
          }
          return legend;
      
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
              steps: 100,
              stepValue: 10,
              min: 0,
              max: 100,
              callback: function(value, index, values) {           
                if(false){
                  return "";
                }
                else{
                  return value+'%'
                }
                
              }
            },
            gridLines: {
              display: false
            }
          }],
          xAxes: [{
            offset: false,
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
    a.unshift("NaN")
    a.push(p.flatMap(c => c).filter(x => x.nombre === y).map(k => {return k.puntuacion + ((k.nivelAlcanzado - 1)* 100)}))
    a.push("NaN")

    return a.flatMap(x => x).reverse()
  }

  parseDate(date: string) : string
  {
    let ndate = date.indexOf('T');
    return  date.substr(0, ndate).split("-").reverse().join(" / ");
  }

  hiddeGlobal(v: boolean){
    this.hiddenGlobal = v
    return v
  }
}