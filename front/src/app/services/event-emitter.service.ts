import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {

  eventEmitter = new EventEmitter();

  constructor() { }

  displayMessage(message: string){
    this.eventEmitter.emit(message);
  }
}
