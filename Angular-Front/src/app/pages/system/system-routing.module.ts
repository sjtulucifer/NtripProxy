import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SystemComponent } from './system.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { RoleListComponent } from './role/role-list/role-list.component';
import { RoleDetailComponent } from './role/role-detail/role-detail.component';
import { PermissionListComponent } from './permission/permission-list/permission-list.component';
import { PermissionDetailComponent } from './permission/permission-detail/permission-detail.component';
import { MenuListComponent } from './menu/menu-list/menu-list.component';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { PermissionComponent } from './permission/permission.component';
import { AuthGuardDirective } from '../../auth/auth-guard.directive';

const routes: Routes = [{
  path: '',
  component: SystemComponent,
  children: [{
    path: 'user',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'userList',
        component: UserListComponent,
      }, {
        path: 'userDetail/:id',
        component: UserDetailComponent,
      }, {
        path: '',
        redirectTo: 'userList',
        pathMatch: 'full',
      },]
  }, {
    path: 'role',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'roleList',
        component: RoleListComponent,
      }, {
        path: 'roleDetail/:id',
        component: RoleDetailComponent,
      }, {
        path: '',
        redirectTo: 'roleList',
        pathMatch: 'full',
      },]
  }, {
    path: 'permission',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'permissionList',
        component: PermissionListComponent,
      }, {
        path: 'permissionDetail/:id',
        component: PermissionDetailComponent,
      }, {
        path: '',
        redirectTo: 'permissionList',
        pathMatch: 'full',
      },]
  }, {
    path: 'menu',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'menuList',
        component: MenuListComponent,
      }, {
        path: '',
        redirectTo: 'menuList',
        pathMatch: 'full',
      },]
  }, {
    path: 'userProfile/:id',
    component: UserProfileComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
