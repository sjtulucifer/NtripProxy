import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Menu } from '../entities/menu';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  
  private baseUrl = environment.webApiUrl + 'Menu/';

  constructor(
    private http: HttpClient
  ) { }

  addMenu(menu: Menu): Observable<any> {
    return this.http.post(this.baseUrl + 'AddMenu', menu);
  }

  deleteMenu(id: string): Observable<any> {
    return this.http.put(this.baseUrl + 'SoftDeleteMenuByID/' + id, null);
  }

  updateMenu(menu: Menu): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateMenu', menu);
  }

  getMenuList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllMenus');
  }
}
