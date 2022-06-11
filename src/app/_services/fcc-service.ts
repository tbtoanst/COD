import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FccService {

  constructor(private _http: HttpClient) { }

  public getListAccountClass(): Promise<any> {
    return this._http.get<Response<any>>(`${environment.apiUrl}/fcc/get_list_account_class_fcc`).toPromise();
  }

}
