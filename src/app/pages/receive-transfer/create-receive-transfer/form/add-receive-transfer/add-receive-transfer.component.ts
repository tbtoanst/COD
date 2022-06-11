import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { PAYMENT_METHOD_LIST } from '@app/_constant';
import { User } from '@app/_models';
import { AuthenticationService, CreateReceiveTransferService, CustomerInfoService, EmployeeService } from '@app/_services';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-add-receive-transfer',
  templateUrl: './add-receive-transfer.component.html',
  styleUrls: ['./add-receive-transfer.component.scss']
})
export class AddReceiveTransferComponent implements OnInit {
  public currentUser!: User;
  public form!: FormGroup;
  public submitted: boolean = false;
  public isLoading: boolean = false;
  public isLoadingCIF: boolean = false;
  public isShowPaymentInfo: boolean = false;
  public dataList: any[] = [];
  public totalTransAmount: number = 0;
  public totalFee: number = 0;
  public totalPayment: number = 0;
  public paymentMethodList: any[] = PAYMENT_METHOD_LIST;
  public customerInfo: any = {};
  public accountNumList: any[] = [];
  public directEmpList: any[] = [];
  public inDirectEmpList: any[] = [];
  
  constructor(
    private _employeeService: EmployeeService,
    private _customerInfoService: CustomerInfoService,
    private _authenticationService: AuthenticationService,
    private _notificationService: NotificationService,
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddReceiveTransferComponent>,
    private _spinner: NgxSpinnerService,
    private _createReceiveTransferService: CreateReceiveTransferService,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.dataList = data.dataList;
    this.dataList.map(f => { 
      this.totalTransAmount +=  Number(f.data.giatri_giao_dich);
      this.totalFee += Number(f.data.phi_chuyen_nhuong);
      this.totalPayment += Number(f.data.so_tien_kh_tt_nhan_cn);
    });
  }

  async ngOnInit(): Promise<void> {
    this._formControl();
    await this._getDirectEmp();
    await this._getInDirectEmp();
  }

  get f() { return this.form.controls; }
  
  private _formControl() {
    this.form = this._formBuilder.group({
      cif_search: [{ value: '', disabled: false }],      
      full_name: [{ value: '', disabled: true }],
      cif: [{ value: '', disabled: true }],
      buy_payment_method: [{ value: 'CK', disabled: false }, Validators.required],
      buy_payment_account_no: [{ value: null, disabled: false }, Validators.required],
      buy_account_balance: [{ value: '', disabled: true }, Validators.required],
      buy_fullname: [{ value: '', disabled: true }, Validators.required],
      buy_payment_ccy: [{ value: 'VND', disabled: true }, Validators.required],
      buy_cif: [{ value: null, disabled: true }, Validators.required],
      buy_id_num: [{ value: '', disabled: true }, Validators.required],
      kpi_direct: [{ value: null, disabled: false }, Validators.required],
      kpi_indirect: [{ value: null, disabled: false }, Validators.required],
    });

    this.f["buy_payment_method"].valueChanges.subscribe(s => {
      switch (s) {
        case "CK":
          this.f["buy_payment_account_no"].enable({emitEvent: false});
          break;
        case "TM":          
          this.form.patchValue({
            buy_payment_account_no: null,
            buy_account_balance: '',
            buy_payment_ccy: 'VND'
          });
          this.f["buy_payment_account_no"].disable({emitEvent: false});
          break;
      }
    })

    this.f["buy_payment_account_no"].valueChanges.subscribe(s => {
      if(s) {
        var account = this.accountNumList.filter(f => { return f.accountNum == s })[0];
        this.form.patchValue({
          buy_account_balance: account.accountBalance,
          buy_payment_ccy: account.accountCurrency
        });
      }
    });
  }

  public async onSubmit() {
    try {
      this.submitted = true;
      if (this.form.invalid) {
        return;
      }
      this.isLoading = true;
      this._spinner.show();

      var request = {
        kpi_indirect: this.f["kpi_indirect"].value,
        kpi_direct: this.f["kpi_direct"].value,
        buy_payment_method: this.f["buy_payment_method"].value,
        buy_id_num: this.f["buy_id_num"].value,
        buy_payment_account_no: this.f["buy_payment_account_no"].value,
        buy_cif: this.f["buy_cif"].value,
        buy_fullname: this.f["buy_fullname"].value,
        buy_account_balance: this.f["buy_account_balance"].value,
        buy_payment_ccy: this.f["buy_payment_ccy"].value,
        sell_ids: this.dataList.map(f => { return f.id })
      }

      const result = await this._createReceiveTransferService.create(request);
      
      this.isLoading = false;
      this._spinner.hide();

      if(result.code == "OK") {
        this._notificationService.success('Thành công', 'Tạo lệnh nhận chuyển nhượng thành công');
        this.closeFormDialog('add');
      } else {
        this._notificationService.error('Thông báo', result.message);
      }
    } catch (error: any) {
      this.isLoading = false;
      this._spinner.hide();
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onSearchCIF() {
    try {
      if(!this.f["cif_search"].value) {
        this._notificationService.error('Thông báo', "Vui lòng nhập số CIF/CMND/CCCD/HC");
        return;
      }
      this.isLoadingCIF = true;
      this.customerInfo = (await this._customerInfoService.getDetailByCIF(this.f["cif_search"].value)).data;

      //Tài khoản của người nước ngoài không cho đăng ký mua
      if(this.customerInfo.customerInfo.nationlityCode != 'VN') {
        this.isLoadingCIF = false;
        this._notificationService.error('Thông báo', "Khách hàng không thuộc đối tượng (người nước ngoài) nhận chuyển nhượng");
        return;
      } 

      this.isShowPaymentInfo = true;
      this.form.patchValue({
        full_name: this.customerInfo.customerInfo.fullname_vn,
        cif: this.customerInfo.cifInfo.CIFNum,
        buy_fullname: this.customerInfo.customerInfo.fullname_vn,
        buy_cif: this.customerInfo.cifInfo.CIFNum,
        buy_id_num: this.customerInfo.customerInfo.IDInfo.IDNum,
      })

      this.accountNumList = (await this._customerInfoService.getAccountList(this.f["cif_search"].value)).data.accountInfo;

      this.isLoadingCIF = false;
      
    } catch (error: any) {
      this.isLoadingCIF = false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  private async _getDirectEmp() {
    try {
      var request = {
        branch_code: this.currentUser.positions?.branch?.code,
        employee_class: 'd'
      }
      this.directEmpList = (await this._employeeService.getList(request)).data.hrEmpDataInfo;
    } catch (error: any) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  private async _getInDirectEmp() {
    try {
      var request = {
        branch_code: this.currentUser.positions?.branch?.code,
        employee_class: 'i'
      }
      this.inDirectEmpList = (await this._employeeService.getList(request)).data.hrEmpDataInfo;
    } catch (error: any) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public calTotalSellPaymentAmount(paymentValue, fee) {
    return Number(paymentValue) + Number(fee);
  }

  public formatVND(origin) {
    origin = Math.round(origin*1) + '';
    return origin.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }

  public closeFormDialog(close: string) {
    this.dialogRef.close(close);
  }

}
