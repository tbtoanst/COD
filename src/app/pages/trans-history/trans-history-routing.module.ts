import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@app/_helpers';
import { TransAccountComponent } from './trans-account/trans-account.component';
import { TransDateComponent } from './trans-date/trans-date.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'trans-account', component: TransAccountComponent, canActivate: [AuthGuard] },      
      { path: 'trans-date', component: TransDateComponent, canActivate: [AuthGuard] },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransHistoryRoutingModule { }
