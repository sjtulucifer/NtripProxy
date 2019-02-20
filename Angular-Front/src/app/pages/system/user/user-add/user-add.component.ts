import { Component, OnInit } from '@angular/core';
import { User } from '../../../../@core/data/entities/user';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../../../@core/data/services/user.service';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';
import { Company } from '../../../../@core/data/entities/company';
import { CompanyService } from '../../../../@core/data/services/company.service';

@Component({
  selector: 'ntrip-user-add',
  templateUrl: './user-add.component.html',
  styleUrls: ['./user-add.component.scss']
})
export class UserAddComponent implements OnInit {

  // 用户信息
  user: User = new User();

  constructor(
    private activeModal: NgbActiveModal,
    private userService: UserService,
    private logService: LogService,
  ) { }

  ngOnInit() {
  }

  onClose(): void {
    this.activeModal.close();
  }

  onSubmit(): void {
    this.userService.addUser(this.user).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "用户管理";
          log.Action = "添加新用户";
          log.Message = "用户" + this.user.Login + "添加成功";
          this.logService.addLog(log).subscribe();

          this.activeModal.close(this.user);
        }
      },
      error => {
        console.error(error);
      }
    );
  }
}
