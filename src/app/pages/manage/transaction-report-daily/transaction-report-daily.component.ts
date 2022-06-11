import { Component, OnInit } from '@angular/core';
import { ConfirmModalService } from '@app/_components/confirm-modal/_services/confirm-modal.service';
import { NotificationService } from '@app/_components/notification/_services/notification.service';
import { TransactionReportService } from '@app/_services/transaction-report.service';

@Component({
  selector: 'app-transaction-report-daily',
  templateUrl: './transaction-report-daily.component.html',
  styleUrls: ['./transaction-report-daily.component.scss']
})
export class TransactionReportDailyComponent implements OnInit {
  public isBlock: boolean = false 
  
  constructor(
    private _confirmModalService:ConfirmModalService,
    private _transactionReportService:TransactionReportService,
    private _notificationService: NotificationService
  ) {
    
   }

  ngOnInit(): void {

    this.getStatusSystem();
  }
  public async getStatusSystem(){
    const res: any = await this._transactionReportService.getStatusTransaction({});
    if(res && res.data && res.data == "MO"){
      this.isBlock= true
    }
  }
  public lockTransaction(){
    let alert = "Bạn có chắc muốn khóa các giao dịch?";
    let alertResultSuccess = "Khóa các giao dịch thành công";
    let request = { status: "DONG"};
    if(!this.isBlock){
      alert = "Bạn có chắc muốn mở khóa các giao dịch?";
      alertResultSuccess = "Mở khóa các giao dịch thành công";
      request = { status: "MO"};
    }
    this._confirmModalService.confirmModal(
      "Thông báo",
      alert,
      async () => {
        try{
          const res: any = await this._transactionReportService.lockTransaction(request);
  
          if(res.code == 'OK'){
            this._notificationService.success('Thông báo', alertResultSuccess);
            this.isBlock = !this.isBlock;
          }
        }catch(error: any){
          this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
        }
      },
      () => {
        
      }
    );
  }
  public clearAllData(){
    this._confirmModalService.confirmModal(
      "Thông báo",
      "Bạn có chắc muốn xóa các giao dịch trên hệ thống?",
      async () => {
        const res: any = await this._transactionReportService.clearAllTransaction({});
        try{
        if(res.code == 'OK'){
          this._notificationService.success('Thông báo', 'Đã xóa các giao dịch trên hệ thống');
        }
      }catch(error: any){
        this._notificationService.error('Thông báo', (error.error?.message) ? error.error.message : error.message);
      }
      },
      () => {
        
      }
    );
  }
  classTransactionStatus(){
    if(this.isBlock){    
      return 'badge badge-success'
    }
    return 'badge badge-danger'

  }
  transactionStatus(){
    if(this.isBlock){
      return 'Các giao dịch trên hệ thống chưa khóa'
    }
    return 'Các giao dịch trên hệ thống đã khóa'
  }
}
