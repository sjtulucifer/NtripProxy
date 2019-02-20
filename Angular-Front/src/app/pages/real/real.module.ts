import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RealComponent } from './real.component';

import { RealRoutingModule } from './real-routing.module';
import { MapListComponent } from './map-list/map-list.component';
import { OnlineListComponent } from './online-list/online-list.component';
import { NbCardModule, NbTabsetModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { AccountTraceComponent } from './account-trace/account-trace.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    RealRoutingModule,
    NbCardModule,
    NbTabsetModule,
    Ng2SmartTableModule,
    FormsModule
  ],
  declarations: [
    RealComponent,
    MapListComponent,
    OnlineListComponent,
    AccountTraceComponent
  ]
})
export class RealModule { }
