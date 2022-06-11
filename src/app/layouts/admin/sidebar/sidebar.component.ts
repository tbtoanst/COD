import { Component, OnInit } from '@angular/core';

import { User } from '@app/_models';
import { AuthenticationService } from '@app/_services';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  private readonly MENU = 'MENU';

  public currentUser!: User;
  public menus: any[] = [];

  constructor(
    private _authenticationService: AuthenticationService,
  ) {
    this._authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit() {
    this.menus = JSON.parse(localStorage.getItem(this.MENU)  || '');
    // this.menus = [
    //   {
    //     "id": "1", 
    //     "parentid": null,
    //     "name": "Thông tin chuyển nhượng",
    //     "url": "/",
    //     "icon": "mdi mdi-crosshairs-gps menu-icon",
    //     "menus": [
    //       {
    //         "id": "1",
    //         "parentid": "1",
    //         "name": "Chuyển nhượng",
    //         "url": "/transfer-info/transfer-list",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       },
    //       {
    //         "id": "2",
    //         "parentid": "1",
    //         "name": "Nhận chuyển nhượng",
    //         "url": "/transaction-receive",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       },
    //       {
    //         "id": "3",
    //         "parentid": "1",
    //         "name": "Upload hợp đồng",
    //         "url": "/transaction-upload",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       },
    //       {
    //         "id": "4",
    //         "parentid": "1",
    //         "name": "Tra cứu lịch sử",
    //         "url": "/transaction-history",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       }
    //     ],
    //     "actionids": null,
    //     "actions": null,
    //     "permit": []
    //   },
    //   {
    //     "id": "2", 
    //     "parentid": null,
    //     "name": "Dịch vụ chuyển nhượng",
    //     "url": "",
    //     "icon": "mdi mdi-crosshairs-gps menu-icon",
    //     "menus": [
    //       {
    //         "id": "1",
    //         "parentid": "1",
    //         "name": "Quản trị giao dịch",
    //         "url": "/manager",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       },
    //       {
    //         "id": "2",
    //         "parentid": "1",
    //         "name": "Báo cáo & xử lý cuối ngày",
    //         "url": "/manager/report",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       },
    //       {
    //         "id": "3",
    //         "parentid": "1",
    //         "name": "Chỉ định ưu tiên",
    //         "url": "/manager/priority",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       },
    //       {
    //         "id": "4",
    //         "parentid": "1",
    //         "name": "Xử lý điều chỉnh giao dịch",
    //         "url": "/manager/adjust",
    //         "menus": [],
    //         "actionids": null,
    //         "actions": null,
    //         "permit": [
    //           {
    //             "id": "1",
    //             "parentid": null,
    //             "name": "Xem",
    //             "action": "isview"
    //           }
    //         ]
    //       }
    //     ],
    //     "actionids": null,
    //     "actions": null,
    //     "permit": []
    //   }
    // ];
  }

  ngAfterViewInit() {
    const sidebar = document.getElementsByClassName('sidebar')[0];
    sidebar?.addEventListener('show.bs.collapse', function (this: HTMLElement, ev: Event) {
      this.querySelector('.collapse.show')?.classList.remove('show');
    });
  }

  //Open submenu on hover in compact sidebar mode and horizontal menu mode
  mouseEnter(event: Event) {
    if(document.body.classList.contains('sidebar-icon-only')) {
      (event.target as HTMLElement).classList.add('hover-open');
    }      
  }

  mouseLeave(event: Event) {
    if(document.body.classList.contains('sidebar-icon-only')) {
      (event.target as HTMLElement).classList.remove('hover-open');
    }    
  }
}
