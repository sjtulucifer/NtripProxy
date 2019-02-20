import { Component, OnInit } from '@angular/core';
import { NbMenuItem } from '@nebular/theme';
import { User } from '../@core/data/entities/user';
import { Menu } from '../@core/data/entities/menu';

@Component({
  selector: 'ngx-pages',
  template: `
    <ngx-sample-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-sample-layout>
  `,
})
export class PagesComponent implements OnInit {

  menu: NbMenuItem[] = new Array();

  ngOnInit(): void {
    //this.menu = MENU_ITEMS;
    let user: User = JSON.parse(sessionStorage.getItem("loginUser")) as User;
    this.menu.push( {
      title: '北斗定位管理系统',
      group: true,
    }) 
    this.menu.push.apply(this.menu, this.initialMenu(user));
  }

  private initialMenu(user: User): NbMenuItem[] {
    let items: NbMenuItem[] = new Array();
    let menuList: Menu[] = new Array();
    //获取所有菜单列表并添加到sessionStorage
    user.Roles.forEach(
      //角色层
      (role, i) => {
        //权限层          
        role.Permissions.forEach(
          (permission, j) => {
            //菜单层
            permission.Menus.forEach(
              (menu, k) => {
                //如果不存在则添加
                if (JSON.stringify(menuList).indexOf(JSON.stringify(menu)) === -1) {
                  menuList.push(menu);
                }
              }
            );
          }
        );
      });
    sessionStorage.setItem("menuList", JSON.stringify(menuList));

    menuList.forEach(element => {
      let menuItem: NbMenuItem = new NbMenuItem();
      if (items.findIndex(
        (value, i, arr) => {
          menuItem = value;
          return value.title === element.Catagory;
        }
      ) === -1) {
        const temp: NbMenuItem = new NbMenuItem();
        temp.title = element.Catagory;
        //设置图标
        if (element.Catagory === '代理设置') {
          temp.icon = 'nb-home';
        } else if (element.Catagory === '系统设置') {
          temp.icon = 'nb-gear';
        } else if (element.Catagory === '实时状态') {
          temp.icon = 'nb-location'
        }
        temp.expanded = true;
        temp.children = new Array<NbMenuItem>();
        temp.children.push({ title: element.Name, link: element.Url });
        items.push(temp);
      } else {
        menuItem.children.push({ title: element.Name, link: element.Url });
      }
    });
    return items;
  }

}
