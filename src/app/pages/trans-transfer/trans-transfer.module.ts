import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@app/_modules/material.module';
import { NgxMaskModule } from 'ngx-mask';
import { NgxSpinnerModule } from 'ngx-spinner';

import { TransTransferRoutingModule } from './trans-transfer-routing.module';
import { TransferCustInfoComponent } from './transfer-cust-info/transfer-cust-info.component';
import { TransDetailModalComponent } from './trans-detail-modal/trans-detail-modal.component';
import { TransferListComponent } from './transfer-list/transfer-list.component';
import { TransCustModalComponent } from './trans-cust-modal/trans-cust-modal.component';
import { BsDatepickerModule, BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from "ngx-bootstrap/chronos";
import { viLocale } from "ngx-bootstrap/locale";


defineLocale("vi", viLocale);

export function getDatepickerConfig(): BsDatepickerConfig {
  return Object.assign(new BsDatepickerConfig(), {
    containerClass:'theme-dark-blue',
    dateInputFormat:'DD/MM/YYYY'
  });
}

@NgModule({
  declarations: [
    TransferCustInfoComponent,
    TransferListComponent,
    TransDetailModalComponent,
    TransCustModalComponent
  ],
  imports: [
    CommonModule,
    TransTransferRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgxMaskModule.forRoot(),
    NgxSpinnerModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
  ],
  providers: [
    { provide: BsDatepickerConfig, useFactory: getDatepickerConfig },
  ],
})
export class TransTransferModule {
  constructor(
    private bsLocaleService: BsLocaleService,
  ) {
    this.bsLocaleService.use('vi');
  }
}
