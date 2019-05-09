import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LevelComponent } from './level.component';
import { ChangeHighLevelComponent } from './change-high-level/change-high-level.component';

const routes: Routes = [{
  path: '',
  component: LevelComponent,
  children: [
    {
      path: 'ChangeHighLevel',
      component: ChangeHighLevelComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LevelRoutingModule { }
