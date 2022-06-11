import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransactionReportService {

  constructor(private _http: HttpClient) { }

  public clearAllTransaction(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/transaction/excecute_end_day`,{}).toPromise();
  }
  public lockTransaction(request: any): Promise<any> {
    return this._http.patch<Response<any>>(`${environment.apiUrl}/transaction/lock/TKGTCG`, request).toPromise();
  }
  public getStatusTransaction(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/transaction/lock`).toPromise();
  }
  public cleanBuyQuery(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/buy/clean/query`, { params: request }).toPromise();
  }
  public cleanSellQuery(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/sell/clean/query`, { params: request }).toPromise();
  }


}
