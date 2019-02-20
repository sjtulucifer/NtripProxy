import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountService } from '../../../../@core/data/services/account.service';
import { Account } from '../../../../@core/data/entities/account';
import { CompanyService } from '../../../../@core/data/services/company.service';
import { Company } from '../../../../@core/data/entities/company';
import { Logs } from 'selenium-webdriver';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-account-add',
  templateUrl: './account-add.component.html',
  styleUrls: ['./account-add.component.scss']
})
export class AccountAddComponent implements OnInit {

  account: Account = new Account();
  companyList: Array<Company> = new Array();
  accountCompany: Company = new Company();

  constructor(
    private activeModal: NgbActiveModal,
    private accountService: AccountService,
    private companyService: CompanyService,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.companyService.getCompanyList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.companyList = res.Data as Company[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  onClose(): void {
    this.activeModal.close();
  }

  onSubmit(): void {
    //未使用session信息的测试用户，使用session用户以后用当前登录用户替代
    this.account.AddUser = JSON.parse(sessionStorage.getItem("loginUser"));
    this.account.Company = this.accountCompany;
    this.accountService.addAccount(this.account).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "账号管理";
          log.Action = "添加账号";
          log.Message = "账号" + this.account.Name + "添加成功";
          this.logService.addLog(log).subscribe();

          this.activeModal.close(this.account);
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
        }
        else {
          console.log(res);
        }
      },
      error => {
        console.log(error);
      }
    )
  }
}
