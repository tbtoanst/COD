import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReceiveTransferListService {

  constructor(private _http: HttpClient) { }

  public getList(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/buy/query`, { params: request }).toPromise()
  }

  public create(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/buy`, request).toPromise()
  }
  
  public update(id: string, request: any): Promise<any> {
    return this._http.patch<Response<any>>(`${environment.apiUrl}/buy/${id}`, request).toPromise()
  }

  public sendApproved(id: string): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/buy/${id}/send`, {}).toPromise()
  }

  public reject(id: string): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/buy/${id}/reject`, {}).toPromise()
  }
  
  public delete(id: string): Promise<any> {
    return this._http.delete<Response<any>>(`${environment.apiUrl}/buy/${id}`).toPromise()
  }


  public approved(id: string): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/buy/${id}/approve`, {}).toPromise()
  }

  public printContract(id: string): Promise<Blob> {
    return this._http.post(`${environment.apiUrl}/buy/${id}/contract/preview`, {}, { responseType: 'blob' }).toPromise()
  }

  public getSeriNum(accountClass: string): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/bankpassbook/${accountClass}/get_bank_passbook_info`).toPromise()
  }

  public confirmSeriNumm(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/bankpassbook/push_bank_passbook_serial_no`, request).toPromise()
  }
}
