import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Role } from '../entities/role';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.webApiUrl + 'Role/';
  }

  addRole(role: Role): Observable<any> {
    return this.http.post(this.baseUrl + 'AddRole', role);
  }

  deleteRole(id: string): Observable<any> {
    return this.http.put(this.baseUrl + 'SoftDeleteRoleByID/' + id, null);
  }

  updateRole(role: Role): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateRole', role);
  }

  updateRolePermissions(obj: any): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateRolePermissions', obj);
  }

  getRoleList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllRoles');
  }

  getRoleByID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetRoleByID/' + id);
  }
}
