import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TransCustModalComponent } from '../trans-cust-modal/trans-cust-modal.component';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import _remove from 'lodash/remove';
import { Permit, User } from '@app/_models';
import { PageEvent } from '@angular/material/paginator';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import {TransferCustomerService } from '@app/_services';
@Component({
  selector: 'app-transfer-cust-info',
  templateUrl: './transfer-cust-info.component.html',
  styleUrls: ['./transfer-cust-info.component.scss']
})
export class TransferCustInfoComponent implements OnInit {
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
  public btnConfirmTranfer: boolean = false;
  public dialogCheckRef: any;
  public transferCustomer: any[] = [];
  public selectTransferCustomer: any[] = [];
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
      cif: [{ value: '', disabled: false }, Validators.required]
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
    this.transferCustomer = [];
    this.selectTransferCustomer = [];
    try {
      this.submitted = true;
      if (this.form.invalid) {
        return;
      }
      this.toggleSwitchLoading(true)
      this.transferCustomer = ((await this._transferCustomerService.getTransferCustomer(this.f.cif.value))).data;
      if(this.transferCustomer.length == 0){
        this._notificationService.error('Thông báo', 'Không có dữ liệu');
      }
      this.toggleSwitchLoading(false)
    } catch (error: any) {
      this.toggleSwitchLoading(false)
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  resetFilter(){
    this.btnConfirmTranfer = false;
    this.selectTransferCustomer = [];
    this.transferCustomer = [];
    this.form.reset();
    this.toggleSwitchLoading(false);
  }
  selectCCTG(event,selectData) {
    if(event.target.checked){
      this.selectTransferCustomer.push(selectData)
      this.btnConfirmTranfer = true
    }
    if(!event.target.checked){
      _remove(this.selectTransferCustomer, function(trans) {
        return trans.stk == selectData.stk;
      });
      if(this.selectTransferCustomer.length == 0){
        this.btnConfirmTranfer = false
      }
    }
  }
  openFormCheckRefercence() {
    this.dialogCheckRef = this.dialog.open(TransCustModalComponent, {
      width: '100%',
      data: {
        transfers: this.selectTransferCustomer
      },
    });

    this.dialogCheckRef.afterClosed().subscribe((result) => {
      if(result == 'detail') {
        //clear select 
        this.resetFilter();
      }
    });
  }
  renderStatus(status){
    if(status == 'DANG PHONG TOA') return 'bg-danger'
    if(status == 'HOAT_DONG') return 'bg-success'
  }
  renderContentStatus(status){
    if(status == 'DANG PHONG TOA') return 'Phong tỏa/Tạm khóa'
    if(status == 'HOAT_DONG') return 'Hoạt động'
  }
  formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    // this._getList(this._getFormRequest());
  }
}
