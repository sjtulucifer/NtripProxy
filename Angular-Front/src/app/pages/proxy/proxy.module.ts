import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProxyRoutingModule } from './proxy-routing.module';
import { ProxyComponent } from './proxy.component';
import { AccountListComponent, DelayExprieViewComponent } from './account/account-list/account-list.component';
import { AccountDetailComponent } from './account/account-detail/account-detail.component';
import { AccountAddComponent } from './account/account-add/account-add.component';
import { CompanyListComponent, ShowCompanyAccountViewComponent } from './company/company-list/company-list.component';
import { CompanyDetailComponent } from './company/company-detail/company-detail.component';
import { AccountSYSListComponent, DelayExprieSYSViewComponent } from './accountSYS/account-syslist/account-syslist.component';
import { NbCardModule, NbTabsetModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { CompanyAddComponent } from './company/company-add/company-add.component';
import { FormsModule } from '@angular/forms';
import { BoolRenderComponent } from './accountSYS/account-syslist/bool-render.component';
import { DateRenderComponent } from './accountSYS/account-syslist/date-render.component';
import { AccountSYSDetailComponent } from './accountSYS/account-sysdetail/account-sysdetail.component';
import { AccountSYSAddComponent } from './accountSYS/account-sysadd/account-sysadd.component';
import { AccountComponent } from './account/account.component';
import { AccountSYSComponent } from './accountSYS/accountsys.component';
import { CompanyComponent } from './company/company.component';
import { ThemeModule } from '../../@theme/theme.module';
import { CompanyAccountComponent } from './company/company-account/company-account.component';

@NgModule({
  imports: [
    CommonModule,
    ProxyRoutingModule,
    NbCardModule,
    Ng2SmartTableModule,
    FormsModule,
    NbTabsetModule,
    ThemeModule
  ],
  declarations: [
    ProxyComponent, 
    AccountComponent,
    AccountSYSComponent,
    CompanyComponent,
    AccountListComponent, 
    AccountDetailComponent, 
    AccountAddComponent, 
    AccountSYSListComponent, 
    CompanyListComponent, 
    CompanyDetailComponent, 
    CompanyAddComponent,
    BoolRenderComponent,
    DateRenderComponent,
    DelayExprieSYSViewComponent,
    AccountSYSDetailComponent,
    AccountSYSAddComponent,
    DelayExprieViewComponent,
    AccountAddComponent,
    CompanyAccountComponent,
    ShowCompanyAccountViewComponent
  ],
  entryComponents: [
    CompanyAddComponent,
    BoolRenderComponent,
    DateRenderComponent,
    DelayExprieSYSViewComponent,
    AccountSYSAddComponent,
    DelayExprieViewComponent,
    AccountAddComponent,
    ShowCompanyAccountViewComponent
  ]  
})
export class ProxyModule { }
