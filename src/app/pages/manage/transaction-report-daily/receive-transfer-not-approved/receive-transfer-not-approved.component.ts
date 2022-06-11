import { Component, OnInit, Input,Output, EventEmitter } from '@angular/core';
import _find from 'lodash/find';
import _clone from 'lodash/clone';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { TransactionReportService } from '@app/_services';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { PageEvent } from '@angular/material/paginator';
import { TRANSFER_BUY_STATUS_LIST } from '@app/_constant/index';

@Component({
  selector: 'app-receive-transfer-not-approved',
  templateUrl: './receive-transfer-not-approved.component.html',
  styleUrls: ['./receive-transfer-not-approved.component.scss']
})
export class ReceiveTransferNotApprovedComponent implements OnInit {
  public pageEvent: PageEvent = {
    pageIndex: 0,
    pageSize: 10,
    length: 0
  };
  public isLoading: boolean = false;

  public dataList: any[] = [];
  public transferListBuy: any[] = [];
  public TRANSFER_BUY_STATUS_LIST: any[] = TRANSFER_BUY_STATUS_LIST;

  constructor(
    private _notificationService: NotificationService,
    private spinner: NgxSpinnerService,
    private _transactionReportService: TransactionReportService,
  ) {

  }


  ngOnInit(): void {
    this._getList();
  }
  private async _getList() {
    try {
      this.transferListBuy = [];
      this.spinner.show();
      const res: any = await this._transactionReportService.cleanBuyQuery(this._getFormRequest());
      if (res.data.result.length > 0) {
        this.transferListBuy = res.data.result;
        this.pageEvent.length = res.data.count;
      }
      if (res.data.result.length == 0) {
        this._notificationService.error('Thông báo', "Không tìm thấy thông tin");
      }
      this.spinner.hide();
    } catch (error: any) {
      this.spinner.hide();
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  private _getFormRequest() {
    const request = {
      page_num: this.pageEvent.pageIndex,
      page_size: this.pageEvent.pageSize
    }
    return request;
  }
  public formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  public renderFieldTime(date) {
    return moment(date).format('DD/MM/YYYY')
  }
  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    this._getList();
  }
  public renderContentStatus(status) {
    let result = _find(TRANSFER_BUY_STATUS_LIST, { 'code': status });
    if (result && result.name) return result.name
    return ''
  }
  public renderClassStatus(status) {
    let result = _find(TRANSFER_BUY_STATUS_LIST, { 'code':status });
    if (result && result.color) return result.color
    return ''
  }
}
