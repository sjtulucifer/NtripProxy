import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';
import { Router } from '@angular/router';
import { PermissionService } from '../../../../@core/data/services/permission.service';
import { Permission } from '../../../../@core/data/entities/permission';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'app-update-menu-view',
  template: `
    <button class="btn btn-primary" (click)="onClick()">更新菜单</button>
  `,
})
export class UpdateMenuViewComponent implements ViewCell, OnInit {

  @Input() value: string | number;
  @Input() rowData: any;

  @Output() save: EventEmitter<any> = new EventEmitter();

  constructor(
    private router: Router,
  ) { }

  ngOnInit() {
  }

  onClick() {
    this.save.emit({ router: this.router, data: this.rowData });
  }
}


@Component({
  selector: 'ntrip-permission-list',
  templateUrl: './permission-list.component.html',
  styleUrls: ['./permission-list.component.scss']
})
export class PermissionListComponent implements OnInit {

  permissionList: Permission[] = new Array<Permission>();
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
      Name: {
        title: '权限名',
        type: 'string',
      },
      Description: {
        title: '权限描述',
        type: 'string',
      },
      button: {
        title: '',
        type: 'custom',
        filter: false,
        renderComponent: UpdateMenuViewComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(row => {
            row.router.navigate(['/pages/system/permission/permissionDetail', row.data.ID]);
          });
        }
      },
    },
  };

  constructor(
    private permissionService: PermissionService,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.getPermissionList();
  }

  getPermissionList(): void {
    this.permissionService.getPermissionList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.permissionList = res.Data as Permission[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  addPermission(event: any): void {
    this.permissionService.addPermission(event.newData as Permission).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "权限管理";
          log.Action = "添加新权限";
          log.Message = "权限" + event.newData.Name + "添加成功";
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

  deletePermission(event: any) {
    if (window.confirm('确定要删除' + event.data.Name + '吗?')) {
      this.permissionService.deletePermission(event.data.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "权限管理";
            log.Action = "删除权限";
            log.Message = "权限" + event.data.Name + "删除成功";
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

  editPermission(event: any) {
    this.permissionService.updatePermission(event.newData).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "权限管理";
          log.Action = "更新权限信息";
          log.Message = "权限" + event.newData.Name + "信息更新成功";
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
