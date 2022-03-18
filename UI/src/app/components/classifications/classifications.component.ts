import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first, zip } from 'rxjs';
import { Classification, MountainSummary } from 'src/app/models/classification';
import { ClassificationSummary } from 'src/app/models/classification-summary';
import { InitialDataService } from 'src/app/services/initial-data.service';
import { MountainDataService } from 'src/app/services/mountain-data.service';
  
const FILTER_PAG_REGEX = /[^0-9]/g;

interface MountainSummaryWithPosition extends MountainSummary {
  position: number;
}

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

  classifications: ClassificationSummary[] = []
  selectedClassificationId?: string;
  selectedClassification?: Classification;
  isLoading = true;
  page = 1;
  pageSize = 10;
  pageData: MountainSummaryWithPosition[] = [];

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
              this.pageMountains();
              this.isLoading = false;
            });
        }
      });
  }
  
  selectPage(page: string) {
    this.page = parseInt(page, 10) || 1;
  }

  formatInput(input: HTMLInputElement) {
    input.value = input.value.replace(FILTER_PAG_REGEX, '');
  }

  pageMountains() {
    if (this.selectedClassification) {
      this.pageData = this.selectedClassification?.mountains
        .map((mountain, i) => ({position: i + 1, ...mountain}))
        .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
    } else {
      this.pageData = [];
    }
  }  
}
