import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first, zip } from 'rxjs';
import { Classification } from 'src/app/models/classification';
import { ClassificationSummary } from 'src/app/models/classification-summary';
import { InitialDataService } from 'src/app/services/initial-data.service';
import { MountainDataService } from 'src/app/services/mountain-data.service';

@Component({
  selector: 'classifications',
  templateUrl: './classifications.component.html',
  styleUrls: ['./classifications.component.css']
})
export class ClassificationsComponent implements OnInit {

  classifications: ClassificationSummary[] = []
  selectedClassificationId?: string;
  selectedClassification?: Classification;
  isLoading = true;

  constructor(
    private initialDataService: InitialDataService,
    private mountainsDataService: MountainDataService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    const params$ = this.route.params.pipe(first());
    const classifications$ = this.initialDataService.getClassifications().pipe(first());

    zip(params$, classifications$)
      .pipe(first())
      .subscribe(pair => {
        this.selectedClassificationId = pair[0]['id'];
        this.classifications = pair[1];

        if (!this.selectedClassificationId) {
          this.router.navigate([this.classifications[0].id], { relativeTo: this.route });
        } else {
          this.mountainsDataService.getClassification(this.selectedClassificationId)
            .subscribe(data => {
              this.selectedClassification = data;
              this.isLoading = false;
            });
        }
      });
  }
}
