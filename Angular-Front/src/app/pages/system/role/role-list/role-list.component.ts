import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';
import { Router } from '@angular/router';
import { Role } from '../../../../@core/data/entities/role';
import { RoleService } from '../../../../@core/data/services/role.service';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-update-pemission-view',
  template: `
    <button class="btn btn-primary" (click)="onClick()">更新权限</button>
  `,
})
export class UpdatePermissionViewComponent implements ViewCell, OnInit {

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
  selector: 'ntrip-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss']
})
export class RoleListComponent implements OnInit {

  roleList: Role[] = new Array<Role>();
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
        title: '角色名',
        type: 'string',
      },
      Description: {
        title: '角色描述',
        type: 'string',
      },
      button: {
        title: '',
        type: 'custom',
        filter: false,
        renderComponent: UpdatePermissionViewComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(row => {
            row.router.navigate(['/pages/system/role/roleDetail', row.data.ID]);
          });
        }
      },
    },
  };

  constructor(
    private roleService: RoleService,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.getRoleList();
  }

  getRoleList(): void {
    this.roleService.getRoleList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.roleList = res.Data as Role[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  addRole(event: any): void {
    this.roleService.addRole(event.newData as Role).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "角色管理";
          log.Action = "添加新角色";
          log.Message = "角色" + event.newData.Name + "添加成功";
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

  deleteRole(event: any) {
    if (window.confirm('确定要删除' + event.data.Name + '吗?')) {
      this.roleService.deleteRole(event.data.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "角色管理";
            log.Action = "删除角色";
            log.Message = "角色" + event.data.Name + "删除成功";
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

  editRole(event: any) {
    this.roleService.updateRole(event.newData).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "角色管理";
          log.Action = "更新角色信息";
          log.Message = "角色" + event.newData.Name + "信息更新成功";
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
