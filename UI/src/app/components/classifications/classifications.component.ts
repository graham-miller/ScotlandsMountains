import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { Classification } from 'src/app/models/classification';
import { ClassificationSummary } from 'src/app/models/classification';
import { InitialDataService } from 'src/app/services/initial-data.service';
import { MountainDataService } from 'src/app/services/mountain-data.service';

@Component({
  selector: 'classifications',
  templateUrl: './classifications.component.html',
  styleUrls: ['./classifications.component.css']
})
export class ClassificationsComponent implements OnInit {

  constructor(
    private initialDataService: InitialDataService,
    private mountainsDataService: MountainDataService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  @ViewChild('table') table?: ElementRef;

  classifications: ClassificationSummary[] = []
  selectedClassificationId?: string;
  selectedClassification?: Classification;
  isLoading = true;
  isMapVisible = true;
  isListVisible = false;

  ngOnInit(): void {
    this.initialDataService.getClassifications()
      .pipe(first())
      .subscribe(data => {
        this.classifications = data;
        this.route.params.subscribe(params => {
          this.selectedClassificationId = params['id'];
          this.isLoading = true;
          this.showMap();
          this.loadClassification();
        });
      })
  }

  showMap() {
    this.isMapVisible = true;
    this.isListVisible = false;
  }

  showList() {
    this.isMapVisible = false;
    this.isListVisible = true;
  }

  private loadClassification() {
    if (!this.selectedClassificationId) {
      this.router.navigate([this.classifications[0].id], { relativeTo: this.route });
    } else {
      this.mountainsDataService.getClassification(this.selectedClassificationId)
        .subscribe(data => {
          this.selectedClassification = data;
          this.isLoading = false;
        });
    }
  }
}
