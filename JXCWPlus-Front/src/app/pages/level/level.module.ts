import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LevelRoutingModule } from './level-routing.module';
import { ChangeHighLevelComponent } from './change-high-level/change-high-level.component';
import { LevelComponent } from './level.component';
import { NbThemeModule, NbCardModule, NbTabsetModule } from '@nebular/theme';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    LevelComponent,
    ChangeHighLevelComponent
  ],
  imports: [
    CommonModule,
    LevelRoutingModule,
    NbThemeModule,
    NbCardModule,
    NbTabsetModule,
    FormsModule
  ]
})
export class LevelModule { }
