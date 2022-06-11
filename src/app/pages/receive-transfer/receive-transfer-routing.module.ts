import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@app/_helpers';
import { CreateReceiveTransferComponent } from './create-receive-transfer/create-receive-transfer.component';
import { ReceiveTransferListComponent } from './receive-transfer-list/receive-transfer-list.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'create-receive-transfer', component: CreateReceiveTransferComponent, canActivate: [AuthGuard] },      
      { path: 'receive-transfer-list', component: ReceiveTransferListComponent, canActivate: [AuthGuard] },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReceiveTransferRoutingModule { }
