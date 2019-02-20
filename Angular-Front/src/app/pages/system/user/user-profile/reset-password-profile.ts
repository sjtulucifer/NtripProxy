import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../../../@core/data/services/user.service';
import { User } from '../../../../@core/data/entities/user';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  template:
    `
    <nb-card>
    <nb-card-body>
      <form novalidate #userForm="ngForm">
        <div class="form-group row">
          <label for="UserLogin" class="col-sm-3 col-form-label">登陆名</label>
          <div class="col-sm-9">
            <div class="input-group input-group-fill-only">
              <input type="text" class="form-control" placeholder="" name="UserLogin" [(ngModel)]="user.Login" disabled required>
            </div>
          </div>
        </div>
        <div class="form-group row">
          <label for="UserPassword" class="col-sm-3 col-form-label">旧密码</label>
          <div class="col-sm-9">
            <input type="password" class="form-control" placeholder="旧密码" name="UserPassword" [(ngModel)]="user.Password" 
              #UserPassword="ngModel" value="" required>
            <div *ngIf="UserPassword.errors?.required && UserPassword.touched" class="error">
              旧密码必须填写
            </div>
          </div>
        </div> 
        <div class="form-group row">
          <label for="UserNewPassword" class="col-sm-3 col-form-label">新密码</label>
          <div class="col-sm-9">
            <input type="password" class="form-control" placeholder="新密码" name="UserNewPassword" [(ngModel)]="user.NewPassword" 
              #UserNewPassword="ngModel" value="" required>
            <div *ngIf="UserNewPassword.errors?.required && UserNewPassword.touched" class="error">
              新密码必须填写
            </div>
          </div>
        </div>
        <div class="form-group row">
          <label for="UserConfirmPassword" class="col-sm-3 col-form-label">确认密码</label>
          <div class="col-sm-9">
            <input type="password" class="form-control" placeholder="确认密码" name="UserConfirmPassword" [(ngModel)]="user.ConfirmPassword" 
              #UserConfirmPassword="ngModel" value="" required>
            <div *ngIf="UserConfirmPassword.errors?.required && UserConfirmPassword.touched" class="error">
              确认新密码必须填写
            </div>
          </div>
        </div>        
      </form>     
    </nb-card-body>
    <nb-card-footer>
      <button type="button" class="btn btn-primary" (click)="cancle()">取消</button>
      <button type="button" class="btn btn-primary" [disabled]="userForm.invalid" (click)="reset()">重置</button>
    </nb-card-footer>
  </nb-card>
    `,
})
export class ResetPasswordProfileComponent implements OnInit {

  public user: any;

  constructor(
    private userService: UserService,
    private activeModal: NgbActiveModal,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.user = JSON.parse(sessionStorage.getItem("loginUser")) as User;
  }

  cancle(): void {
    this.user.Password = "";
    this.user.NewPassword = "";
    this.user.ConfirmPassword = "";
    this.activeModal.close();
  }

  reset(): void {
    if (this.user.NewPassword === this.user.ConfirmPassword) {
      this.userService.resetUserPasswordProfile(this.user as User).subscribe(
        res => {
          if (res.IsSuccess) {
            this.user.Password = "";
            this.user.NewPassword = "";
            this.user.ConfirmPassword = "";
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "用户管理";
            log.Action = "用户更改个人密码";
            log.Message = "用户" + this.user.Login + "密码更新成功";
            this.logService.addLog(log).subscribe();
            this.activeModal.close();
          }
          else {
            alert("旧密码不正确");
            console.log(res);
          }
        },
        error => {
          console.error(error);
        }
      );
    } else {
      alert("俩次新密码输入不一致");
    }

  }
}
