import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'subsProject'
})
export class SubsProjectPipe implements PipeTransform {

  transform(value: any, extension: number): any {
    const limitsbs = value.indexOf(" - ");   
    if(extension) {
      value = value.substring(0, limitsbs + 3)
      return value
    } else  {
       value = value.substring(limitsbs + 3, value.length)
      return value
    }
  }
}
