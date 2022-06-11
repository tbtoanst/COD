import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransferListService {

  constructor(private _http: HttpClient) { }

  public create(request: any): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/sell`, request).toPromise()
  }
  
  public update(id: string, request: any): Promise<any> {
    return this._http.patch<Response<any>>(`${environment.apiUrl}/sell/${id}`, request).toPromise()
  }

  public sendApproved(id: string): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/sell/${id}/send`, {}).toPromise()
  }
  
  public reject(id: string): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/sell/${id}/reject`, {}).toPromise()
  }

  public delete (id: string): Promise<any> {
    return this._http.delete<Response<any>>(`${environment.apiUrl}/sell/${id}`).toPromise()
  }

  public approved(id: string): Promise<any> {
    return this._http.post<Response<any>>(`${environment.apiUrl}/sell/${id}/approve`, {}).toPromise()
  }

  public printContract(id: string): Promise<Blob> {
    return this._http.post(`${environment.apiUrl}/sell/${id}/contract/preview`,{}, { responseType: 'blob' }).toPromise()
  }
}
