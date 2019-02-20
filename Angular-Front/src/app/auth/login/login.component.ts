import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { NbLoginComponent, NbAuthService } from '@nebular/auth';
import { User } from '../../@core/data/entities/user';
import { Router } from '@angular/router';
import { UserService } from '../../@core/data/services/user.service';

@Component({
  selector: 'ntrip-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends NbLoginComponent implements OnInit {

  user: User = new User();

  constructor(
    private userService: UserService,
    public service: NbAuthService,
    public cd: ChangeDetectorRef,
    public router: Router
  ) {
    super(service, null, cd, router);
  }

  ngOnInit() {
  }

  login(): void {
    this.userService.userLogin(this.user).subscribe(
      res => {
        if (res.IsSuccess) {
          sessionStorage.setItem('loginUser', JSON.stringify(res.Data as User));
          this.router.navigate(['/pages']);
        }
        else {
          alert("用户名或密码错误");
        }
      },
      error => {
        console.error(error);
      }
    );
  }
}
