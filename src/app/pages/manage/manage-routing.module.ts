import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionManageComponent } from './transaction-manage/transaction-manage.component';
import { TransactionReportDailyComponent} from './transaction-report-daily/transaction-report-daily.component';
import {TransactionPriorityComponent} from './transaction-priority/transaction-priority.component';
import {TransactionAdjustmentComponent} from './transaction-adjustment/transaction-adjustment.component';
const routes: Routes = [
  {
    path: '',
    children: [     
      { path: 'trans-mana', component: TransactionManageComponent},      
      { path: 'report-process-end-day', component: TransactionReportDailyComponent},      
      { path: 'assign_priority', component: TransactionPriorityComponent},      
      { path: 'handle-trans-adjustment', component: TransactionAdjustmentComponent},      
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
