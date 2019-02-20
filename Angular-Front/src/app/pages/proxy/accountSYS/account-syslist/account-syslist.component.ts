import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AccountSYS } from '../../../../@core/data/entities/account-sys';
import { AccountSYSService } from '../../../../@core/data/services/account-sys.service';
import { BoolRenderComponent } from './bool-render.component';
import { DateRenderComponent } from './date-render.component';
import { ViewCell } from 'ng2-smart-table';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountSYSAddComponent } from '../account-sysadd/account-sysadd.component';
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
export class DelayExprieSYSViewComponent implements ViewCell, OnInit {

  @Input() value: string | number;
  @Input() rowData: any;

  @Output() save: EventEmitter<any> = new EventEmitter();

  constructor(
    private accountSYSService: AccountSYSService,
    private router: Router,
    private logService: LogService
  ) { }

  ngOnInit() {
  }

  delayFunction(days: number): void {
    let accountSYS: AccountSYS = this.rowData as AccountSYS;
    let expireDate: Date = new Date(accountSYS.Expire);
    let delayDate: Date = new Date(expireDate.setDate(expireDate.getDate() + days));
    accountSYS.Expire = new Date(delayDate.getFullYear(), delayDate.getMonth(), delayDate.getDate());
    if (window.confirm(accountSYS.Name + '的过期时间将延期' + days + '天，确认延期吗？')) {
      this.save.emit({ service: this.accountSYSService, data: this.rowData, router: this.router });
      
      //记录日志
      let log: Log = new Log();
      log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
      //从UTC时间转换成北京时间
      log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
      log.Module = "系统账号管理";
      log.Action = "系统账号延期";
      log.Message = "系统账号" + accountSYS.Name + "延期" + days + "天，过期时间延长至" + accountSYS.Expire;
      this.logService.addLog(log).subscribe();
    }
    else {
      this.accountSYSService.getAccountSYSByID(this.rowData.ID).subscribe(
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
  selector: 'ntrip-account-syslist',
  templateUrl: './account-syslist.component.html',
  styleUrls: ['./account-syslist.component.scss']
})
export class AccountSYSListComponent implements OnInit {

  accountSYSList: AccountSYS[] = new Array<AccountSYS>();
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
        title: '系统账户名',
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
      Age: {
        title: '登录次数',
        type: 'string',
        filter: false,
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
      button: {
        title: '延期操作',
        type: 'custom',
        filter: false,
        renderComponent: DelayExprieSYSViewComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(
            row => {
              row.service.updateAccountSYS(row.data).subscribe(
                res => {
                  if (res.IsSuccess) {
                    row.router.navigate(['/pages/proxy/accountSYS/accountSYSDetail', row.data.ID]);
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
    private accountSYSService: AccountSYSService,
    private router: Router,
    private modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.getAccountSYSList();
  }

  getAccountSYSList(): void {
    this.accountSYSService.getAccountSYSList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.accountSYSList = res.Data as AccountSYS[];
        }
      },
      error => {
        console.error(error);
      }
    )
  }

  createAccountSYS(): void {
    const modalRef = this.modalService.open(AccountSYSAddComponent, {
      size: 'lg',
      backdrop: 'static',
      container: 'nb-layout',
    });
    modalRef.result.then((result) => {
      if (result) {
        this.accountSYSService.getAccountSYSList().subscribe(
          res => {
            if (res.IsSuccess) {
              this.accountSYSList = res.Data as AccountSYS[];
            }
          },
          error => {
            console.log(error);
          }
        );
      }
    });
  }

  editAccountSYS(event: any): void {
    this.router.navigate(['/pages/proxy/accountSYS/accountSYSDetail', event.data.ID]);
  }

}
