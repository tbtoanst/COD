import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmModalService } from '@app/_components/confirm-modal/_services/confirm-modal.service';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { PAYMENT_METHOD_LIST, PAYOUT_EXPIRE_TYPE_CODE, TRANSFER_BUY_STATUS_CODE, TRANSFER_BUY_STATUS_LIST, TYPE_PAYMENT_STATUS_CODE } from '@app/_constant';
import { Permit, User } from '@app/_models';
import _find from 'lodash/find';
import { AuthenticationService, CustomerInfoService, EmployeeService, PermitService } from '@app/_services';
import { ReceiveTransferListService } from '@app/_services/receive-transfer-list.service';
import * as moment from 'moment';

@Component({
  selector: 'app-detail-receive-transfer',
  templateUrl: './detail-receive-transfer.component.html',
  styleUrls: ['./detail-receive-transfer.component.scss']
})
export class DetailReceiveTransferComponent implements OnInit {
  public currentUser!: User;
  public form!: FormGroup;
  public submitted: boolean = false;
  public isLoading: boolean = false;
  public isLoadingDelete: boolean = false;
  public isLoadingPrintContract: boolean = false;
  public isLoadingReject: boolean = false;
  public isLoadingApproved: boolean = false;
  public isLoadingGetSeriNum: boolean = false;
  public isLoadingConfirmSeriNum: boolean = false;
  public serialTransaction:"";
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
  public directEmpList: any[] = [];
  public inDirectEmpList: any[] = [];
  public paymentMethodList: any[] = PAYMENT_METHOD_LIST;  
  public statusList: any[] = TRANSFER_BUY_STATUS_LIST;
  public PAYOUT_EXPIRE_TYPE_CODE: any[] = PAYOUT_EXPIRE_TYPE_CODE;
  public TYPE_PAYMENT_STATUS_CODE: any[] = TYPE_PAYMENT_STATUS_CODE;
  public isShowButtonSeri: boolean = false;
  public isShowButtonConfirmSeri: boolean = false;
  public isShowButtonActionKSV: boolean = false;
  public isShowButtonActionKSVReject: boolean = false;
  public isShowButtonActionGDV: boolean = false;
  public isShowButtonActionGDVDelete: boolean = false;
  public payoutAccountNumList: any[] = [];
  public confirmSerialTransaction: boolean = false;
  
  constructor(
    private _employeeService: EmployeeService,
    private _customerInfoService: CustomerInfoService,
    private _receiveTransferListService: ReceiveTransferListService,
    private _permitService: PermitService,
    private _authenticationService: AuthenticationService,
    private _notificationService: NotificationService,
    private _confirmModalService: ConfirmModalService,
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<DetailReceiveTransferComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.permit = this._permitService.getPermitByUser();
    
    switch (this.data.status) {
      case TRANSFER_BUY_STATUS_CODE.TAM_GIU_MUA:
        this.isShowButtonActionGDV = true;
        this.isShowButtonActionGDVDelete = true;
        this.isShowButtonSeri = true;
        break;
      case TRANSFER_BUY_STATUS_CODE.CHO_DUYET_LENH_MUA:
        this.isShowButtonActionKSV = true;
        this.isShowButtonActionKSVReject = true;
        break;
      case TRANSFER_BUY_STATUS_CODE.CHO_CQSH_MUA:
        this.isShowButtonActionGDV = true;
        break;
      case TRANSFER_BUY_STATUS_CODE.CHO_DUYET_CQSH_MUA:
        this.isShowButtonActionKSV = true;
        break;
    }

    // if(this.data.status == TRANSFER_BUY_STATUS_CODE.TAM_GIU_MUA) {
    //   this.isShowButtonActionGDV = true;
    //   this.isShowButtonSeri = true;
    // }
    // if(this.data.status == TRANSFER_BUY_STATUS_CODE.CHO_DUYET_LENH_MUA) {
    //   this.isShowButtonActionKSV = true;
    // }
  }

  async ngOnInit(): Promise<void> {
    this._formControl();
    await this._getDirectEmp();
    await this._getInDirectEmp();
    await this._getPayoutAccountList();
    if(this.data.sell.data.hinhthuc_daohan == 'TAI KY GOC LAI') {
        this.f.buy_payout_acc_num.clearValidators();
        this.f.buy_payout_acc_num.disable();
        if(this.data.sell.data.hinh_thuc_linh_lai == TYPE_PAYMENT_STATUS_CODE.CUOI_KY){
          this.f.buy_payout_acc_num.setValue(this.data.sell.data.stk)
        }
    }
    if(this.data.status !== TRANSFER_BUY_STATUS_CODE.TAM_GIU_MUA){
      this.f.buy_payout_acc_num.disable();
      this.f.buy_seri_num.disable();
    }
  }

  get f() { return this.form.controls; }
  
  private _formControl() {
    this.form = this._formBuilder.group({
      ////thông tin đơn vị chuyển nhượng
      sell_branch_code: [{ value: this.data.sell.created_branch_code, disabled: true }],
      sell_created_user: [{ value: this.data.sell.created_user, disabled: true }],
      sell_teller_user: [{ value: this.data.sell.teller_user, disabled: true }],
      sell_approved_user: [{ value: this.data.sell.approved_user, disabled: true }],
      sell_account_num: [{ value: this.data.sell.account_num, disabled: true }],
      sell_approved_date: [{ value: moment(this.data.sell.approved_date).format("DD/MM/YYYY"), disabled: true }],

      ////thông tin lệnh nhận chuyển nhượng
      buy_account_num: [{ value: this.data.data.stk, disabled: true }],
      transaction_date: [{ value: moment(this.data.transaction_date).format("DD/MM/YYYY"), disabled: true }],
      buy_account_class: [{ value: this.data.data.loai_hinh, disabled: true }],
      buy_interest_rate: [{ value: this.data.data.lai_suat, disabled: true }],
      buy_account_balance: [{ value: this.data.data.so_du, disabled: true }],
      buy_remain_day: [{ value: this.data.data.so_ngay_con_lai, disabled: true }],
      open_date: [{ value: moment(this.data.data.ngay_mo).format("DD/MM/YYYY"), disabled: true }],
      expired_date: [{ value: moment(this.data.data.ngay_tai_ky).format("DD/MM/YYYY"), disabled: true }],
      buy_fee: [{ value: this.data.data.phi_chuyen_nhuong, disabled: true }],
      buy_status: [{ value: this.data.status, disabled: true }],
      buy_trans_value: [{ value: this.data.data.giatri_giao_dich, disabled: true }],
      buy_payment_amount: [{ value: this.data.data.so_tien_kh_tt_nhan_cn, disabled: true }],

      ////thông tin khách hàng nhận chuyển nhượng
      buy_fullname: [{ value: this.data.buy_fullname, disabled: true }],
      buy_cif: [{ value: this.data.buy_cif, disabled: true }],
      payout_expire_type: [{ value: this.data.sell.data.hinhthuc_daohan, disabled: true }],
      payout_fullname: [{ value: this.data.buy_fullname, disabled: true }],
      buy_payout_acc_num: [{ value: this.data.buy_payout_acc_num, disabled: false  }, Validators.required],
      buy_seri_num: [{ value: this.data.seri_no, disabled: true }, Validators.required],

      ////Thông tin thanh toán
      buy_payment_method: [{ value: this.data.buy_payment_method, disabled: true }],
      buy_payment_account_no: [{ value: this.data.buy_payment_account_no, disabled: true }],
      buy_account_available: [{ value: this.data.buy_account_balance, disabled: true }],
      buy_payment_ccy: [{ value: this.data.buy_payment_ccy, disabled: true }],
      buy_id_num: [{ value: this.data.buy_id_num, disabled: true }],
      kpi_direct: [{ value: this.data.kpi_direct, disabled: true }],
      kpi_indirect: [{ value: this.data.kpi_indirect, disabled: true }],

      ////Thông tin tư vấn
      created_branch_code: [{ value: this.data.created_branch_code, disabled: true }],
      created_date: [{ value: moment(this.data.created_date).format("DD/MM/YYYY"), disabled: true }],
      xref_id: [{ value: this.data.xref_id, disabled: true }],
      teller_user: [{ value: this.data.teller_user, disabled: true }],
      approved_user: [{ value: this.data.approved_user, disabled: true }],
      approved_date: [{ value: this.data.approved_date, disabled: true }],
    });
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

  private async _getPayoutAccountList() {
    try {
      this.payoutAccountNumList = (await this._customerInfoService.getAccountList(this.data.buy_cif)).data.accountInfo;
    } catch (error: any) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }    
  }

  public async onGetSeriNum() {
    try {
      this.isLoadingGetSeriNum= true;
      const result = await this._receiveTransferListService.getSeriNum(this.data.data.loai_hinh);
      this.isLoadingGetSeriNum = false;
      if(result.code == "OK") {
        this.serialTransaction = result.data.pm_serial_number;
        this.f["buy_seri_num"].setValue(result.data.pm_serial_prefix + result.data.pm_serial_number);
        this.isShowButtonConfirmSeri = true;
        this.isShowButtonSeri = false;
        this._notificationService.success('Thành công', 'Lấy số seri thành công');
      } else {
        this._notificationService.error('Thông báo', result.message);
      }
    } catch (error: any) {
      this.isLoadingGetSeriNum= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onConfirmSeriNum() {
    try {
      this.isLoadingConfirmSeriNum= true;
      const request = {
        acc_class: this.data.data.loai_hinh,
        serial_number: this.serialTransaction
      }
      const result = await this._receiveTransferListService.confirmSeriNumm(request);
      this.isLoadingConfirmSeriNum = false;
      if(result.code == "OK") {
        this.isShowButtonConfirmSeri = false;
        this.confirmSerialTransaction = true;
        this._notificationService.success('Thành công', 'Xác nhận số seri thành công');
      } else {
        this._notificationService.error('Thông báo', result.message);
      }
    } catch (error: any) {
      this.isLoadingConfirmSeriNum= false;
      this.confirmSerialTransaction = false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onUpdate() {
    try {
      this.submitted = true;
      if (this.form.invalid) {
        return;
      }
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn cập nhật lệnh nhận chuyển nhượng này không?",
        async () => {
          this.isLoading = true;
          var request = {
            status: TRANSFER_BUY_STATUS_CODE.CHO_DUYET_LENH_MUA,
            seri_no: this.f["buy_seri_num"].value,
            buy_payout_acc_num: this.f["buy_payout_acc_num"].value
          };
          const result = await this._receiveTransferListService.update(this.data.id, request);
          this.isLoading = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Cập nhật lệnh nhận chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { this.isLoading = false; }
      );
    } catch (error: any) {
      this.isLoading = false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onSendApproved() {
    try {
      this.submitted = true;
      if (this.form.invalid) {
        return;
      }
      if(!this.confirmSerialTransaction && this.data.status == TRANSFER_BUY_STATUS_CODE.TAM_GIU_MUA){
        this._notificationService.error('Thông báo', 'Chưa xác nhận số Seri');
        return;
      }
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn gửi duyệt lệnh nhận chuyển nhượng này không?",
        async () => {
          this.isLoading = true;

          //update field
          var request = {
            seri_no: this.f["buy_seri_num"].value,
            buy_payout_acc_num: this.f["buy_payout_acc_num"].value
          };
          await this._receiveTransferListService.update(this.data.id, request);

          const result = await this._receiveTransferListService.sendApproved(this.data.id);
          this.isLoading = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Gửi duyệt lệnh nhận chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { this.isLoading = false; }
      );
    } catch (error: any) {
      this.isLoading = false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onReject() {
    try {
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn từ chối lệnh nhận chuyển nhượng này không?",
        async () => {
          this.isLoadingReject = true;
          const result = await this._receiveTransferListService.reject(this.data.id);
          this.isLoadingReject = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Từ chối lệnh nhận chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { this.isLoadingReject = false; }
      );
    } catch (error: any) {
      this.isLoadingReject= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public async onApproved() {
    try {
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn duyệt lệnh nhận chuyển nhượng này không?",
        async () => {
          this.isLoadingApproved= true;
          const result = await this._receiveTransferListService.approved(this.data.id);
          this.isLoadingApproved = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Duyệt lệnh nhận chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { this.isLoadingApproved = false; }
      );
    } catch (error: any) {
      this.isLoadingApproved= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public onPrintContract() {
    try {
      this.isLoadingPrintContract = true;
      this._receiveTransferListService.printContract(this.data.id).then(
        (result: Blob) => {
          this.isLoadingPrintContract = false;
          const blob = new Blob([result], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
          const url = window.URL.createObjectURL(blob);
          // window.open(url, '_blank');
          const link = document.createElement('a');
          const fileName = this.data.data.stk + ".doc";
          link.href = url;
          link.setAttribute('download', fileName);
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        },
        error => {
          this.isLoadingPrintContract = false;
          this._notificationService.error('Thông báo', (error.error.message) ? error.error.message : error.message);
        }
      );
    } catch (error: any) {
      this.isLoadingPrintContract= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public onDelete() {
    
    this._confirmModalService.confirmModal(
      "Thông báo",
      "Bạn có chắc muốn xóa hóa đơn này không?",
      () => {
        this.isLoadingDelete = true;
        this._receiveTransferListService.delete(this.data.id).then(
          rs => {
            this.isLoadingDelete = false;
            if (rs.code == "OK") {
              this._notificationService.success('Thông báo', 'Xóa lệnh nhận chuyển nhượng thành công');
              this.closeFormDialog('detail');
            } else {
              this._notificationService.error('Thông báo', rs.message)
            }
          },
          error => {
            this._notificationService.error('Thông báo', error.error.message)
          }
        )
      },
      () => { this.isLoadingDelete = false; }
    );
  }
  renderContentStatus(status){
    return (_find(TRANSFER_BUY_STATUS_LIST, {'code':status})).name;
  }
  renderClassStatus(status){
    return (_find(TRANSFER_BUY_STATUS_LIST, {'code':status})).color;
  }
  public closeFormDialog(close: string) {
    this.dialogRef.close(close);
  }
}
