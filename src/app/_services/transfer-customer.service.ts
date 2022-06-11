import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransferCustomerService {

  constructor(private _http: HttpClient) { }

  public createTransferSell(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/sell`, request).toPromise();
  }
  public getTransferCustomer(cif: String): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/fcc/${cif}/get_account_list_fcc`).toPromise();
  }
  public getTransferSell(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/sell/query`, { params: request }).toPromise();
  }
  public getTransferBuy(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/buy/query`, { params: request }).toPromise();
  }

}
