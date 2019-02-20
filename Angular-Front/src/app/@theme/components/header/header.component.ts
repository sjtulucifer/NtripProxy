import { Component, Input, OnInit } from '@angular/core';

import { NbMenuService, NbSidebarService } from '@nebular/theme';
import { UserService } from '../../../@core/data/users.service';
import { AnalyticsService } from '../../../@core/utils/analytics.service';
import { LayoutService } from '../../../@core/data/layout.service';
import { filter, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { User } from '../../../@core/data/entities/user';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ResetPasswordProfileComponent } from '../../../pages/system/user/user-profile/reset-password-profile';

@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {

  @Input() position = 'normal';

  user: any;

  userMenu = [{ title: '个人信息' }, { title: '重置密码' }, { title: '注销' }];

  constructor(private sidebarService: NbSidebarService,
              private nbMenuService: NbMenuService,
              private modal: NgbModal,
              // private userService: UserService,
              //private analyticsService: AnalyticsService,
              private layoutService: LayoutService,
              private router: Router) {
  }

  ngOnInit() {
    this.user = JSON.parse(sessionStorage.getItem('loginUser')) as User;
    this.nbMenuService.onItemClick()
      .pipe(
        filter(({ tag }) => tag === 'user-context-menu'),
        map(({ item: { title } }) => title),
    ).subscribe(
        obj => {
          if (obj === '个人信息') {
            this.router.navigate(['/pages/system/userProfile', this.user.ID]);
          } else if (obj === '重置密码') {
            const modalRef = this.modal.open(ResetPasswordProfileComponent, {
              size: 'lg',
              backdrop: 'static',
              container: 'nb-layout',
            });
          } else if (obj === '注销') {
            sessionStorage.clear();
            this.router.navigate(['/auth']);  
            //必须刷新不然无法再次登录
            window.location.reload();          
          }
        },
        error => {
          console.error(error);
        }
      );
    /*
    this.userService.getUsers()
      .subscribe((users: any) => this.user = users.nick);
    */
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  toggleSettings(): boolean {
    this.sidebarService.toggle(false, 'settings-sidebar');

    return false;
  }
  /*
  goToHome() {
    this.menuService.navigateHome();
  }

  startSearch() {
    this.analyticsService.trackEvent('startSearch');
  }
  */
}
