import { Component, OnInit } from '@angular/core';
import { User } from '../../../../@core/data/entities/user';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { UserService } from '../../../../@core/data/services/user.service';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  // 用户信息
  user: User = new User();

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private logService: LogService
  ) { }

  ngOnInit() {
    let userID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        userID = params.get('id');
      }
    );
    //获取用户角色信息
    this.userService.getUserByID(userID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.user = res.Data as User;
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  saveUser(): void {
    this.userService.updateUser(this.user).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "用户管理";
          log.Action = "用户更改个人信息";
          log.Message = "用户" + this.user.Login + "更新个人信息成功";
          this.logService.addLog(log).subscribe();

          this.userService.getUserByID(this.user.ID).subscribe(
            next => {
              if (next.IsSuccess) {
                this.user = next.Data as User;
                alert(this.user.Login + "保存成功");
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
