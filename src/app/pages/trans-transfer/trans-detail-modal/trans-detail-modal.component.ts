import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {PAYMENT_METHOD_LIST,TKTK_CCTG_STATUS_LIST,TRANSFER_SELL_STATUS_CODE, TRANSFER_SELL_STATUS_LIST} from '@app/_constant/index';
import { AuthenticationService, PermitService, TransferListService } from '@app/_services';
import { Permit, User } from '@app/_models';
import _find from 'lodash/find';
import * as moment from 'moment';
import { ConfirmModalService } from '@app/_components/confirm-modal/_services/confirm-modal.service';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
@Component({
  selector: 'app-trans-detail-modal',
  templateUrl: './trans-detail-modal.component.html',
  styleUrls: ['./trans-detail-modal.component.css']
})

export class TransDetailModalComponent implements OnInit {
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
  public userIsNVTV: boolean = false;
  public userIsGDV: boolean = false;
  public userIsGDVDelete: boolean = false;
  public userIsKSV: boolean = false;
  public userIsKSVReject: boolean = false;
  public isLoading: boolean = false;
  public submitted: boolean = false;
  public isLoadingDelete: boolean = false;
  public isLoadingPrintContract: boolean = false;
  public isLoadingReject: boolean = false;
  public isLoadingApproved: boolean = false;
  public tranData: any = {}
  public isAnotherCustomer: boolean = false;
  public currentUser: any = {};
  public form!: FormGroup;
  public PAYMENT_METHOD_LIST: any[] = PAYMENT_METHOD_LIST;
  public TKTK_CCTG_STATUS_LIST: any[] = TKTK_CCTG_STATUS_LIST;
  public TRANSFER_SELL_STATUS_CODE: any = TRANSFER_SELL_STATUS_CODE;
  constructor(
    public dialogCheckRef: MatDialogRef<TransDetailModalComponent>,
    private formBuilder: FormBuilder,
    private _permitService: PermitService,
    private _transferListService: TransferListService,
    private _confirmModalService:ConfirmModalService,
    private _notificationService: NotificationService,
    private _authenticationService: AuthenticationService ,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.tranData = this.data.data;
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.permit = this._permitService.getPermitByUser();

    switch (this.data.status) {
      case TRANSFER_SELL_STATUS_CODE.CHO_GHI_NHAN_BAN:
        this.userIsGDVDelete = true;
        this.userIsGDV = true;
        break;
      case TRANSFER_SELL_STATUS_CODE.CHO_KHOP_LENH_BAN:
        this.userIsGDV = true;
        break;
      case TRANSFER_SELL_STATUS_CODE.CHO_DUYET_TT_BAN:
        this.userIsKSVReject = true;
        this.userIsKSV = true;
        break;
      case TRANSFER_SELL_STATUS_CODE.CHO_DUYET_CHI_TIEN_BAN:
        this.userIsKSV = true;
        break;
    }

    // if(this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_GHI_NHAN_BAN) {
    //   this.userIsGDVDelete = true;
    // }

    // if(this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_GHI_NHAN_BAN || this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_KHOP_LENH_BAN) {
    //   this.userIsGDV = true;
    // }

    // if(this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_DUYET_TT_BAN) {
    //   this.userIsKSVReject = true;
    // }

    // if(this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_DUYET_TT_BAN || this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_DUYET_CHI_TIEN_BAN) {
    //   this.userIsKSV = true;
    // }
  }


  ngOnInit(): void {
    this._formControl();
  }

  get f() { return this.form.controls; }

  _formControl() {
    this.form = this.formBuilder.group({
      // THONG TIN TKTK/CCTG
      stk: [{ value: this.tranData.stk, disabled: true }],
      seri: [{ value: this.tranData.so_seri, disabled: true }],
      type_code: [{ value: this.tranData.loai_hinh, disabled: true },],
      balance: [{ value: this.tranData.so_du, disabled: true }],
      laisuattktt: [{ value: this.tranData.lai_suat, disabled: true }],
      tran_created: [{ value: moment(this.tranData.ngay_mo).format("DD/MM/YYYY"), disabled: true }],
      tran_expried: [{ value: moment(this.tranData.ngay_tai_ky).format("DD/MM/YYYY"), disabled: true }],
      payment_method: [{ value: this.data.sell_payment_method, disabled: true }],
      sell_payment_account_no: [{ value: this.data.sell_payment_account_no, disabled: true }],
      sell_fullname: [{ value: this.data.sell_fullname, disabled: true }],
      kpi_direct: [{ value: null, disabled: false },],
      kpi_indirect: [{ value: null, disabled: false },],
      transfer_date: [{ value: moment(this.data.transaction_date).format("DD/MM/YYYY"), disabled: true }],
      transfer_date_min: [{ value:moment(this.tranData.ngay_cn_gan_nhat).format("DD/MM/YYYY"), disabled: true }],
      hold_time: [{ value: this.tranData.so_ngay_nam_giu, disabled: true }],
      laisuat: [{ value: this.tranData.lai_suat, disabled: true }],
      money_expance: [{ value: this.tranData.lai_duoc_huong, disabled: true }],
      money_cost: [{ value: this.tranData.phi_chuyen_nhuong, disabled: true }],
      tranfer_value: [{ value: this.tranData.giatri_giao_dich, disabled: true }],
      tranfer_money_handle: [{ value: this.tranData.so_tien_kh_nhan_cn, disabled: true }],
      // THONG TIN CHU SO HUU
      cus_name: [{ value: this.tranData.ho_ten, disabled: true }],
      cus_cif: [{ value: this.tranData.ma_cif, disabled: true }],
      // THONG TIN TU VAN
      branch_name: [{ value: this.data.created_branch_code, disabled: true }],
      created_date: [{ value: moment(this.data.created_date).format("DD/MM/YYYY"), disabled: true }],

      ///REF
      ref: [{ value: '', disabled: true }],
      confirm_dgv: [{ value: '', disabled: true }],

      //thông tin nhân viên duyệt push code
      approved_user_full_name: [{ value: this.data.approved_user_full_name, disabled: true }],
      approved_date: [{ value: moment(this.data.approved_date).format("DD/MM/YYYY HH:mm"), disabled: true }],
      
      // DON VI NHAN CHUYEN NHUONGH
      // tranfer_branch: [{ value: '', disabled: true }],
      // tranfer_nvtv: [{ value: '', disabled: true }],
      // tranfer_gdv: [{ value: '', disabled: true }],
      // tranfer_ksv: [{ value: '', disabled: true }],

    

    });
  }
  onChangePaymentMethod(paymentMethod){
    if(paymentMethod.code == 'TM'){ //Tiền mặt
      // this.clearFormAccountOnline();
      this.isAnotherCustomer= false;
    }
    if(paymentMethod.code == 'CK'){ //Tài khoản thanh toán
      // this.getFormAccountOnline();
      this.isAnotherCustomer= true;
    }
  }
  onSubmit() {

  }
  public async onUpdate() {
    try {
      var title = this.data.status == TRANSFER_SELL_STATUS_CODE.CHO_GHI_NHAN_BAN ? 'gửi duyệt' : 'gửi duyệt chi tiền';
      this._confirmModalService.confirmModal(
        "Thông báo",
        `Bạn có chắc muốn ${title} lệnh chuyển nhượng này không?`,
        async () => {
          this.isLoading = true;
          const result = await this._transferListService.sendApproved(this.data.id);
          this.isLoading = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Gửi duyệt lệnh chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { 
          this.isLoading = false;
        }
      );
    } catch (error: any) {
      this.isLoading= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  public onDelete() {
   
    this._confirmModalService.confirmModal(
      "Thông báo",
      "Bạn có chắc muốn xóa lệnh chuyển nhượng này không?",
      () => {
        this.isLoadingDelete = true;
        this._transferListService.delete(this.data.id).then(
          rs => {
            this.isLoadingDelete = false;
            if (rs.code == "OK") {
              this._notificationService.success('Thông báo', 'Xóa lệnh chuyển nhượng thành công');
              this.closeFormDialog('detail');
            } else {
              this._notificationService.error('Thông báo', rs.message)
            }
          },
          error => {
            this.isLoadingDelete = false;
            this._notificationService.error('Thông báo', error.error.message)
          }
        )
      },
      () => {
        this.isLoadingDelete = false;
       }
    );
    this.isLoadingDelete = false;
  }
  public async onApproved() {
    try {
      
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn duyệt lệnh chuyển nhượng này không?",
        async () => {
          this.isLoadingApproved = true;
          const result = await this._transferListService.approved(this.data.id);
          this.isLoadingApproved = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Duyệt lệnh chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { 
          this.isLoadingApproved = false;
        }
      );
    } catch (error: any) {
      this.isLoadingApproved= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  public async onReject() {
    try {
     
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn từ chối lệnh chuyển nhượng này không?",
        async () => {
          this.isLoadingReject= true;
          const result = await this._transferListService.reject(this.data.id);
          this.isLoadingReject = false;
          if(result.code == "OK") {
            this._notificationService.success('Thành công', 'Từ chối lệnh chuyển nhượng thành công');
            this.closeFormDialog('detail');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => {
          this.isLoadingReject = false;
        }
      );
    } catch (error: any) {
      this.isLoadingReject= false;
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public onPrintContract() {
    try {
      this.isLoadingPrintContract = true;
      var id = '';
      this._transferListService.printContract(this.data.id).then(
        (result: Blob) => {
          this.isLoadingPrintContract = false;
          const blob = new Blob([result], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', });
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
  renderContentStatus(status){
    return (_find(TRANSFER_SELL_STATUS_LIST, {'code':status})).name;
  }
  renderClassStatus(status){
    return (_find(TRANSFER_SELL_STATUS_LIST, {'code':status})).color;
  }
  closeFormDialog(close: String) {
    this.dialogCheckRef.close(close);
  }
}
