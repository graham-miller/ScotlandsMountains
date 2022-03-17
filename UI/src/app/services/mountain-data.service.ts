import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscription, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MountainDataService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getClassifications(): Observable<any> {
    return this.httpClient.get<any>('api/classifications');
  }
}
