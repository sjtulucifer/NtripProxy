import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProxyComponent } from './proxy.component';
import { CompanyListComponent } from './company/company-list/company-list.component';
import { CompanyDetailComponent } from './company/company-detail/company-detail.component';
import { AccountListComponent } from './account/account-list/account-list.component';
import { AccountDetailComponent } from './account/account-detail/account-detail.component';
import { AccountSYSListComponent } from './accountSYS/account-syslist/account-syslist.component';
import { AccountSYSDetailComponent } from './accountSYS/account-sysdetail/account-sysdetail.component';
import { AuthGuardDirective } from '../../auth/auth-guard.directive';
import { CompanyAccountComponent } from './company/company-account/company-account.component';

const routes: Routes = [{
  path: '',
  component: ProxyComponent,
  children: [{
    path: 'company',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'companyList',
        component: CompanyListComponent,
      }, {
        path: 'companyDetail/:id',
        component: CompanyDetailComponent,
      }, {
        path: 'companyAccount/:id',
        component: CompanyAccountComponent,
      },{
        path: '',
        redirectTo: 'companyList',
        pathMatch: 'full',
      },]
  }, {
    path: 'account',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'accountList',
        component: AccountListComponent,
      }, {
        path: 'accountDetail/:id',
        component: AccountDetailComponent,
      }, {
        path: '',
        redirectTo: 'accountList',
        pathMatch: 'full',
      },]
  }, {
    path: 'accountSYS',
    canActivateChild: [AuthGuardDirective],
    children: [
      {
        path: 'accountSYSList',
        component: AccountSYSListComponent,
      }, {
        path: 'accountSYSDetail/:id',
        component: AccountSYSDetailComponent,
      }, {
        path: '',
        redirectTo: 'accountSYSList',
        pathMatch: 'full',
      },]
  },],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProxyRoutingModule { }
