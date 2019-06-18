import { Component, OnInit } from '@angular/core';
import { AppComponent } from 'app/app.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {

  public breadcrumbList: Array<any> = [];

  constructor(
    private _appComponent: AppComponent,
    private _router: Router) {
    this.breadcrumbList = this._appComponent._storageDataService.breadcrumbList;
  }

  ngOnInit() {
   console.log(this.breadcrumbList);
  }

  navigateTo(i: number) {
    let path: string =  this._appComponent.getBreadcrumb(i).path;
    this._appComponent.popBreadcrumb(i);
    this._router.navigate([path]);
  }


}
