import { Component, OnInit } from '@angular/core';
import { environment } from '@environments/environment';
import axios from 'axios';
import { AuthenticationService } from './_services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'e-invoice';
  constructor(private _authenticationService: AuthenticationService){

  }

  ngOnInit(): void {
    axios.defaults.baseURL = environment.apiUrl;
    if (this._authenticationService.currentUserValue) {
      axios.defaults.headers.common['Authorization'] = `Bearer ${this._authenticationService.getJwtToken()}`;
      axios.defaults.headers.common['Content-Type'] = 'application/json; charset=utf-8';
    } else {
      axios.defaults.headers.common['Authorization'] = "";
    }
  }
}


