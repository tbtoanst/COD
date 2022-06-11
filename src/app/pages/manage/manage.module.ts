import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { MaterialModule } from '@app/_modules/material.module';
import { NgxSpinnerModule } from 'ngx-spinner';

import { ManageRoutingModule } from './manage-routing.module';
import { TransactionManageComponent } from './transaction-manage/transaction-manage.component';
import { TransactionReportDailyComponent } from './transaction-report-daily/transaction-report-daily.component';
import { TransactionPriorityComponent } from './transaction-priority/transaction-priority.component';
import { TransactionAdjustmentComponent } from './transaction-adjustment/transaction-adjustment.component';
import { TransTransferNotApprovedComponent } from './transaction-report-daily/trans-transfer-not-approved/trans-transfer-not-approved.component';
import { ReceiveTransferNotApprovedComponent } from './transaction-report-daily/receive-transfer-not-approved/receive-transfer-not-approved.component';


@NgModule({
  declarations: [
    TransactionManageComponent,
    TransactionReportDailyComponent,
    TransactionPriorityComponent,
    TransactionAdjustmentComponent,
    TransTransferNotApprovedComponent,
    ReceiveTransferNotApprovedComponent
  ],
  imports: [
    CommonModule,
    ManageRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgxSpinnerModule
  ]
})
export class ManageModule { }
