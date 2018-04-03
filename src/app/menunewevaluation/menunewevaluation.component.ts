import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-menunewevaluation',
  templateUrl: './menunewevaluation.component.html',
  styleUrls: ['./menunewevaluation.component.scss']
})
export class MenunewevaluationComponent implements OnInit {
  
  numeroPreguntasCeremonias: number=26;
  preguntasRespondidasCeremonias: number=15;
  total= (this.preguntasRespondidasCeremonias/this.numeroPreguntasCeremonias)*100;
  trun= Math.round(this.total*10)/10;

  numeroPreguntasRoles: number=15;
  preguntasRespondidasRoles: number=0;
  total2= (this.preguntasRespondidasRoles/this.numeroPreguntasRoles)*100;
  trun2= Math.round(this.total2*10)/10;

  numeroPreguntasArtefactos: number=28;
  preguntasRespondidasArtefactos: number=28;
  total3= (this.preguntasRespondidasArtefactos/this.numeroPreguntasArtefactos)*100;
  trun3= Math.round(this.total3*10)/10;

  constructor() { }

  ngOnInit() {
    
  }

 
}
