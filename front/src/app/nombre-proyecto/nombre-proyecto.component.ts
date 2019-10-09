import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nombre-proyecto',
  templateUrl: './nombre-proyecto.component.html',
  styleUrls: ['./nombre-proyecto.component.scss']
})
export class NombreProyectoComponent implements OnInit {

  @Input() dataImput: string;

  constructor() { }

  public nombreCompleto: string;
  public nombre: string;
  public proyecto: string;
  public arrayNombre: string [];

  ngOnInit() {
    this.nombreCompleto = this.dataImput;
    this.arrayNombre = this.nombreCompleto.split("-");

    this.proyecto = this.arrayNombre[0].trim();
    if (this.arrayNombre.length > 1)
    {
      this.nombre = this.arrayNombre[1].trim();
    }
  }

}
