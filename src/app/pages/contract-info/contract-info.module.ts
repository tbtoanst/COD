import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@app/_modules/material.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxMaskModule } from 'ngx-mask';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BsDatepickerModule, BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from "ngx-bootstrap/chronos";
import { viLocale } from "ngx-bootstrap/locale";

import { ContractInfoRoutingModule } from './contract-info-routing.module';
import { UploadContractComponent } from './upload-contract/upload-contract.component';

defineLocale("vi", viLocale);
export function getDatepickerConfig(): BsDatepickerConfig {
  return Object.assign(new BsDatepickerConfig(), {
    containerClass:'theme-dark-blue',
    dateInputFormat:'DD/MM/YYYY'
  });
}

@NgModule({
  declarations: [
    UploadContractComponent
  ],
  imports: [
    CommonModule,
    ContractInfoRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgxMaskModule.forRoot(),
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    NgSelectModule
  ],  
  providers: [
    { provide: BsDatepickerConfig, useFactory: getDatepickerConfig },
  ],
})
export class ContractInfoModule {
  constructor(
    private bsLocaleService: BsLocaleService,
  ) {
    this.bsLocaleService.use('vi');
  }
}
