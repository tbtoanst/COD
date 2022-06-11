import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerInfoService {

  constructor(private _http: HttpClient) { }

  public getDetailByCIF(cif: string): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/customer/${cif}/get_cust_info_core`).toPromise()
  }

  public getAccountList(cif: string): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/customer/${cif}/get_cust_payment_account`).toPromise()
  }
}
