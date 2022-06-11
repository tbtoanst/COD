import { Component, OnInit, Input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TransDetailModalComponent } from '../trans-detail-modal/trans-detail-modal.component';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import _find from 'lodash/find';
import _clone from 'lodash/clone';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { TransferCustomerService, FccService } from '@app/_services';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { PageEvent } from '@angular/material/paginator';
import { TRANSFER_SELL_STATUS_LIST } from '@app/_constant/index';

@Component({
  selector: 'app-transfer-list',
  templateUrl: './transfer-list.component.html',
  styleUrls: ['./transfer-list.component.scss'],
  providers: [DatePipe]
})
export class TransferListComponent implements OnInit {
  public pageEvent: PageEvent = {
    pageIndex: 0,
    pageSize: 10,
    length: 0
  };
  public dialogCheckRef: any;
  public submitted: boolean = false;
  public isLoading: boolean = false;
  public form!: FormGroup;
  public transferListBuy: any[] = [];
  public currentDate: Date = new Date();
  public listAccountClass: any[] = [];
  public dataStatistic = {
    'CHO_GHI_NHAN_BAN' : 0,
    'CHO_DUYET_THONG_TIN' : 0,
    'CHO_MUA' : 0,
    'TU_CHOI' : 0,
    'DA_XOA' : 0,
  }
  public TRANSFER_SELL_STATUS_LIST: any[] = TRANSFER_SELL_STATUS_LIST;
  constructor(
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    private spinner: NgxSpinnerService,
    private _datePipe: DatePipe,
    private _transferCustomerService: TransferCustomerService,
    private _fccService: FccService,
    private _notificationService: NotificationService,
  ) { }
  get f() { return this.form.controls; }
  ngOnInit() {
    this._formControl();
    this._getList();
    this.fecthDataFilter()
  }
  _formControl() {
    this.form = this.formBuilder.group({
      cif: [{ value: '', disabled: false }],
      account_class: [{ value: '', disabled: false }],
      account_no: [{ value: '', disabled: false }],
      status_code: [{ value: '', disabled: false }],
      transaction_date: [{ value: this._datePipe.transform(this.currentDate, 'dd/MM/yyyy'), disabled: false }],
    });
  }
  private _getFormRequest() {
    const request = {
      account_class: this.f.account_class.value || '',
      account_no: this.f.account_no.value || '',
      cif: this.f.cif.value || '',
      status_code: this.f.status_code.value || '',
      transaction_date: moment(this.f.transaction_date.value, 'DD/MM/YYYY').format('YYYY-MM-DD') || null,
      page_num: this.pageEvent.pageIndex,
      page_size: this.pageEvent.pageSize
    }
    return request;
  }
  private async fecthDataFilter() {
    try {
      const res: any = await this._fccService.getListAccountClass();
      res.data.forEach(el => {
        el.name = el.ma_sp + " - " + el.ten_sp
      });
      this.listAccountClass = res.data;
      this.listAccountClass.unshift({'name': '---Tất cả---', 'ma_sp':''})
    } catch (error: any) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  private async _getList() {
    try {
      this.transferListBuy = [];
      this.toggleSwitchLoading(true)
      const res: any = await this._transferCustomerService.getTransferSell(this._getFormRequest());
        if(res.data.result.length > 0){

          this.transferListBuy = res.data.result;
          this.dataStatistic = {
            CHO_GHI_NHAN_BAN : res.data.total_status_sell_cho_hach_toan,
            CHO_DUYET_THONG_TIN : res.data.total_status_sell_cho_duyet_lenh_cn,
            CHO_MUA : res.data.total_status_sell_cho_duyet_lenh_ncn,
            TU_CHOI : res.data.total_status_sell_tu_choi,
            DA_XOA : res.data.total_status_sell_xoa,
          }
          this.pageEvent.length = res.data.count;
        }
        if(res.data.result.length == 0){
          this._notificationService.error('Thông báo', "Không tìm thấy thông tin");
        }
      this.toggleSwitchLoading(false)
    } catch (error: any) {
      this.toggleSwitchLoading(false)
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  toggleSwitchLoading(state) {
    if (state) {
      this.isLoading = true;
      this.submitted = true;
      this.spinner.show();
      return;
    }
    this.isLoading = false;
    this.submitted = false;
    this.spinner.hide();
  }
  onSearch() {
    this._getList();
  }
  resetFilter() {
    this.toggleSwitchLoading(false);
    this.form.reset();
    this.f.status_code.setValue('');
    this.f.account_class.setValue('');
    this.f.transaction_date.setValue(this._datePipe.transform(this.currentDate, 'dd/MM/yyyy'));
    this._getList();
  }
  onChangeBuyStatus(e) {

  }
  openTransactionDetail(tran) {
    this.dialogCheckRef = this.dialog.open(TransDetailModalComponent, {
      width: '100%',
      data: tran
    });

    this.dialogCheckRef.afterClosed().subscribe((result) => {
      if (result == 'detail') {
        this._getList();
      }
      // this.onSubmit();
    });
  }
  renderContentStatus(tran) {
    let result = _find(TRANSFER_SELL_STATUS_LIST, { 'code': tran.status });
    if (result && result.name) return result.name
    return ''
  }
  renderClassStatus(tran) {
    let result = _find(TRANSFER_SELL_STATUS_LIST, { 'code': tran.status });
    if (result && result.color) return result.color
    return ''
  }
  renderFieldTime(date) {
    return moment(date).format('DD/MM/YYYY')
  }
  formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    this._getList();
  }
}
