import { Component } from '@angular/core';
import { StorageDataService } from './services/StorageDataService';
import * as $ from 'jquery';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [StorageDataService]
})
export class AppComponent {
    constructor(public _storageDataService : StorageDataService){

    }
}
