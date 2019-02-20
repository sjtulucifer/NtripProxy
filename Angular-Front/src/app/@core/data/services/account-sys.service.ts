import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountSYS } from '../entities/account-sys';

@Injectable({
  providedIn: 'root'
})
export class AccountSYSService {

  private baseUrl = environment.webApiUrl + 'AccountSYS/';

  constructor(
    private http: HttpClient
  ) { }

  addAccountSYS(accountSYS: AccountSYS): Observable<any> {
    return this.http.post(this.baseUrl + 'AddAccountSYS', accountSYS);
  }

  updateAccountSYS(accountSYS: AccountSYS): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateAccountSYS', accountSYS);
  }

  getAccountSYSList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllAccountSYSs');
  }

  getAccountSYSCount(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllAccountSYSCount');
  }

  getAccountSYSOnlineCount(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllAccountSYSOnlineCount');
  }

  getAccountSYSByID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAccountSYSByID/' + id);
  }
  
}
