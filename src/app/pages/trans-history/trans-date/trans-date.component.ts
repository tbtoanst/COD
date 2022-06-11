
import { Component, OnInit, Input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// import { TransCustModalComponent } from '../trans-cust-modal/trans-cust-modal.component';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { AuthenticationService, PermitService, TransferCustomerService } from '@app/_services';
import _filter from 'lodash/filter';
import _clone from 'lodash/clone';
import * as moment from 'moment';
import { Permit, User } from '@app/_models';
import { PageEvent } from '@angular/material/paginator';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-trans-date',
  templateUrl: './trans-date.component.html',
  styleUrls: ['./trans-date.component.scss'],
  providers: [DatePipe]
})
export class TransDateComponent implements OnInit {
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
  public dialogRef: any;
  dataTemp = [
    {
      'id': 1,
      'cif': 1,
      'name': 'Nguyên Hiền',
      'cmnd': '1',
      'cctg': '1',
      'balance': '200.000',
      'status': '1',
      'check': false
    },
    {
      'id': 2,
      'cif': 1,
      'name': 'Nguyên Hiền',
      'cmnd': '1',
      'cctg': '1',
      'balance': '200.000',
      'status': '1',
      'check': false
    }
  ]
  isWaiting: false;
  dialogCheckRef: any;
  btnConfirmTranfer: boolean = false;
  listTransCheck = [];
  public transPerDay: any[] = []
   public currentDate: Date = new Date();
  constructor(
    private formBuilder: FormBuilder,
    private _permitService: PermitService,
    private _authenticationService: AuthenticationService,
    private _notificationService: NotificationService,
    private _formBuilder: FormBuilder,
    private spinner: NgxSpinnerService,
    public dialog: MatDialog,
        private _datePipe: DatePipe,
    private _transferCustomerService: TransferCustomerService,
  ) { 
    
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.permit = this._permitService.getPermitByUser();
  }
  get f() { return this.form.controls; }
  ngOnInit(): void {
    this._formControl();
  }
  _formControl(){
    this.form = this.formBuilder.group({
      trans_day: [{ value: this._datePipe.transform(this.currentDate, 'dd/MM/yyyy'), disabled: false }],
      branch: [{ value: '', disabled: false }, Validators.required]
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
      // this.transPerDay = ((await this._transferCustomerService.getTransferCustomer(this.f.cctg.value))).data;
      if(this.transPerDay.length == 0){
        this._notificationService.error('Thông báo', 'Không có dữ liệu');
      }
      this.toggleSwitchLoading(false)
    } catch (error: any) {
      this.toggleSwitchLoading(false)
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  async resetFilter(){
    this.transPerDay = [];
    this.form.reset();
    this.f.trans_day.setValue(this._datePipe.transform(this.currentDate, 'dd/MM/yyyy'))
    this.toggleSwitchLoading(false)
  }

  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    // this._getList(this._getFormRequest());
  }
}
