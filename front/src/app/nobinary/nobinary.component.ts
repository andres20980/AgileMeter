import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-nobinary',
  templateUrl: './nobinary.component.html',
  styleUrls: ['./nobinary.component.scss']
})
export class NobinaryComponent implements OnInit {
  private selected: boolean
  @ViewChild("myButton") myButton: ElementRef;
  constructor(private renderer: Renderer2) { }


  ngOnInit() {
  }

  choose(event: any)
  {
    // event.path[2].childNodes.map(x => x.childNodes[0].className.replace("questions-yes-btn", ""))
    console.log(Array.from(event.path[2].childNodes).map(x => x.childNodes[0].className.replace("questions-yes-btn", "")))
    event.target.className += " questions-yes-btn";
  }

}
