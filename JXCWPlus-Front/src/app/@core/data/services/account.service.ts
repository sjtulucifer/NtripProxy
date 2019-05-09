import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountDetail } from '../entity/account-detail';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl = environment.webApiUrl + 'AccountAsy/';

  constructor(
    private http: HttpClient
  ) { }

  accountLogin(account: AccountDetail): Observable<any> {
    return this.http.post(this.baseUrl + 'AccountLogin', account);
  }
  
}
