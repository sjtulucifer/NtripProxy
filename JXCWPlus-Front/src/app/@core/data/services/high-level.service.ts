import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { HighLevel } from '../entity/high-level';

@Injectable({
  providedIn: 'root'
})
export class HighLevelService {

  private baseUrl = environment.webApiUrl + 'highLevel/';

  constructor(
    private http: HttpClient
  ) {
  }

  PostCsvFile(file: any): Observable<any> {
    return this.http.post(this.baseUrl + 'PostCsv', file);
  }

  PostHighLevelFile(file: any): Observable<any> {
    return this.http.post(this.baseUrl + 'FittingHighLevel', file);
  }

  UploadHighLevelFile(fileName: string, file: any): Observable<any> {
    return this.http.post(this.baseUrl + 'UpLoadHighLevelFile/' + fileName, file);
  }

  ComputeHighLevelFile(fileName: string): Observable<any> {
    return this.http.get(this.baseUrl + 'ComputeHighLevelFile/' + fileName);
  }

  ComputeHighLevel(highLevel: HighLevel): Observable<any> {
    return this.http.post(this.baseUrl + 'ComputeHighLevel/', highLevel);
  }

  test(): Observable<any> {
    return this.http.get("http://localhost:12345/api/csv/GetCsv");
  }
}
