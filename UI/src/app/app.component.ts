import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { InitialDataService } from './services/initial-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(
    private initialDataService: InitialDataService
  ) { }

  isLoading = true;

  ngOnInit(): void {
    this.initialDataService.getClassifications()
      .pipe(first())
      .subscribe(data => {
        this.isLoading = false;
      })
  }
}
