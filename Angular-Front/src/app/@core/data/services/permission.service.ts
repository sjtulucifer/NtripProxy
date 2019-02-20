import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Permission } from '../entities/permission';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {

  private baseUrl: string;

  constructor(
    private http: HttpClient
  ) {
    this.baseUrl = environment.webApiUrl + 'Permission/';
  }

  addPermission(permission: Permission): Observable<any> {
    return this.http.post(this.baseUrl + 'AddPermission', permission);
  }

  deletePermission(id: string): Observable<any> {
    return this.http.put(this.baseUrl + 'SoftDeletePermissionByID/' + id, null);
  }

  updatePermission(permission: Permission): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdatePermission', permission);
  }

  updatePermissionMenus(obj: any): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdatePermissionMenus', obj);
  }

  getPermissionList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllPermissions');
  }

  getPermissionByID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetPermissionByID/' + id);
  }
}
