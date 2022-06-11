import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { Permit, User } from '@app/_models';
import { AuthenticationService, PermitService } from '@app/_services';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-transaction-adjustment',
  templateUrl: './transaction-adjustment.component.html',
  styleUrls: ['./transaction-adjustment.component.scss']
})
export class TransactionAdjustmentComponent implements OnInit {
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
  
  constructor(
    private _permitService: PermitService,
    private _authenticationService: AuthenticationService,
    private _notificationService: NotificationService,
    private _formBuilder: FormBuilder,
    private spinner: NgxSpinnerService,
  ) {
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.permit = this._permitService.getPermitByUser();
  }

  ngOnInit(): void {
    this._formControl();
  }

  get f() { return this.form.controls; }

  private _formControl() {
    this.form = this._formBuilder.group({
      amount_from: [{ value: '', disabled: false }],
      amount_to: [{ value: '', disabled: false }],
      remaining_days: [{ value: '', disabled: false }],
      interest_rate: [{ value: '', disabled: false }]
    });
  }

  private _getFormRequest() {
    const request = {
      from_date: this.f['amount_from'].value,
      to_date: this.f['amount_to'].value,
      branch_code: this.f['remaining_days'].value,
      type_code: this.f['interest_rate'].value,
      page_num: this.pageEvent.pageIndex,
      page_size: this.pageEvent.pageSize
    }
    return request;
  }

  private async _getList(request: any) {
    // try {
    //   this.dataList = [];
    //   this.isLoading = true;
    //   this.spinner.show();
    //   const res: any = await this._invoiceService.getInvoice(request);
    //   this.dataList = res.invoice_list;
    //   this.pageEvent.length = res.total_count;
    //   this.dataList = this.dataList.map((ob, i) => ({ ...ob, "is_checked": false }));
    //   this.isAllChecked = false;
    //   this.isLoading = false;
    //   this.spinner.hide()
    // } catch (error: any) {
    //   this.spinner.hide()
    //   this.isLoading = false;
    //   this.dataList = [];
    //   this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    // }
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

  public async onSubmit() {
    
  }

  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    this._getList(this._getFormRequest());
  }
}
