import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RealComponent } from './real.component';
import { MapListComponent } from './map-list/map-list.component';
import { OnlineListComponent } from './online-list/online-list.component';
import { AccountTraceComponent } from './account-trace/account-trace.component';
import { AuthGuardDirective } from '../../auth/auth-guard.directive';

const routes: Routes = [
  { path: '',
    component: RealComponent,
    children: [{
      path: 'accountMap',
      //canActivateChild: [AuthGuardDirective],
      children: [{
        path: 'mapList',
        component: MapListComponent,
      }, {
        path: '',
        redirectTo: 'mapList',
        pathMatch: 'full',
      },]
    }, {
      path: 'accountOnline',
      //canActivateChild: [AuthGuardDirective],
      children: [{
        path: 'onlineList',
        component: OnlineListComponent,
      }, {
        path: '',
        redirectTo: 'onlineList',
        pathMatch: 'full',
      },]
    }, {
      path: 'accountTrace',
      //canActivateChild: [AuthGuardDirective],
      children: [{
        path: 'traceDetail',
        component: AccountTraceComponent,
      }, {
        path: 'traceDetail/:accountName',
        component: AccountTraceComponent,
      }, {
        path: '',
        redirectTo: 'traceDetail',
        pathMatch: 'full',
      }]
    },]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RealRoutingModule { }
