import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CreateReceiveTransferService {

  constructor(private _http: HttpClient) { }

  public getList(request: any): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/buy/sell/query`, { params: request }).toPromise()
  }

  public create(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/buy`, request).toPromise()
  }
}
