import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { ConfirmModalService } from '@app/_components/confirm-modal/_services/confirm-modal.service';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { CONTRACT_STATUS_LIST, TRANSFER_TYPE_LIST, UPLOAD_CONTRACT_STATUS_CODE } from '@app/_constant';
import { Permit, User } from '@app/_models';
import { AuthenticationService, PermitService } from '@app/_services';
import { UploadContractService } from '@app/_services/upload-contract.service';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-upload-contract',
  templateUrl: './upload-contract.component.html',
  styleUrls: ['./upload-contract.component.scss']
})
export class UploadContractComponent implements OnInit {
  @ViewChild('importFile') importFile: ElementRef;
  public currentUser!: User;
  public pageEvent: PageEvent = {
    pageIndex: 0,
    pageSize: 10,
    length: 0
  };
  public form!: FormGroup;
  public submitted: boolean = false;
  public isLoading: boolean = false;
  public isLoadingUploadFile: boolean = false;
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
  public statusList: any[] = [];
  public transferTypeList: any[] = TRANSFER_TYPE_LIST;
  public contractStatusList: any[] = CONTRACT_STATUS_LIST;
  public uploadContractStatusCode: any = UPLOAD_CONTRACT_STATUS_CODE;
  public dialogRef: any;
  public fileToImport!: File;
  
  constructor(
    private _uploadContractService: UploadContractService,
    private _permitService: PermitService,
    private _authenticationService: AuthenticationService,
    private _notificationService: NotificationService,    
    private _confirmModalService: ConfirmModalService,
    private _formBuilder: FormBuilder,
    private spinner: NgxSpinnerService
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
      branch_code: [{ value: this.currentUser.positions.branch.code, disabled: true }],
      from_date: [{ value: '', disabled: false }],
      to_date: [{ value: '', disabled: false }],
      contract_num: [{ value: '', disabled: false }],
      trans_type: [{ value: '', disabled: false }],
      status_code: [{ value: '', disabled: false }]
    });
  }

  private _getFormRequest() {
    const request = {
      branch_code: this.f['branch_code'].value,
      from_date: this.f["from_date"].value ?? moment(this.f["from_date"].value).format("YYYY-MM-DD"),
      to_date: this.f["to_date"].value ?? moment(this.f["to_date"].value).format("YYYY-MM-DD"),
      contract_num: this.f['contract_num'].value,
      trans_type: this.f['trans_type'].value,
      status_code: this.f['status_code'].value,
      page_num: this.pageEvent.pageIndex,
      page_size: this.pageEvent.pageSize
    }
    return request;
  }

  private async _getList(request: any) {
    try {
      this.dataList = [];
      this.isLoading = true;
      this.spinner.show();
      const res: any = (await this._uploadContractService.getList(request)).data;
      this.dataList = res.result;
      console.log(this.dataList);
      this.pageEvent.length = res.count;
      this.isLoading = false;
      this.spinner.hide()
    } catch (error: any) {
      this.spinner.hide()
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

  public changeLayoutUploading(event){
    let parentClassFormUpload = event.target.parentNode.parentNode.parentNode
    let classAddFile = parentClassFormUpload.querySelector('.add-file');
    let classPlaceholder = parentClassFormUpload.querySelector('.placeholder-loading');
    classAddFile.classList.add( "hidden" )
    classPlaceholder.classList.remove( "hidden" )
  }
  public changeLayoutUploadError(event){
    let parentClassFormUpload = event.target.parentNode.parentNode.parentNode
    let classAddFile = parentClassFormUpload.querySelector('.add-file');
    let classPlaceholder = parentClassFormUpload.querySelector('.placeholder-loading');
    classAddFile.classList.add( "hidden" )
    classPlaceholder.classList.remove( "hidden" )
  }
   public async onChangeUploadFile(event: Event, type: string, id: string ) {
     this.changeLayoutUploading(event)
    try{
      const formData: FormData = new FormData();
      formData.append("file", (event.target as HTMLInputElement).files?.item(0) as File);
      if(type == 'SELL') {
          await this._uploadContractService.uploadFileForSell(formData, id);
      } 
      if(type == 'BUY') {
          await this._uploadContractService.uploadFileForBuy(formData, id);
      }
      this._notificationService.success('Thông báo', "Upload hợp đồng thành công");

      const res: any = (await this._uploadContractService.getList(this._getFormRequest())).data;
      this.dataList = res.result;
    }catch(error :any){
      this.changeLayoutUploadError(event)
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }
  public changeLayoutDeleting(event){
    let parentClassDelete = event.target.parentNode.parentNode.parentNode;
    console.log(parentClassDelete);
    let classDeleteFile = parentClassDelete.querySelector('.form-delete');
    let classPlaceholder = parentClassDelete.querySelector('.placeholder-loading');
    classDeleteFile.classList.add( "hidden" )
    classPlaceholder.classList.remove( "hidden" )
  }
  public changeLayoutDeletingError(event){
    let parentClassDelete = event.target.parentNode.parentNode.parentNode;
    console.log(parentClassDelete);
    let classDeleteFile = parentClassDelete.querySelector('.form-delete');
    let classPlaceholder = parentClassDelete.querySelector('.placeholder-loading');
    classDeleteFile.classList.add( "hidden" )
    classPlaceholder.classList.remove( "hidden" )
  }
  public async onDelete(event: Event, type: string, id: string){
    this._confirmModalService.confirmModal(
      "Thông báo",
      "Bạn có chắc xóa file này không?",
      async () => {
        try{
          this.changeLayoutDeleting(event)
          if(type == 'SELL') {
                await this._uploadContractService.deleteFileForSell(id);
          } 
          if(type == 'BUY') {
                await this._uploadContractService.deleteFileForBuy(id);
          }
          this._notificationService.success('Thông báo', "Đã xóa file");

          const res: any = (await this._uploadContractService.getList(this._getFormRequest())).data;
          this.dataList = res.result;
        }catch(error :any){
          this.changeLayoutDeletingError(event)
          this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
        }
      },
      () => { }
    );
  }
  public onApprovedCN(sellId: string) {
    try {
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn duyệt hợp đồng chuyển nhượng này không?",
        async () => {
          const result = await this._uploadContractService.approvedFileForSell(sellId);
          if(result.code == "OK") {
            await this._getList(this._getFormRequest());
            this._notificationService.success('Thành công', 'Duyệt hợp đồng chuyển nhượng thành công');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { }
      );
    } catch (error) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public onApprovedNCN(buyId: string) {
    try {
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn duyệt hợp đồng nhận chuyển nhượng này không?",
        async () => {
          const result = await this._uploadContractService.approvedFileForBuy(buyId);
          if(result.code == "OK") {
            await this._getList(this._getFormRequest());
            this._notificationService.success('Thành công', 'Duyệt hợp đồng nhận chuyển nhượng thành công');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { }
      );
    } catch (error) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public showTextUploadStatus(code: string) {
    if(code) {
      return this.uploadContractStatusCode.CHO_DUYET == code ? 'Chờ duyệt' : 'Đã duyệt';
    }
    return '';
  }

  public onDownload(type: string, id: string) {
    try {
      switch (type) {
        case "SELL":
          this._uploadContractService.printContractForSell(id).then(
            (result: Blob) => {
              const blob = new Blob([result], { type: 'application/pdf' });
              const url = window.URL.createObjectURL(blob);
              window.open(url, '_blank');
            },
            error => {
              this._notificationService.error('Thông báo', (error.error.message) ? error.error.message : error.message);
            }
          );
          break;
        case "BUY":
          this._uploadContractService.printContractForBuy(id).then(
            (result: Blob) => {
              const blob = new Blob([result], { type: 'application/pdf' });
              const url = window.URL.createObjectURL(blob);
              window.open(url, '_blank');
            },
            error => {
              this._notificationService.error('Thông báo', (error.error.message) ? error.error.message : error.message);
            }
          );
          break;
      }      
    } catch (error) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public onCompleteTrans(id: string) {
    try {
      this._confirmModalService.confirmModal(
        "Thông báo",
        "Bạn có chắc muốn hoàn tất hợp đồng này không?",
        async () => {
          const result = await this._uploadContractService.updateCompleteStatus(id);
          if(result.code == "OK") {
            await this._getList(this._getFormRequest());
            this._notificationService.success('Thành công', 'Hoàn tất hợp đồng thành công');
          } else {
            this._notificationService.error('Thông báo', result.message);
          }
        },
        () => { }
      );
    } catch (error) {
      this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
    }
  }

  public checkBtnComplete(item: any) {
    if(item.buy.buy_contract && item.buy.buy_contract?.status == UPLOAD_CONTRACT_STATUS_CODE.DA_DUYET && item.sell.sell_contract && item.sell.sell_contract?.status == UPLOAD_CONTRACT_STATUS_CODE.DA_DUYET) {
      return true;
    }
    return false;
  }

  public setPaginatorData(page: PageEvent) {
    this.pageEvent = page;
    this._getList(this._getFormRequest());
  }

  public trigerClickInputUpload(event) {
    let parentClassFormUpload = event.target.parentNode.parentNode.parentNode
    let fileUpload = parentClassFormUpload.querySelector('.file-upload');
    fileUpload.click();
  }

  public viewFile(file_id, name){

  }
  public deleteFile(file_id, name){

  }
}
