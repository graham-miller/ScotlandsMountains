import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscription, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ClassificationSummary } from '../models/classification-summary';
import { Classification } from '../models/classification';

@Injectable({
  providedIn: 'root'
})
export class MountainDataService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getClassifications(): Observable<ClassificationSummary[]> {
    return this.httpClient.get<ClassificationSummary[]>('api/classifications');
  }

  getClassification(id: string) {
    return this.httpClient.get<Classification>(`api/classifications/${id}`);
  }
}
