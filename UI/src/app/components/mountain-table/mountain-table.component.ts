import { Component, Input, OnChanges, QueryList, SimpleChanges, ViewChildren } from '@angular/core';
import { SortableHeader, SortDirection, SortEvent } from 'src/app/directives/sortable-header';
import { MountainSummary } from 'src/app/models/mountain';

interface MountainSummaryWithPosition extends MountainSummary {
  position: number;
}

@Component({
  selector: 'mountain-table',
  templateUrl: './mountain-table.component.html',
  styleUrls: ['./mountain-table.component.css']
})
export class MountainTableComponent implements OnChanges {

  @Input() mountains: MountainSummary[] = [];

  constructor() { }

  @ViewChildren(SortableHeader) sortHeaders?: QueryList<SortableHeader>;

  pageNumber = 1;
  pageSize = 10;
  pageData: MountainSummaryWithPosition[] = [];
  sortColumn = 'position';
  sortDirection: SortDirection = 'asc';

  ngOnChanges(changes: SimpleChanges): void {
    this.pageNumber = 1;
    this.updatePageData();
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
    let result = this.mountains.map((mountain, i) => ({ position: i + 1, ...mountain }))
    result = this.sort(result);
    result = this.page(result)

    this.pageData = result;
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
