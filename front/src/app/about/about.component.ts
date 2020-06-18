import { Component, OnInit, HostListener } from '@angular/core';
import { ProyectoService } from '../services/ProyectoService';
import { Router } from "@angular/router";
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss'],
  providers: [ ProyectoService]
})

export class AboutComponent implements OnInit {
  public ScreenWidth;


  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.ScreenWidth = window.innerWidth;
  }

  constructor(
    private _proyectoService: ProyectoService,
    private _router: Router,
    private _appComponent: AppComponent
   ) {


  }

  

  ngOnInit() {
    if (!this._proyectoService.verificarUsuario()) {
      this._router.navigate(['/login']);
    } 

    this._appComponent.pushBreadcrumb("BREADCRUMB.ABOUT", "/about");
  }
}


