import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';


@Component({
  selector: 'app-no-binary',
  templateUrl: './no-binary.component.html',
  styleUrls: ['./no-binary.component.scss']
})
export class NoBinaryComponent implements OnInit {
  private selected: boolean
  @ViewChild("myButton") myButton: ElementRef;
  constructor(private renderer: Renderer2) { }


  ngOnInit() {
  }

  choose(event: any)
  {
    event.path[2]['childNodes'].forEach(element => {
      element['childNodes'][0].className = "w-100 h-100 mat-elevation-z3 item-not-selected"
    })
    
    setTimeout(() => {
      event.target.className =  event.target.className.replace("item-not-selected", "pid") + " questions-yes-btn";
    }, 10)
  }

}