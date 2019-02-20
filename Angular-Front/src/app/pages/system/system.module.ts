import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemRoutingModule } from './system-routing.module';
import { SystemComponent } from './system.component';
import { UserListComponent, ResetUserPasswordViewComponent } from './user/user-list/user-list.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { UserAddComponent } from './user/user-add/user-add.component';
import { RoleListComponent, UpdatePermissionViewComponent } from './role/role-list/role-list.component';
import { RoleDetailComponent } from './role/role-detail/role-detail.component';
import { PermissionListComponent, UpdateMenuViewComponent } from './permission/permission-list/permission-list.component';
import { PermissionDetailComponent } from './permission/permission-detail/permission-detail.component';
import { MenuListComponent } from './menu/menu-list/menu-list.component';
import { NbCardModule, NbTabsetModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { FormsModule } from '@angular/forms';
import { RoleSelectComponent } from './user/user-detail/role-select.component';
import { PermissionSelectComponent } from './role/role-detail/permission-select.component';
import { MenuSelectComponent } from './permission/permission-detail/menu-select.component';
import { ResetPasswordComponent } from './user/user-list/reset-password';
import { ResetPasswordProfileComponent } from './user/user-profile/reset-password-profile';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';
import { PermissionComponent } from './permission/permission.component';
import { MenuComponent } from './menu/menu.component';

@NgModule({
  imports: [
    CommonModule,
    SystemRoutingModule,
    NbCardModule,
    NbTabsetModule,
    Ng2SmartTableModule,
    FormsModule
  ],
  declarations: [
    SystemComponent,
    UserComponent, 
    RoleComponent,
    PermissionComponent,
    MenuComponent,
    UserListComponent, 
    ResetUserPasswordViewComponent,
    ResetPasswordComponent,
    ResetPasswordProfileComponent,
    UserDetailComponent, 
    UserProfileComponent, 
    UserAddComponent, 
    RoleListComponent, 
    RoleDetailComponent, 
    PermissionListComponent, 
    PermissionDetailComponent, 
    MenuListComponent, 
    RoleSelectComponent,
    UpdatePermissionViewComponent,
    PermissionSelectComponent,
    UpdateMenuViewComponent,
    MenuSelectComponent,
    UserComponent
  ],
  entryComponents:[
    ResetUserPasswordViewComponent,
    ResetPasswordComponent,
    ResetPasswordProfileComponent,
    UserAddComponent,
    RoleSelectComponent,
    UpdatePermissionViewComponent,
    PermissionSelectComponent,
    UpdateMenuViewComponent,
    MenuSelectComponent,
  ]
})
export class SystemModule { }
