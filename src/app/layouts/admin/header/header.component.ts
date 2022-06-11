import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '@app/_services';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(
    private _authenticationService: AuthenticationService,
    private _router: Router
  ) { }

  ngOnInit(): void {
  }

  logout() {
    this._authenticationService.logout();
    this._router.navigate(['login']);
  }

  toggleMobileNav() {
    document.getElementById('sidebar')?.classList.toggle('active');
  }

  toggleMinimize() {
    if ((document.body.classList.contains('sidebar-toggle-display')) || (document.body.classList.contains('sidebar-absolute'))) {
      document.body?.classList.toggle('sidebar-hidden');
    } else {
      document.body?.classList.toggle('sidebar-icon-only');
    }
  }
}
