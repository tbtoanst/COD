import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Permit } from '@app/_models';
import { List } from 'linq-typescript';

@Injectable({
  providedIn: 'root'
})
export class PermitService {

  constructor(private _router: Router) { }

  public getPermitByUser() {
    var menu = new List<any>(JSON.parse(localStorage.getItem('ORG_MENU') || '')).firstOrDefault(f => this._router.url === f.url && f.parent_id != null);

    if (menu !== null) {
      return {
        is_query: new List<any>(menu.permit).any(f =>  f.action === 'is_query'),
        is_add: new List<any>(menu.permit).any(f =>  f.action === 'is_add'),
        is_delete: new List<any>(menu.permit).any(f =>  f.action === 'is_delete'),
        is_send_approve: new List<any>(menu.permit).any(f =>  f.action === 'is_send_approve'),
        is_upload: new List<any>(menu.permit).any(f =>  f.action === 'is_upload'),
        is_approve: new List<any>(menu.permit).any(f =>  f.action === 'is_approve'),
        is_approve_upload: new List<any>(menu.permit).any(f =>  f.action === 'is_approve_upload'),
        is_reject: new List<any>(menu.permit).any(f =>  f.action === 'is_reject'),
        is_edit: new List<any>(menu.permit).any(f =>  f.action === 'is_edit'),
        is_print_contract: new List<any>(menu.permit).any(f =>  f.action === 'is_print_contract'),
        is_export: new List<any>(menu.permit).any(f =>  f.action === 'is_export')
      } as Permit;
    } else {
      return {} as Permit;
    }
  }
  
  // public checkRoleAccess(userName: string, branchCode: string, deptCode: string) {
  //   let isAccess = true;
  //   const currentUser = JSON.parse(localStorage.getItem('CURRENT_USER'));
  //   const roles = JSON.parse(localStorage.getItem('CURRENT_USER')).role as [];
  //   // Phòng Ban Hội Sở - Nhân viên/CV
  //   if (roles.filter(f => f == "C04406").length > 0) {
  //     if (branchCode != '000') {
  //       isAccess = false;
  //     }
  //     else if (deptCode != null && currentUser.positions.department.code != deptCode) {
  //       isAccess = false;
  //     }
  //     else if (userName != null && userName != currentUser.username) {
  //       isAccess = false;
  //     }
  //   }

  //   // Phòng Ban Hội Sở - CVC/.../GĐ
  //   if (roles.filter(f => f == "C04405").length > 0) {
  //     if (branchCode != '000') {
  //       isAccess = false;
  //     }
  //     else if (deptCode != null && currentUser.positions.department.code != deptCode) {
  //       isAccess = false;
  //     }
  //   }

  //    // Các đơn vị KD - KSV/GĐ DVKH/GĐ đơn vị
  //    if (roles.filter(f => f == "C04401").length > 0) {
  //     if (branchCode != currentUser.positions.branch.branch_code) {
  //       isAccess = false;
  //     }
  //   }

  //   // Các đơn vị KD - GDV
  //   if (roles.filter(f => f == "C04402").length > 0) {
  //     if (branchCode != currentUser.positions.branch.branch_code) {
  //       isAccess = false;
  //     }
  //     else if (userName != null && userName != currentUser.username) {
  //       isAccess = false;
  //     }
  //   }

   

  //   // PBHS KTT - Nhân Viên/CV
  //   // if (roles.filter(f => f == "C04403").length > 0) {
  //   //   if (branchCode == '000') {
  //   //     isAccess = false;
  //   //   }
  //   //   else if (userName != currentUser.username) {
  //   //     isAccess = false;
  //   //   }
  //   // }

  //   // PBHS KTT - CVC/...GĐ
  //   // if (roles.filter(f => f == "C04404").length > 0) {
  //   //   if (branchCode == '000') {
  //   //     isAccess = false;
  //   //   }
  //   //   else if (currentUser.positions.department.code != deptCode) {
  //   //     isAccess = false;
  //   //   }
  //   //   else if (userName != currentUser.username) {
  //   //     isAccess = false;
  //   //   }
  //   // }

  //   return isAccess;
  // }
}
