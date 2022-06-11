import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// import { TransCustModalComponent } from '../trans-cust-modal/trans-cust-modal.component';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import _filter from 'lodash/filter'
import _clone from 'lodash/clone';
import { Permit, User } from '@app/_models';
import { PageEvent } from '@angular/material/paginator';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import {TransferCustomerService } from '@app/_services';

@Component({
  selector: 'app-trans-account',
  templateUrl: './trans-account.component.html',
  styleUrls: ['./trans-account.component.scss']
})
export class TransAccountComponent implements OnInit {
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
  public transAccount : any[] = []
  constructor(
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    private spinner: NgxSpinnerService,
    private _notificationService: NotificationService,
    private _transferCustomerService: TransferCustomerService,
  ) { }
  get f() { return this.form.controls; }
  ngOnInit(): void {
    this._formControl();
  }
  _formControl(){
    this.form = this.formBuilder.group({
      cctg: [{ value: '', disabled: false }, Validators.required]
    });
  }
  toggleSwitchLoading(state){
    if(state){
      this.isLoading = true;
      this.submitted = true;
      this.spinner.show();
      return;
    }
    this.isLoading = false;
    this.submitted = false;
    this.spinner.hide();
  }
  async onSubmit() {
    try {
      this.submitted = true;
      if (this.form.invalid) {
        return;
      }
      this.toggleSwitchLoading(true)
      // FIX HERE
      this.transAccount = ((await this._transferCustomerService.getTransferCustomer(this.f.cctg.value))).data;
      if(this.transAccount.length == 0){
        this._notificationService.error('Thông báo', 'Không có dữ liệu');
      }
      this.toggleSwitchLoading(false)
    } catch (error: any) {
      this.toggleSwitchLoading(false)
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  async resetFilter(){
    this.transAccount = [];
    this.form.reset();
    this.toggleSwitchLoading(false)
  }
  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    // this._getList(this._getFormRequest());
  }
}
