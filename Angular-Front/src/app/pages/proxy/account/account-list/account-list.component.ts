import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Account } from '../../../../@core/data/entities/account';
import { ViewCell } from 'ng2-smart-table';
import { AccountService } from '../../../../@core/data/services/account.service';
import { Router } from '@angular/router';
import { DateRenderComponent } from '../../accountSYS/account-syslist/date-render.component';
import { BoolRenderComponent } from '../../accountSYS/account-syslist/bool-render.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountAddComponent } from '../account-add/account-add.component';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-delay-exprie-view',
  template: `
    <div ngbRadioGroup
          class="btn-group btn-group-toggle btn-outline-toggle-group btn-group-full-width btn-toggle-radio-group">
      <label ngbButtonLabel class="btn btn-outline-primary">
        <input ngbButton type="radio" value="left" (click)="delayFunction(7)"> 周
      </label>
      <label ngbButtonLabel class="btn btn-outline-primary">
        <input ngbButton type="radio" value="middle" (click)="delayFunction(30)"> 月
      </label>
      <label ngbButtonLabel class="btn btn-outline-primary">
        <input ngbButton type="radio" value="right" (click)="delayFunction(365)"> 年
      </label>
    </div>
  `,
})
export class DelayExprieViewComponent implements ViewCell, OnInit {

  @Input() value: string | number;
  @Input() rowData: any;

  @Output() save: EventEmitter<any> = new EventEmitter();

  constructor(
    private accountService: AccountService,
    private router: Router,
    private logService: LogService
  ) { }

  ngOnInit() {
  }

  delayFunction(days: number): void {
    let account: Account = this.rowData as Account;
    let expireDate: Date = new Date(account.Expire);
    let delayDate: Date = new Date(expireDate.setDate(expireDate.getDate() + days));
    account.Expire = new Date(delayDate.getFullYear(), delayDate.getMonth(), delayDate.getDate());
    if (window.confirm(account.Name + '的过期时间将延期' + days + '天，确认延期吗？')) {
      this.save.emit({ service: this.accountService, data: account, router: this.router });
      //记录日志
      let log: Log = new Log();
      log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
      //从UTC时间转换成北京时间
      log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
      log.Module = "账号管理";
      log.Action = "账号延期";
      log.Message = "账号" + account.Name + "延期" + days + "天，过期时间延长至" + account.Expire;
      this.logService.addLog(log).subscribe();
    }
    else {
      this.accountService.getAccountByID(this.rowData.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            this.rowData = res.Data;
          }
        },
        error => {
          console.log(error);
        }
      )
    }
  }
}

@Component({
  selector: 'ntrip-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.scss']
})
export class AccountListComponent implements OnInit {

  accountList: Account[] = new Array<Account>();
  settings = {
    mode: 'external',
    noDataMessage: '无数据',
    actions: {
      columnTitle: '操作',
      delete: false,
    },
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    columns: {
      Name: {
        title: '账户名',
        type: 'string',
      },
      Password: {
        title: '密码',
        type: 'string',
        filter: false,
      },

      Register: {
        title: '注册时间',
        type: 'custom',
        filter: false,
        renderComponent: DateRenderComponent
      },
      Expire: {
        title: '过期时间',
        type: 'custom',
        filter: false,
        renderComponent: DateRenderComponent
      },
      LastLogin: {
        title: '上次登录时间',
        type: 'html',
        filter: false,
      },
      PasswordOvertime: {
        title: '密码输错时间',
        type: 'html',
        filter: false,
      },
      PasswordOvercount: {
        title: '密码输错次数',
        type: 'string',
        filter: false,
      },
      IsLocked: {
        title: '是否被锁定',
        filter: {
          type: 'checkbox',
          config: {
            true: 'true',
            false: 'false',
            resetText: '清除',
          },
        },
        editable: false,
        type: 'custom',
        renderComponent: BoolRenderComponent
      },
      IsOnline: {
        title: '是否在线',
        filter: {
          type: 'checkbox',
          config: {
            true: 'true',
            false: 'false',
            resetText: '清除',
          },
        },
        editable: false,
        type: 'custom',
        renderComponent: BoolRenderComponent
      },      
      button: {
        title: '延期操作',
        type: 'custom',
        filter: false,
        renderComponent: DelayExprieViewComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(
            row => {
              row.service.updateAccount(row.data).subscribe(
                res => {
                  if (res.IsSuccess) {
                    row.router.navigate(['/pages/proxy/account/accountDetail', row.data.ID]);
                  }
                }
              );
            },
            error => {
              console.error(error);
            }
          );
        }
      },
    },
  };

  constructor(
    private accountService: AccountService,
    private router: Router,
    private modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.accountService.getAccountList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.accountList = res.Data as Account[];
        }
      },
      error => {
        console.error(error);
      }
    )
  }

  createAccount(): void {
    const modalRef = this.modalService.open(AccountAddComponent, {
      size: 'lg',
      backdrop: 'static',
      container: 'nb-layout',
    });
    modalRef.result.then((result) => {
      if (result) {
        this.accountService.getAccountList().subscribe(
          res => {
            if (res.IsSuccess) {
              this.accountList = res.Data as Account[];
            }
          },
          error => {
            console.log(error);
          }
        );
      }
    });
  }

  editAccount(event: any): void {
    this.router.navigate(['/pages/proxy/account/accountDetail', event.data.ID]);
  }
}
