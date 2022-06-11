import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@app/_modules/material.module';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule, BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from "ngx-bootstrap/chronos";
import { viLocale } from "ngx-bootstrap/locale";

import { ReceiveTransferRoutingModule } from './receive-transfer-routing.module';
import { CreateReceiveTransferComponent } from './create-receive-transfer/create-receive-transfer.component';
import { ReceiveTransferListComponent } from './receive-transfer-list/receive-transfer-list.component';
import { AddReceiveTransferComponent } from './create-receive-transfer/form/add-receive-transfer/add-receive-transfer.component';
import { DetailReceiveTransferComponent } from './receive-transfer-list/form/detail-receive-transfer/detail-receive-transfer.component';

defineLocale("vi", viLocale);

const maskConfig: Partial<IConfig> = {
  validation: true,
  // separatorLimit: "2",
  thousandSeparator: ',',
  decimalMarker: '.',
  allowNegativeNumbers: false,
};

export function getDatepickerConfig(): BsDatepickerConfig {
  return Object.assign(new BsDatepickerConfig(), {
    containerClass:'theme-dark-blue',
    dateInputFormat:'DD/MM/YYYY'
  });
}

@NgModule({
  declarations: [
    CreateReceiveTransferComponent,
    ReceiveTransferListComponent,
    AddReceiveTransferComponent,
    DetailReceiveTransferComponent,
  ],
  imports: [
    CommonModule,
    ReceiveTransferRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxSpinnerModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
  ],
  providers: [
    { provide: BsDatepickerConfig, useFactory: getDatepickerConfig },
  ],
})
export class ReceiveTransferModule {
  constructor(
    private bsLocaleService: BsLocaleService,
  ) {
    this.bsLocaleService.use('vi');
  }
}
