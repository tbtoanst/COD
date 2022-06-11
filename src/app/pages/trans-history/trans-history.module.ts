import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@app/_modules/material.module';
import { NgxMaskModule,IConfig } from 'ngx-mask';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule, BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from "ngx-bootstrap/chronos";
import { viLocale } from "ngx-bootstrap/locale";

import { TransHistoryRoutingModule } from './trans-history-routing.module';
import { TransAccountComponent } from './trans-account/trans-account.component';
import { TransDateComponent } from './trans-date/trans-date.component';

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
    TransAccountComponent,
    TransDateComponent
  ],
  imports: [
    CommonModule,
    TransHistoryRoutingModule,
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
export class TransHistoryModule { 
  constructor(
    private bsLocaleService: BsLocaleService,
  ) {
    this.bsLocaleService.use('vi');
  }
}
