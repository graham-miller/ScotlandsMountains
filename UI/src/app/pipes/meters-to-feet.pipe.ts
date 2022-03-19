import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'metersToFeet'
})
export class MetersToFeetPipe implements PipeTransform {

  transform(value: number): number {
    return Math.floor(value * 3.28084);
  }
}
