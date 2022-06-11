import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private _http: HttpClient) { }

  public getList(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/employee/get_emp_list_seller`, request).toPromise()
  }


}
