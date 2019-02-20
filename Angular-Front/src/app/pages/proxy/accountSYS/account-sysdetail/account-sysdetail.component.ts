import { Component, OnInit } from '@angular/core';
import { AccountSYS } from '../../../../@core/data/entities/account-sys';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { AccountSYSService } from '../../../../@core/data/services/account-sys.service';
import { Log } from '../../../../@core/data/entities/log';
import { LogService } from '../../../../@core/data/services/log.service';

@Component({
  selector: 'ntrip-account-sysdetail',
  templateUrl: './account-sysdetail.component.html',
  styleUrls: ['./account-sysdetail.component.scss']
})
export class AccountSYSDetailComponent implements OnInit {

  accountSYS: AccountSYS = new AccountSYS();

  constructor(
    private route: ActivatedRoute,
    private accountSYSService: AccountSYSService,
    private logService: LogService
  ) { }

  ngOnInit() {
    let accountSYSID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        accountSYSID = params.get('id');
      }
    );
     //获取系统账户信息
     this.accountSYSService.getAccountSYSByID(accountSYSID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.accountSYS = res.Data as AccountSYS;
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  //处理注册时间字段
  public registerChange(date: string) {
    this.accountSYS.Register = new Date(date);
  }

  //处理过期时间字段
  public expireChange(date: string) {
    this.accountSYS.Expire = new Date(date);
  }

  save(): void {
    this.accountSYSService.updateAccountSYS(this.accountSYS).subscribe(
      res => {
        if (res.IsSuccess) {
           //记录日志
           let log: Log = new Log();
           log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
           //从UTC时间转换成北京时间
           log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
           log.Module = "系统账号管理";
           log.Action = "更新系统账号信息";
           log.Message = "系统账号" + this.accountSYS.Name + "信息更新成功";
           this.logService.addLog(log).subscribe();

          this.accountSYSService.getAccountSYSByID(this.accountSYS.ID).subscribe(
            next => {
              if (next.IsSuccess) {
                this.accountSYS = next.Data as AccountSYS;
                alert(this.accountSYS.Name + "保存成功");
              }
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
}
