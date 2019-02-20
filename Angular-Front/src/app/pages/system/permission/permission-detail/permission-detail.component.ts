import { Component, OnInit } from '@angular/core';
import { Permission } from '../../../../@core/data/entities/permission';
import { LocalDataSource } from 'ng2-smart-table';
import { PermissionService } from '../../../../@core/data/services/permission.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MenuSelectComponent } from './menu-select.component';
import { Menu } from '../../../../@core/data/entities/menu';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-permission-detail',
  templateUrl: './permission-detail.component.html',
  styleUrls: ['./permission-detail.component.scss']
})
export class PermissionDetailComponent implements OnInit {

  permission: Permission = new Permission();
  source: LocalDataSource = new LocalDataSource();
  settings = {
    noDataMessage: '无数据',
    mode: 'external',
    actions: {
      columnTitle: '操作',
      edit: false
    },
    add: {
      addButtonContent: '<i class="nb-edit"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
    },
    columns: {
      Catagory: {
        title: '类别',
        type: 'string',
      },
      Name: {
        title: '菜单名',
        type: 'string',
      },
    }
  };

  constructor(
    private permissionService: PermissionService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private logService: LogService
  ) { }

  ngOnInit() {
    let permissionID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        permissionID = params.get('id');
      }
    );

    this.permissionService.getPermissionByID(permissionID).subscribe(
      res => {
        this.permission = res.Data as Permission;
        this.source.load(this.permission.Menus);
      },
      error => {
        console.log(error);
      }
    );
  }

  updateMenus(): void {
    const modalRef = this.modalService.open(MenuSelectComponent, {
      size: 'sm',
      backdrop: 'static',
      container: 'nb-layout',
    });
    // 模态窗口返回数据给主页面
    modalRef.result.then((result) => {
      if (result) {
        const newMenus: Menu[] = result as Menu[];
        this.permissionService.updatePermissionMenus({ permission: this.permission, menus: newMenus }).subscribe(
          res => {
            if (res.IsSuccess) {
              //记录日志
              let log: Log = new Log();
              log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
              //从UTC时间转换成北京时间
              log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
              log.Module = "权限管理";
              log.Action = "更新权限菜单";
              log.Message = "权限" + this.permission.Name + "菜单更新为:";
              for (var j = 0, len = newMenus.length; j < len; j++) {
                log.Message += newMenus[j].Name + "+";
              }
              this.logService.addLog(log).subscribe();
              this.permissionService.getPermissionByID(this.permission.ID).subscribe(
                next => {
                  this.permission = next.Data as Permission;
                  this.source.load(this.permission.Menus);
                },
                error => {
                  console.error(error);
                }
              );
            }
          },
          error => {
            console.error(error);
          }
        );
      }
    });
  }

  deleteMenu(event: any) {
    if (window.confirm('确定要删除' + event.data.Name + '吗?')) {
      const index = this.permission.Menus.indexOf(event.data);
      this.permission.Menus.splice(index, 1);
      this.permissionService.updatePermissionMenus({ permission: this.permission, menus: this.permission.Menus }).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "权限管理";
            log.Action = "删除权限菜单";
            log.Message = "删除权限" + this.permission.Name + "菜单" + event.data.Name;
            this.logService.addLog(log).subscribe();
            this.permissionService.getPermissionByID(this.permission.ID).subscribe(
              next => {
                if (next.IsSuccess) {
                  this.permission = next.Data as Permission;
                  this.source.load(this.permission.Menus);
                }
              }
            );
          }
        },
        error => {
          console.error(error);
        }
      );
    }
  }
}
