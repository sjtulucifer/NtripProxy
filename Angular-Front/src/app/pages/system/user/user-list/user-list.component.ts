import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../../../@core/data/entities/user';
import { UserService } from '../../../../@core/data/services/user.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserAddComponent } from '../user-add/user-add.component';
import { Router } from '@angular/router';
import { ViewCell } from 'ng2-smart-table';
import { ResetPasswordComponent } from './reset-password';
import { Log } from '../../../../@core/data/entities/log';
import { LogService } from '../../../../@core/data/services/log.service';

@Component({
  template: `
    <button class="btn btn-primary" (click)="onClick()">重置密码</button>
  `,
})
export class ResetUserPasswordViewComponent implements ViewCell, OnInit {

  @Input() value: string | number;
  @Input() rowData: any;

  @Output() save: EventEmitter<any> = new EventEmitter();

  constructor(
    private modalService: NgbModal,
  ) { }

  ngOnInit() {
  }

  onClick() {
    this.save.emit({ modal: this.modalService, data: this.rowData });
  }
}

@Component({
  selector: 'ntrip-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  userList: User[] = new Array();
  settings = {
    mode: 'external',
    noDataMessage: '无数据',
    actions: {
      columnTitle: '操作',
    },
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
    },
    columns: {
      Login: {
        title: '登陆名',
        type: 'string',
      },
      Name: {
        title: '用户名',
        type: 'string',
      },
      Phone: {
        title: '电话',
        type: 'string',
      },
      Email: {
        title: '邮箱',
        type: 'string',
      },
      button: {
        title: '',
        type: 'custom',
        filter: false,
        renderComponent: ResetUserPasswordViewComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(row => {
            const modalRef = row.modal.open(ResetPasswordComponent, {
              size: 'lg',
              backdrop: 'static',
              container: 'nb-layout',
            });
            //传值给模态框属性名为user
            modalRef.componentInstance.user = row.data;
          });
        }
      },
    },
  };

  constructor(
    private userService: UserService,
    private modalService: NgbModal,
    private router: Router,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.getAllUsers();
  }

  getAllUsers(): void {
    this.userService.getUserList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.userList = res.Data as User[];
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  createUser(): void {
    const modalRef = this.modalService.open(UserAddComponent, {
      size: 'lg',
      backdrop: 'static',
      container: 'nb-layout',
    });
    modalRef.result.then((result) => {
      if (result) {
        this.userService.getUserList().subscribe(
          res => {
            if (res.IsSuccess) {
              this.userList = res.Data as User[];
            }
          },
          error => {
            console.log(error);
          }
        );
      }
    });
  }

  deleteUser(event: any): void {
    if (window.confirm('确定要删除' + event.data.Login + '吗?')) {
      // 逻辑删除数据库数据
      this.userService.softDeleteUser(event.data.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "用户管理";
            log.Action = "删除用户";
            log.Message = "用户" + event.data.Login + "删除成功";
            this.logService.addLog(log).subscribe();
            
            // 刷新列表
            this.userService.getUserList().subscribe(
              next => {
                if (next.IsSuccess) {
                  this.userList = next.Data as User[];
                }
              },
              error => {
                console.log(error);
              }
            );
          }
        },
        error => {
          console.log(error);
        }
      );
    }
  }

  editUser(event: any): void {
    this.router.navigate(['/pages/system/user/userDetail', event.data.ID]);
  }

}
