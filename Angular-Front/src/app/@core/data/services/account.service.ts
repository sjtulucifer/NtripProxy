import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account } from '../entities/account';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl = environment.webApiUrl + 'Account/';

  constructor(
    private http: HttpClient
  ) { }

  addAccount(account: Account): Observable<any> {
    return this.http.post(this.baseUrl + 'AddAccount', account);
  }

  updateAccount(account: Account): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateAccount', account);
  }

  updateAccountCompany(obj: any): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateAccountCompany', obj);
  }

  getAccountList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllAccounts');
  }

  getAccountOnline(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAccountsOnline');
  }

  getAccountByCompanyID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAccountByCompanyID/' + id);
  }

  getAccountByID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAccountByID/' + id);
  }

}
