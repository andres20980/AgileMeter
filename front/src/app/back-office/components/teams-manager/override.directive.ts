import {Directive, HostListener, Input, ElementRef, Renderer2} from '@angular/core';
import {VERSION, MatTooltip} from '@angular/material';
import { SRCSET_ATTRS } from '@angular/core/src/sanitization/html_sanitizer';

@Directive({
  selector: '[textoverride]',
  providers: [MatTooltip]
})
export class OverrideDirective {

  tooltip: MatTooltip;

  @Input('textoverride') myDir: any;

  constructor(tooltip: MatTooltip, private el: ElementRef, private renderer: Renderer2) {
    this.tooltip = tooltip;
  }

  @HostListener('mouseover', ['$event.target']) mouseover(e) {

    if(this.el.nativeElement.attributes['overflow']) {
      this.tooltip.message = this.el.nativeElement.attributes.overflow.value
      this.tooltip.show();
    }

  }

  ngAfterViewInit() {
    let original = this.el.nativeElement.innerHTML
    let substring
    if(original.length > 20) {
      substring = original.substr(0,8);
      this.el.nativeElement.innerHTML = substring + "...";
      this.renderer.setAttribute(this.el.nativeElement, 'overflow', original);
    }
    

  }

}
