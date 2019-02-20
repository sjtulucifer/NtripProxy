import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { User } from '../entities/user';
import { Observable } from 'rxjs/Observable';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.webApiUrl + 'User/';
   }

   
  addUser(user: User): Observable<any> {
    return this.http.post(this.baseUrl + 'AddUser', user);
  }

  userLogin(user: User): Observable<any> {
    return this.http.post(this.baseUrl + 'Login', user);
  }

  softDeleteUser(id: string): Observable<any> {
    return this.http.put(this.baseUrl + 'SoftDeleteUserByID/' + id, null);
  }

  resetUserPassword(user: User): Observable<any> {
    return this.http.put(this.baseUrl + 'ResetUserPassword', user);
  }

  resetUserPasswordProfile(user: User): Observable<any> {
    return this.http.put(this.baseUrl + 'ResetUserPasswordProfile', user);
  }

  updateUser(user: User): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateUser', user);
  }

  updateUserRoles(obj: any): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateUserRoles', obj);
  }

  updateUserCompany(obj: any): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateUserCompany', obj);
  }

  getUserList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllUsers');
  }

  getUserByID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetUserByID/' + id);
  }

  getUserByLogin(name: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetUserByLogin/' + name);
  }

  getMenuByUserID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetMenusByUserID/' + id);
  }
}
