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
          <label for="UserPassword" class="col-sm-3 col-form-label">新密码</label>
          <div class="col-sm-9">
            <input type="password" class="form-control" placeholder="新密码" name="UserNewPassword" [(ngModel)]="user.Password" 
              #UserNewPassword="ngModel" value="" required>
            <div *ngIf="UserNewPassword.errors?.required && UserNewPassword.touched" class="error">
              密码必须填写
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
export class ResetPasswordComponent implements OnInit{

  public user: any;

  constructor(
    private userService: UserService,
    private activeModal: NgbActiveModal,
    private logService: LogService
  ) { }

  ngOnInit() {
  }

  cancle(): void {
    this.user.Password = "";
    this.activeModal.close();
  }

  reset(): void {
    this.userService.resetUserPassword(this.user as User).subscribe(
      res => {
        if(res.IsSuccess){
          this.user.Password = "";
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "用户管理";
          log.Action = "管理员重置用户密码";
          log.Message = "用户" + this.user.Login + "密码被管理员重置";
          this.logService.addLog(log).subscribe();
          this.activeModal.close();
        }
        else{
          console.log(res);
        }
      },
      error => {
        console.error(error);
      }
    );
  }
}
