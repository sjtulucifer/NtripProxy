import { Component, OnInit } from '@angular/core';
import { Role } from '../../../../@core/data/entities/role';
import { LocalDataSource } from 'ng2-smart-table';
import { RoleService } from '../../../../@core/data/services/role.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PermissionSelectComponent } from './permission-select.component';
import { Permission } from '../../../../@core/data/entities/permission';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrls: ['./role-detail.component.scss']
})
export class RoleDetailComponent implements OnInit {

  role: Role = new Role();
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
      Name: {
        title: '权限名',
        type: 'string',
      },
    }
  };

  constructor(
    private roleService: RoleService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private logService: LogService
  ) { }

  ngOnInit() {
    let roleID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        roleID = params.get('id');
      }
    );

    this.roleService.getRoleByID(roleID).subscribe(
      res => {
        this.role = res.Data as Role;
        this.source.load(this.role.Permissions);
      },
      error => {
        console.log(error);
      }
    );
  }

  updatePermissions(): void {
    const modalRef = this.modalService.open(PermissionSelectComponent, {
      size: 'sm',
      backdrop: 'static',
      container: 'nb-layout',
    });
    // 模态窗口返回数据给主页面
    modalRef.result.then((result) => {
      if (result) {
        const newPermissions: Permission[] = result as Permission[];
        this.roleService.updateRolePermissions({ role: this.role, permissions: newPermissions }).subscribe(
          res => {
            if (res.IsSuccess) {
              //记录日志
              let log: Log = new Log();
              log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
              //从UTC时间转换成北京时间
              log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
              log.Module = "角色管理";
              log.Action = "更新角色权限";
              log.Message = "角色" + this.role.Name + "权限更新为:";
              for (var j = 0, len = newPermissions.length; j < len; j++) {
                log.Message += newPermissions[j].Name + "+";
              }
              this.logService.addLog(log).subscribe();
              this.roleService.getRoleByID(this.role.ID).subscribe(
                next => {
                  this.role = next.Data as Role;
                  this.source.load(this.role.Permissions);
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

  deletePermission(event: any) {
    if (window.confirm('确定要删除权限' + event.data.Name + '吗?')) {
      const index = this.role.Permissions.indexOf(event.data);
      this.role.Permissions.splice(index, 1);
      this.roleService.updateRolePermissions({ role: this.role, permissions: this.role.Permissions }).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "角色管理";
            log.Action = "删除角色权限";
            log.Message = "删除角色" + this.role.Name + "权限" + event.data.Name;            
            this.logService.addLog(log).subscribe();
            
            this.roleService.getRoleByID(this.role.ID).subscribe(
              next => {
                if (next.IsSuccess) {
                  this.role = next.Data as Role;
                  this.source.load(this.role.Permissions);
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
