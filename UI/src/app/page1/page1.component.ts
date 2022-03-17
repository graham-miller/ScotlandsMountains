import { Component, OnInit } from '@angular/core';
import { MountainDataService } from '../services/mountain-data.service';

@Component({
  selector: 'sm-page1',
  templateUrl: './page1.component.html',
  styleUrls: ['./page1.component.css']
})
export class Page1Component implements OnInit {
  test: string = ''

  constructor(private mountains: MountainDataService) {}
  
  ngOnInit(): void {
    // this.mountains.getClassifications().subscribe(x => {
    //   this.test = JSON.stringify(x);
    // });
  }
}
Component