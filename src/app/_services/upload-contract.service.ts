import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UploadContractService {

  constructor(private _http: HttpClient) { }

  public getList(request: any): Promise<any> {
    return this._http.get<any>(`${environment.apiUrl}/transaction`, { params: request }).toPromise()
  }

  public updateCompleteStatus(id: string): Promise<any> {
    return this._http.patch<any>(`${environment.apiUrl}/transaction/${id}`, {}).toPromise()
  }

  /////////////////Sell
  public uploadFileForSell(request: any, id: string): Promise<any> {
    return this._http.post<any>(`${environment.apiUrl}/sell/${id}/contract/upload`, request).toPromise()
  }

  public reUploadFileForSell(request: any, id: string): Promise<any> {
    return this._http.post<any>(`${environment.apiUrl}/sell/${id}/contract/reupload`, request).toPromise()
  }

  public deleteFileForSell( id: string): Promise<any> {
    return this._http.delete<any>(`${environment.apiUrl}/sell/${id}/contract`).toPromise()
  }

  public printContractForSell(id: string): Promise<Blob> {
    return this._http.get(`${environment.apiUrl}/sell/${id}/contract/download`, { responseType: 'blob' }).toPromise()
  }

  public approvedFileForSell(id: string): Promise<any> {
    return this._http.post<any>(`${environment.apiUrl}/sell/${id}/contract/approve`, {}).toPromise()
  } 

  ///////////Buy
  public printContractForBuy(id: string): Promise<Blob> {
    return this._http.get(`${environment.apiUrl}/buy/${id}/contract/download`, { responseType: 'blob' }).toPromise()
  }

  public uploadFileForBuy(request: any, id: string): Promise<any> {
    return this._http.post<any>(`${environment.apiUrl}/buy/${id}/contract/upload`, request).toPromise()
  }

  public reUploadFileForBuy(request: any, id: string): Promise<any> {
    return this._http.post<any>(`${environment.apiUrl}/buy/${id}/contract/reupload`, request).toPromise()
  }

  public approvedFileForBuy(id: string): Promise<any> {
    return this._http.post<any>(`${environment.apiUrl}/buy/${id}/contract/approve`, {}).toPromise()
  } 

  public deleteFileForBuy( id: string): Promise<any> {
    return this._http.delete<any>(`${environment.apiUrl}/buy/${id}/contract`).toPromise()
  }
}
