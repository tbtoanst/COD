import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { ADVANCE_STATUS_LIST, TRANSFER_SELL_STATUS_CODE } from '@app/_constant';
import { Permit, User } from '@app/_models';
import { AuthenticationService, PermitService } from '@app/_services';
import { CreateReceiveTransferService } from '@app/_services/create-receive-transfer.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AddReceiveTransferComponent } from './form/add-receive-transfer/add-receive-transfer.component';

@Component({
  selector: 'app-create-receive-transfer',
  templateUrl: './create-receive-transfer.component.html',
  styleUrls: ['./create-receive-transfer.component.scss']
})
export class CreateReceiveTransferComponent implements OnInit {
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
  public dataList: any[] = [];
  public checkedList: any[] = [];
  public dialogRef: any;
  public advanceStatusList: any[] = ADVANCE_STATUS_LIST;

  constructor(
    private _permitService: PermitService,
    private _authenticationService: AuthenticationService,
    private _createReceiveTransferService: CreateReceiveTransferService,
    private _notificationService: NotificationService,
    private _formBuilder: FormBuilder,
    private _spinner: NgxSpinnerService,
    public dialog: MatDialog,
  ) {
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.permit = this._permitService.getPermitByUser();
  }

  ngOnInit(): void {
    this._formControl();
    this._getList(this._getFormRequest());
  }

  get f() { return this.form.controls; }

  private _formControl() {
    this.form = this._formBuilder.group({
      account_balance_from: [{ value: '', disabled: false }],
      account_balance_to: [{ value: '', disabled: false }],
      remain_day_from: [{ value: '', disabled: false }],
      remain_day_to: [{ value: '', disabled: false }],
      advance_status_code: [{ value: '', disabled: false }],
      interest_rate: [{ value: '', disabled: false }]
    });
  }

  private _getFormRequest() {
    const request = {
      account_balance_from: this.f['account_balance_from'].value == null ? '' : this.f['account_balance_from'].value,
      account_balance_to: this.f['account_balance_to'].value == null ? '' : this.f['account_balance_to'].value,
      remain_day_from: this.f['remain_day_from'].value == null ? '' : this.f['remain_day_from'].value,
      remain_day_to: this.f['remain_day_to'].value == null ? '' : this.f['remain_day_to'].value,
      advance_status_code: this.f['advance_status_code'].value == null ? '' : this.f['advance_status_code'].value,
      status_code: TRANSFER_SELL_STATUS_CODE.CHO_MUA_BAN,
      interest_rate: this.f['interest_rate'].value == null ? '' : this.f['interest_rate'].value,
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
      
      const res = (await this._createReceiveTransferService.getList(request)).data;
      this.dataList = res.result;
      this.pageEvent.length = res.count;
      
      if(this.dataList.length == 0) {
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

    if(this.f["account_balance_from"].value && this.f["account_balance_to"].value) {
      if(Number(this.f["account_balance_from"].value) > Number(this.f["account_balance_to"].value)) {
        this._notificationService.error('Thông báo', "Mệnh giá từ - đến không hợp lệ");
        return;
      }
    }

    if(this.f["remain_day_from"].value && this.f["remain_day_to"].value) {
      if(Number(this.f["remain_day_from"].value) > Number(this.f["remain_day_to"].value)) {
        this._notificationService.error('Thông báo', "Số ngày còn lại từ - đến không hợp lệ");
        return;
      }
    }

    this.pageEvent.pageIndex = 0;
    this.pageEvent.pageSize = 10;
    await this._getList(this._getFormRequest());
  }

  public onCheck(item: any) {
    item.is_checked = !item.is_checked;
  }

  public openForm(type: string) {
    switch (type) {
      case 'ADD':
        const checkedList = this.dataList.filter(f => f.is_checked === true);
        if(checkedList.length == 0) {
          this._notificationService.error('Thông báo', "Vui lòng chọn TKTK/GTCG cần mua");
          return;
        }

        this.dialogRef = this.dialog.open(AddReceiveTransferComponent, {
          width: '90vw',
          data: { dataList: checkedList }
        });
        this.dialogRef.afterClosed().subscribe((result: any) => {
          if(result == 'add') {
            this._getList(this._getFormRequest());
          }
        });
        break;
    }
  }

  public calTotalSellPaymentAmount(paymentValue, fee) {
    return Number(paymentValue) + Number(fee);
  }

  public onClearForm() {
    this.form.reset();
  }

  public formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  
  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    this._getList(this._getFormRequest());
  }
}
