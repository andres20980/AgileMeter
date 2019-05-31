import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';

@Component({
  selector: 'app-team-management',
  templateUrl: './team-management.component.html',
  styleUrls: ['./team-management.component.scss']
})
export class TeamManagementComponent implements OnInit {

  constructor(
    private _router: Router,
  ) { }

  ngOnInit() {
  }

  public volver() {
    this._router.navigate(['/backoffice/usermanagement']);
  }

}
