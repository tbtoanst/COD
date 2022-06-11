import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import _map from 'lodash/map';
// import { NgxSpinnerService } from 'ngx-spinner';
import { User } from '@app/_models';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { AuthenticationService, EmployeeService, CustomerInfoService, TransferCustomerService } from '@app/_services';
import { PAYMENT_METHOD_LIST } from '@app/_constant/index';
@Component({
  selector: 'app-trans-cust-modal',
  templateUrl: './trans-cust-modal.component.html',
  styleUrls: ['./trans-cust-modal.component.css']
})
export class TransCustModalComponent implements OnInit {

  public isLoading: boolean = false;
  public submitted: boolean = false;
  public directEmpList: any[] = [];
  public form!: FormGroup;
  public currentUser!: User;
  public inDirectEmpList: any[] = [];
  public accountNumList: any[] = [];
  public tranferData: any;
  public PAYMENT_METHOD_LIST: any[] = PAYMENT_METHOD_LIST;
  constructor(
    public dialogCheckRef: MatDialogRef<TransCustModalComponent>,
    // private spinner: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private _authenticationService: AuthenticationService,
    private _employeeService: EmployeeService,
    private _notificationService: NotificationService,
    private _customerInfoService: CustomerInfoService,
    private _transferCustomerService: TransferCustomerService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this._authenticationService.currentUser.subscribe(user => this.currentUser = user);
    this.tranferData = this.data.transfers[0];
  }

  async ngOnInit(): Promise<void> {
    this._formControl();
    await this._getDirectEmp();
    await this._getInDirectEmp();
    this.accountNumList = (await this._customerInfoService.getAccountList(this.tranferData.ma_cif)).data.accountInfo;
  }
  async getEmployeeService(request) {
    try {
      let employee = (await this._employeeService.getList(request)).data.hrEmpDataInfo;
      return employee
    } catch (error: any) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  private async _getInDirectEmp() {
    var request = {
      branch_code: this.currentUser.positions?.branch?.code,
      employee_class: 'i'
    }
    this.inDirectEmpList = await this.getEmployeeService(request)
  }
  private async _getDirectEmp() {
    var request = {
      branch_code: this.currentUser.positions?.branch?.code,
      employee_class: 'd'
    }
    this.directEmpList = await this.getEmployeeService(request)
  }
  _formControl() {
    this.form = this.formBuilder.group({
      payment_method: [{ value: 'TM', disabled: false }],
      account_list: [{ value: '', disabled: true }],
      balance: [{ value: '', disabled: true },],
      acccount_user: [{ value: this.tranferData.ho_ten, disabled: true },],
      ccy: [{ value: 1, disabled: true }],
      member_id: [{ value: `${this.tranferData.ma_cif} - ${this.tranferData.ho_ten}`, disabled: true }],
      cmnd: [{ value: this.tranferData.cmnd, disabled: true }],
      kpi_direct: [{ value: null, disabled: false }, Validators.required],
      kpi_indirect: [{ value: null, disabled: false }, Validators.required],
    });
  }

  //   onAddValidationClick(){
  //     this.formGroup.controls["firstName"].setValidators(Validators.required);
  //     this.formGroup.controls["firstName"].updateValueAndValidity();
  // }

  // onRemoveValidationClick(){
  //     this.formGroup.controls["firstName"].clearValidators();
  //     this.formGroup.controls["firstName"].updateValueAndValidity();
  // }

  get f() {
    return this.form.controls;
  }
  clearFormAccountOnline() {
    this.f.account_list.setValue("");
    this.f.account_list.disable();
    this.f.balance.setValue("");
    this.f.balance.disable();
  }
  async getFormAccountOnline() {
    this.form.controls["account_list"].enable();
    this.f.balance.setValue(this.accountNumList[0].accountBalance);
    this.f.account_list.setValue(this.accountNumList[0].accountNum);

  }
  onChangePaymentMethod(paymentMethod) {
    if (paymentMethod.code == 'TM') { //Tiền mặt
      this.clearFormAccountOnline();
    }
    if (paymentMethod.code == 'CK') { //Tài khoản thanh toán
      this.getFormAccountOnline();
    }
  }
  onChangeAccountOnline(account) {
    this.f.balance.setValue(account.accountBalance);
  }
  async onSubmit() {
    try {
      this.submitted = true;
      if (this.form.invalid) {
        return;
      }

      this.isLoading = true;

      let transfers = this.data.transfers;
      let account_num_list = [];
      transfers.map((tran) => {
        account_num_list.push(tran.stk)
      })
      let request = {
        "sell_cif": this.tranferData.ma_cif,
        "sell_payment_method": this.f.payment_method.value,
        "sell_payment_account_no": this.f.account_list.value,
        "sell_fullname": this.tranferData.ho_ten,
        "sell_id_num": this.tranferData.cmnd,
        "kpi_indirect": this.f.kpi_indirect.value,
        "kpi_direct": this.f.kpi_direct.value,
        "account_num_list": account_num_list
      }
      const req = await this._transferCustomerService.createTransferSell(request);
      if (req.code == "OK") {
        this._notificationService.success('Thành công', 'Đã ghi nhận các TK tiền gửi của KH cần chuyển nhượng');
        this.closeModalCheck('detail')
      } else {
        this._notificationService.error('Thông báo', req.message);
        this.closeModalCheck('detail')
      }

      this.isLoading = false;
    } catch (error: any) {
      this.isLoading = false;
      if(error.status == 400){
        this._notificationService.error('Thông báo', `CCTG/TKTK ${this.data.transfers[0].stk} đã được đăng ký`);
        return;
      }
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  closeModalCheck(close: String) {
    this.dialogCheckRef.close(close);
  }
}
