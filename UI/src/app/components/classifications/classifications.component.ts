import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first, zip } from 'rxjs';
import { SortableHeader, SortDirection, SortEvent } from 'src/app/directives/sortable-header';
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
  
  @ViewChildren(SortableHeader) sortHeaders?: QueryList<SortableHeader>;

  classifications: ClassificationSummary[] = []
  selectedClassificationId?: string;
  selectedClassification?: Classification;
  isLoading = true;
  pageNumber = 1;
  pageSize = 10;
  pageData: MountainSummaryWithPosition[] = [];
  sortColumn = 'position';
  sortDirection: SortDirection = 'asc';

  ngOnInit(): void {

    const params$ = this.route.params.pipe(first());
    const classifications$ = this.initialDataService.getClassifications().pipe(first());

    zip(params$, classifications$)
      .pipe(first())
      .subscribe(pair => {
        this.selectedClassificationId = pair[0]['id'];
        this.classifications = pair[1];
        this.loadClassification();
      });
  }

  selectPage(page: string) {
    this.pageNumber = parseInt(page, 10) || 1;
  }

  formatPageInput(input: HTMLInputElement) {
    input.value = input.value.replace(FILTER_PAG_REGEX, '');
  }

  pageMountains() {
    this.updatePageData();
  }

  sortMountains({ column, direction }: SortEvent) {
    this.pageNumber = 1;    
    this.sortColumn = column;
    this.sortDirection = direction;
    this.resetOtherSortHeaders();
    this.updatePageData();
  }

  private loadClassification() {
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
  }

  private resetOtherSortHeaders() {
    if (this.sortHeaders) {
      this.sortHeaders.forEach(header => {
        if (header.sortable !== this.sortColumn) {
          header.direction = '';
        }
      });
    }
  }

  private updatePageData() {
    let mountains = this.selectedClassification
      ? this.selectedClassification.mountains.map((mountain, i) => ({ position: i + 1, ...mountain }))
      : [];

    mountains = this.sort(mountains);
    mountains = this.page(mountains)

    this.pageData = mountains;
  }

  private sort(mountains: MountainSummaryWithPosition[]): MountainSummaryWithPosition[] {
    if (this.sortColumn !== '' && this.sortDirection != '') {
      const column = this.sortColumn as keyof MountainSummaryWithPosition;
      return [...mountains].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return this.sortDirection === 'asc' ? res : -res;
      });
    }
    
    return mountains;
  }

  private page(mountains: MountainSummaryWithPosition[]): MountainSummaryWithPosition[] {
    return mountains.slice((this.pageNumber - 1) * this.pageSize, (this.pageNumber - 1) * this.pageSize + this.pageSize);
  }

  private compare(v1: string | number, v2: string | number): number {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
  }
}
