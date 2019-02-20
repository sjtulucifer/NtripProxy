import { Component, OnInit } from '@angular/core';
import { Menu } from '../../../../@core/data/entities/menu';
import { MenuService } from '../../../../@core/data/services/menu.service';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.scss']
})
export class MenuListComponent implements OnInit {

  menuList: Menu[] = new Array<Menu>();
  settings = {
    noDataMessage: '无数据',
    actions: {
      columnTitle: '操作',
    },
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
      confirmCreate: true,
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
      confirmSave: true,
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      Catagory: {
        title: '类别',
        type: 'html',
        editor: {
          type: 'list',
          config: {
            list: [
              { value: '实时状态', title: '实时状态' },
              { value: '代理设置', title: '代理设置' },
              { value: '系统设置', title: '系统设置' },
            ],
          },
        },
      },
      Name: {
        title: '菜单名',
        type: 'string',
      },
      Url: {
        title: '路径',
        type: 'string',
      },
      Description: {
        title: '菜单描述',
        type: 'string',
      }
    },
  };

  constructor(
    private menuService: MenuService,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.getMenuList();
  }

  getMenuList(): void {
    this.menuService.getMenuList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.menuList = res.Data as Menu[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  addMenu(event: any): void {
    this.menuService.addMenu(event.newData as Menu).subscribe(
      res => {
        if (res.IsSuccess) {

          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "菜单管理";
          log.Action = "添加菜单";
          log.Message = "菜单" + event.newData.Name + "添加成功";
          this.logService.addLog(log).subscribe();

          event.confirm.resolve(event.newData);
        }
      },
      error => {
        event.confirm.reject();
        console.error(error);
      }
    );
  }

  deleteMenu(event: any) {
    if (window.confirm('确定要删除' + event.data.Name + '吗?')) {
      this.menuService.deleteMenu(event.data.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "菜单管理";
            log.Action = "删除菜单";
            log.Message = "菜单" + event.data.Name + "删除成功";
            this.logService.addLog(log).subscribe();

            event.confirm.resolve();
          }
        },
        error => {
          event.confirm.reject();
          console.error(error);
        }
      );
    }
  }

  editMenu(event: any) {
    this.menuService.updateMenu(event.newData as Menu).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "菜单管理";
          log.Action = "更新菜单";
          log.Message = "菜单" + event.newData.Name + "更新成功";
          this.logService.addLog(log).subscribe();

          event.confirm.resolve(event.newData);
        }
      },
      error => {
        console.error(error);
        event.confirm.reject();
      }
    );
  }
}
