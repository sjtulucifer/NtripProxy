import { Injectable } from '@angular/core';
import { Menu } from '../entities/menu';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  checkURLAccess(url: string): boolean {
    let menuList: Menu[] = JSON.parse(sessionStorage.menuList) as Menu[];    
    let result: boolean = false;
    for (let i = 0; i < menuList.length; i++) {
      if (url.indexOf(menuList[i].Url) >= 0) {
        result = true;
        break;
      }
    }
    return result;
  }
}
