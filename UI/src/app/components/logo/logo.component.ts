import { Component, Input } from '@angular/core';

@Component({
  selector: 'logo',
  templateUrl: './logo.component.svg',
  styleUrls: ['./logo.component.css']
})
export class LogoComponent {

  @Input() color = 'rgba(0,0,0,.9)';
  @Input() height = '1em';
  @Input() width = '1em';


}
