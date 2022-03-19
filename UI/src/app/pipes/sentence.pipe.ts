import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sentence'
})
export class SentencePipe implements PipeTransform {

  transform(s?: string): string {

    let result = s;

    if (result && result.length > 0) {
      if (result.slice(result.length - 1) !== '.') {
        result += '.';
      }

      result = result.charAt(0).toUpperCase() + result.slice(1)
    }

    return result || '';
  }
}
