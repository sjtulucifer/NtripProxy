import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Log } from '../entities/log';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogService {

  private baseUrl = environment.webApiUrl + 'Log/';

  constructor(
    private http: HttpClient
  ) { }

  addLog(log: Log): Observable<any> {
    return this.http.post(this.baseUrl + 'AddLog', log);
  }
}
