import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { NbLoginComponent, NbAuthService } from '@nebular/auth';
import { Router } from '@angular/router';
import { AccountService } from '../../@core/data/services/account.service';
import { AccountDetail } from '../../@core/data/entity/account-detail';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends NbLoginComponent implements OnInit {

  account: AccountDetail = new AccountDetail();

  constructor(
    public accountService: AccountService,
    public service: NbAuthService,
    public cd: ChangeDetectorRef,
    public router: Router
  ) {
    super(service, null, cd, router);
  }

  ngOnInit() {
  }

  login(): void {
    this.accountService.accountLogin(this.account).subscribe(
      res => {
        let account: AccountDetail = res as AccountDetail
        if (new Date(account.AccountExpire).valueOf() < Date.now()) {
          alert("账号已过期");
        } else if (account.AccountIsLocked) {
          alert("账号已锁定");
        } else {
          sessionStorage.setItem('loginAccount', JSON.stringify(account));
          this.router.navigate(['/pages']);
        }
      },
      error => {
        alert("账号名或密码错误");
        console.error(error);
      }
    );
  }

}
