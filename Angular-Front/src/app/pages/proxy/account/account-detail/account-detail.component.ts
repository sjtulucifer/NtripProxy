import { Component, OnInit } from '@angular/core';
import { Company } from '../../../../@core/data/entities/company';
import { Account } from '../../../../@core/data/entities/account';
import { AccountService } from '../../../../@core/data/services/account.service';
import { CompanyService } from '../../../../@core/data/services/company.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.scss']
})
export class AccountDetailComponent implements OnInit {

  //账户信息
  account: Account = new Account();
  //账户公司
  accountCompany: Company = new Company();
  //所有公司信息
  companies: Company[] = new Array();

  constructor(
    private accountService: AccountService,
    private companyService: CompanyService,
    private route: ActivatedRoute,
    private logService: LogService
  ) { }

  ngOnInit() {
    let userID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        userID = params.get('id');
      }
    );
    //获取账户信息
    this.accountService.getAccountByID(userID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.account = res.Data as Account;
          this.accountCompany = this.account.Company;
        }
      },
      error => {
        console.log(error);
      }
    );
    //获取所有公司信息
    this.companyService.getCompanyList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.companies = res.Data as Company[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  saveAccount(): void {
    this.accountService.updateAccount(this.account).subscribe(
      res => {
        if (res.IsSuccess) {

          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "账号管理";
          log.Action = "更新账号信息";
          log.Message = "账号" + this.account.Name + "信息更新成功";
          this.logService.addLog(log).subscribe();

          this.accountService.getAccountByID(this.account.ID).subscribe(
            next => {
              if (next.IsSuccess) {
                this.account = next.Data as Account;
                alert(this.account.Name + "保存成功");
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

  accountCompanyChange(event: any): void {
    this.companyService.getCompanyByID(event).subscribe(
      res => {
        if (res.IsSuccess) {
          this.accountCompany = res.Data as Company;
          this.accountService.updateAccountCompany({ account: this.account, company: this.accountCompany }).subscribe(
            res => {
              if (res.IsSuccess) {
                //记录日志
                let log: Log = new Log();
                log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
                //从UTC时间转换成北京时间
                log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
                log.Module = "账号管理";
                log.Action = "更新账号公司信息";
                log.Message = "账号" + this.account.Name + "所属公司更新为" + this.accountCompany.Name;
                this.logService.addLog(log).subscribe();

                console.log(res.Data);
              }
              else {
                console.log(res);
              }
            },
            error => {
              console.log(error);
            }
          );
        }
        else {
          console.log(res);
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  // 处理注册时间字段
  public registerChange(date: string) {
    this.account.Register = new Date(date);
  }

  // 处理过期时间字段
  public expireChange(date: string) {
    this.account.Expire = new Date(date);
  }
}
