import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomPreloadingStrategyService } from '@_services/custom-preloading-strategy.service';

import { AuthGuard } from '@app/_helpers';
import { LoginComponent } from './layouts/auth/login/login.component';
import { AdminComponent } from './layouts/admin/admin.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        canActivate: [AuthGuard],
        component: DashboardComponent
      },
      {
        path: 'trans-transfer',
        canActivate: [AuthGuard],
        loadChildren: () => import('@pages/trans-transfer/trans-transfer.module').then(m => m.TransTransferModule), data: { preload: true }
      },
      {
        path: 'receive-transfer',
        canActivate: [AuthGuard],
        loadChildren: () => import('@pages/receive-transfer/receive-transfer.module').then(m => m.ReceiveTransferModule), data: { preload: true }
      },
      {
        path: 'contract-info',
        canActivate: [AuthGuard],
        loadChildren: () => import('@pages/contract-info/contract-info.module').then(m => m.ContractInfoModule), data: { preload: true }
      },
      {
        path: 'trans-history',
        canActivate: [AuthGuard],
        loadChildren: () => import('@pages/trans-history/trans-history.module').then(m => m.TransHistoryModule), data: { preload: true }
      },
      {
        path: 'manage',
        canActivate: [AuthGuard],
        loadChildren: () => import('@app/pages/manage/manage.module').then(m => m.ManageModule), data: { preload: true }
      }
    ]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  // {
  //   path: '**',
  //   redirectTo: '',
  //   pathMatch: 'full'
  // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: CustomPreloadingStrategyService, relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
  providers: [
    CustomPreloadingStrategyService
  ],
  declarations: [
  ]
})
export class AppRoutingModule { }
