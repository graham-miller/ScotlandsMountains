import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { first } from 'rxjs/operators';
import { ClassificationSummary } from '../models/classification';
import { MountainDataService } from './mountain-data.service';

@Injectable({
  providedIn: 'root'
})
export class InitialDataService {

  constructor(
    private mountainDataService: MountainDataService
  ) { }

  private classificationsCache?: ReplaySubject<ClassificationSummary[]>;

  getClassifications(): Observable<ClassificationSummary[]> {

    if (!this.classificationsCache) {
      this.classificationsCache = new ReplaySubject<ClassificationSummary[]>(1)

      this.mountainDataService.getClassifications()
        .pipe(first())
        .subscribe(data => {
          this.classificationsCache?.next(data);
        });
    }

    return this.classificationsCache.asObservable();
  }
}
