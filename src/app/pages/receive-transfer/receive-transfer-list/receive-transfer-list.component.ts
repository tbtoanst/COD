import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { ADVANCE_STATUS_LIST, TRANSFER_BUY_STATUS_LIST } from '@app/_constant';
import { Permit, User } from '@app/_models';
import { AuthenticationService, FccService, PermitService } from '@app/_services';
import { ReceiveTransferListService } from '@app/_services/receive-transfer-list.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { DetailReceiveTransferComponent } from './form/detail-receive-transfer/detail-receive-transfer.component';
import * as moment from 'moment';

@Component({
  selector: 'app-receive-transfer-list',
  templateUrl: './receive-transfer-list.component.html',
  styleUrls: ['./receive-transfer-list.component.scss'],
  providers: [DatePipe]
})
export class ReceiveTransferListComponent implements OnInit {
  public currentUser!: User;
  public pageEvent: PageEvent = {
    pageIndex: 0,
    pageSize: 10,
    length: 0
  };
  public form!: FormGroup;
  public submitted: boolean = false;
  public isLoading: boolean = false;
  public permit: Permit = {
    is_edit: false,
    is_print_contract: false,
    is_export: false,
    is_query: false,
    is_add: false,
    is_delete: false,
    is_send_approve: false,
    is_upload: false,
    is_approve: false,
    is_approve_upload: false,
    is_reject: false,
  };
  public dataStatistic = {
    'CHO_GHI_NHAN_BAN' : 0,
    'CHO_DUYET_THONG_TIN' : 0,
    'CHO_MUA' : 0,
    'TU_CHOI' : 0,
    'DA_XOA' : 0,
  }
  public accountClassList: any[] = [];
  public statusList: any[] = TRANSFER_BUY_STATUS_LIST;
  public dataList: any[] = [];
  public dialogRef: any;
  public advanceStatusList: any[] = ADVANCE_STATUS_LIST;
  public currentDate: Date = new Date();
  constructor(
    private _receiveTransferListService: ReceiveTransferListService,
    private _permitService: PermitService,
    private _authenticationService: AuthenticationService,
    private _fccService: FccService,
    private _notificationService: NotificationService,
    private _formBuilder: FormBuilder,
    private _spinner: NgxSpinnerService,
    public dialog: MatDialog,
    private _datePipe: DatePipe,
  ) {
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.permit = this._permitService.getPermitByUser();
  }

  ngOnInit(): void {
    this._formControl();
    this._getAccountClassList();
    this._getList(this._getFormRequest());
  }

  get f() { return this.form.controls; }

  private _formControl() {
    this.form = this._formBuilder.group({
      account_class_code: [{ value: '', disabled: false }],
      account_num: [{ value: '', disabled: false }],
      cif: [{ value: '', disabled: false }],
      advance_status_code: [{ value: '', disabled: false }],
      status_code: [{ value: '', disabled: false }],
      trans_date: [{ value: this._datePipe.transform(this.currentDate, 'dd/MM/yyyy'), disabled: true }],
    });
  }

  private async _getAccountClassList() {
    try {
      const res: any = await this._fccService.getListAccountClass();
      res.data.forEach(el => {
        el.name = el.ma_sp + " - " + el.ten_sp
      });
      this.accountClassList = res.data;      
      this.accountClassList.unshift({'name': '---Tất cả---', 'ma_sp':''})
    } catch (error: any) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  private _getFormRequest() {
    const request = {
      account_class_code: this.f['account_class_code'].value == null ? '' : this.f['account_class_code'].value,
      account_num: this.f['account_num'].value == null ? '' : this.f['account_num'].value,
      cif: this.f['cif'].value == null ? '' : this.f['cif'].value,
      advance_status_code: this.f['advance_status_code'].value == null ? '' : this.f['advance_status_code'].value,
      status_code: this.f['status_code'].value == null ? '' : this.f['status_code'].value,
      trans_date: moment(this.f["trans_date"].value, 'DD/MM/YYYY').format("YYYY-MM-DD"),
      page_num: this.pageEvent.pageIndex,
      page_size: this.pageEvent.pageSize
    }
    return request;
  }

  private async _getList(request: any) {
    try {
      this.dataList = [];
      this.isLoading = true;
      this._spinner.show();

      const res: any = (await this._receiveTransferListService.getList(request)).data;
      this.dataList = res.result;
      this.pageEvent.length = res.count;
      this.dataStatistic = {
        CHO_GHI_NHAN_BAN : res.total_status_buy_cho_hach_toan,
        CHO_DUYET_THONG_TIN : res.total_status_buy_cho_duyet_lenh_cn,
        CHO_MUA : res.total_status_buy_cho_duyet_lenh_ncn,
        TU_CHOI : res.total_status_buy_tu_choi,
        DA_XOA : res.total_status_buy_xoa,
      }
      if (this.dataList.length == 0) {
        this._notificationService.error('Thông báo', "Không tìm thấy dữ liệu");
      } else {
        this.dataList = this.dataList.map((ob, i) => ({ ...ob, "is_checked": false }));
      }

      this.isLoading = false;
      this._spinner.hide();
    } catch (error: any) {
      this._spinner.hide()
      this.isLoading = false;
      this.dataList = [];
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onSearch() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    this.pageEvent.pageIndex = 0;
    this.pageEvent.pageSize = 10;
    await this._getList(this._getFormRequest());
  }
  public resetFilter(){
    this.f.account_class_code.setValue('')
    this.f.account_num.setValue('')
    this.f.cif.setValue('')
    this.f.advance_status_code.setValue('')
    this.f.status_code.setValue('')
    this.pageEvent.pageIndex = 0;
    this._getList(this._getFormRequest());
  }
  public onCheck(item: any) {
    item.is_checked = !item.is_checked;
  }

  public openForm(type: string, item: any) {
    switch (type) {
      case 'DETAIL':
        this.dialogRef = this.dialog.open(DetailReceiveTransferComponent, {
          width: '90vw',
          data: item
        });
        this.dialogRef.afterClosed().subscribe((result: any) => {
          if (result == 'detail') {
            this._getList(this._getFormRequest());
          }
        });
        break;
    }
  }
  public formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  public renderStatus(statusCode) {
    return this.statusList.filter(f => f.code == statusCode)[0];
  }

  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    this._getList(this._getFormRequest());
  }
}
