import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@app/_helpers';
import { TransferCustInfoComponent } from './transfer-cust-info/transfer-cust-info.component';
import { TransferListComponent } from './transfer-list/transfer-list.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'transfer-cust-info', component: TransferCustInfoComponent, canActivate: [AuthGuard] },      
      { path: 'transfer-list', component: TransferListComponent, canActivate: [AuthGuard] },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransTransferRoutingModule { }
