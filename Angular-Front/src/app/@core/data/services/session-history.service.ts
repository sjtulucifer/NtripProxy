import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SessionHistoryService {

  private baseUrl = environment.webApiUrl + 'SessionHistory/';

  constructor(
    private http: HttpClient
  ) { }

  //根据用户ID查找可显示的在线会话
  getOnlineSessionByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionByUserID/' + id);
  }

  //根据用户ID查找可显示的固定解在线会话
  getOnlineSessionFixedByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionFixedByUserID/' + id);
  }

  //根据用户ID查找可显示的浮点解在线会话
  getOnlineSessionFloatByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionFloatByUserID/' + id);
  }

  //根据用户ID查找可显示的单点解在线会话
  getOnlineSessionSingleByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionSingleByUserID/' + id);
  }

  //根据用户ID查找可显示的差分解在线会话
  getOnlineSessionDifferenceByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionDifferenceByUserID/' + id);
  }

  //根据用户ID查找可显示的其他解在线会话
  getOnlineSessionOtherByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'getOnlineSessionOtherByUserID/' + id);
  }
  
  //根据用户ID查找可显示的总体固定率
  getOnlineSessionFixedRateByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionFixedRateByUserID/' + id);
  }

  getOnlineSession(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSession');
  }

  getOnlineSessionByStatus(status: number): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionByStatus/' + status);
  }

  getOnlineSessionByStatusOther(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionByStatusOther');
  }

  getOnlineSessionFixedRate(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOnlineSessionFixedRate');
  }

  getSessionHistoryByAccount(account: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetSessionHistoryByAccount/' + account);
  }
}
