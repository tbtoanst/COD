import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@app/_helpers';
import { UploadContractComponent } from './upload-contract/upload-contract.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'upload-contract', component: UploadContractComponent, canActivate: [AuthGuard] }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContractInfoRoutingModule { }
