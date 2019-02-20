import { Component, OnInit } from '@angular/core';
import { AccountSYS } from '../../../../@core/data/entities/account-sys';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountSYSService } from '../../../../@core/data/services/account-sys.service';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-account-sysadd',
  templateUrl: './account-sysadd.component.html',
  styleUrls: ['./account-sysadd.component.scss']
})
export class AccountSYSAddComponent implements OnInit {

  accountSYS: AccountSYS = new AccountSYS();

  constructor(
    private activeModal: NgbActiveModal,
    private accountSYSService: AccountSYSService,
    private logService: LogService
  ) { }

  ngOnInit() {
  }

  onClose(): void {
    this.activeModal.close();
  }

  onSubmit(): void {
    this.accountSYSService.addAccountSYS(this.accountSYS).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "系统账号管理";
          log.Action = "添加系统账号";
          log.Message = "系统账号" + this.accountSYS.Name + "添加成功";
          this.logService.addLog(log).subscribe();
          this.activeModal.close(this.accountSYS);
        }
      },
      error => {
        console.error(error);
      }
    );
  }
}
