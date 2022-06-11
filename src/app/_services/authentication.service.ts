import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Tokens, User } from '@app/_models';
import { List } from 'linq-typescript';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly JWT_TOKEN = 'JWT_TOKEN';
  private readonly REFRESH_TOKEN = 'REFRESH_TOKEN';
  private readonly CURRENT_USER = 'CURRENT_USER';
  private readonly ORG_MENU = 'ORG_MENU';
  private readonly MENU = 'MENU';

  private _currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(
    private _http: HttpClient,
    private _router: Router
  ) {
    this._currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem(this.CURRENT_USER)!));
    this.currentUser = this._currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this._currentUserSubject.value;
  }

  public getJwtToken(): any {
    return localStorage.getItem(this.JWT_TOKEN);
  }

  public isLoggedIn(): boolean {
    return !!this.getJwtToken();
  }

  public verify(): any {
    return this._http.get<any>(`${environment.apiUrl}/account/verify`)
      .pipe(map(user => {
        let menus = new List<any>(user.data.menus).where(w => w.parent_id === null).toArray();

        menus.forEach(menu => {
          this._getChildrenMenu(menu, user.data.menus);
        });

        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem(this.CURRENT_USER, JSON.stringify(user.data.user_info));
        localStorage.setItem(this.ORG_MENU, JSON.stringify(user.data.menus));
        localStorage.setItem(this.MENU, JSON.stringify(menus));
        // store token
        this._storeTokens({ jwt: user.data.token, refresh_token: '' });

        this._currentUserSubject.next(user.data.user_info);
        //location.reload();
        return user;
      }, (error: any) => {
        console.log(error);
      }));
  }

  public login(username: string, password: string): any {
    return this._http.post<any>(`${environment.apiUrl}/account/login`, { username, password })
      .pipe(map(user => {
        let menus = new List<any>(user.data.menus).where(w => w.parent_id === null).toArray();
        menus.forEach(menu => {
          this._getChildrenMenu(menu, user.data.menus);
        });

        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem(this.CURRENT_USER, JSON.stringify(user.data.user_info));
        localStorage.setItem(this.ORG_MENU, JSON.stringify(user.data.menus));
        localStorage.setItem(this.MENU, JSON.stringify(menus));
        // store token
        this._storeTokens({ jwt: user.data.token, refresh_token: '' });

        this._currentUserSubject.next(user.data.user_info);
        return user;
      },(error: any) => {
        console.log(error);
      }));
  }

  public logout(): void {
    // remove user from local storage and set current user to null
    this._currentUserSubject.next(null!);
    this._removeStorage();
  }

  public refreshToken(): any {
    return this._http.get<any>(`${environment.apiUrl}/account/${this._getRefreshToken()}/refresh`)
      .pipe(tap((tokens: any) => {
        localStorage.setItem(this.JWT_TOKEN, tokens.jwt);
      }, error => {
        this.logout();
        this._router.navigate(['login']);
      }));
  }

  private _getRefreshToken() {
    return localStorage.getItem(this.REFRESH_TOKEN);
  }

  private _getChildrenMenu(menu: any, orgMenus: any): void {
    menu.menus = new List<any>(orgMenus).where(w => w.parent_id === menu.id).toArray();
    menu.menus.forEach((item: any) => {
      this._getChildrenMenu(item, orgMenus);
    });
  }

  private _storeTokens(tokens: Tokens): void {
    localStorage.setItem(this.JWT_TOKEN, tokens.jwt);
    localStorage.setItem(this.REFRESH_TOKEN, tokens.refresh_token);
  }

  private _removeStorage(): void {
    localStorage.removeItem(this.MENU);
    localStorage.removeItem(this.ORG_MENU);
    localStorage.removeItem(this.CURRENT_USER);
    localStorage.removeItem(this.JWT_TOKEN);
    localStorage.removeItem(this.REFRESH_TOKEN);
  }

}
