import { Component, Inject, Injectable } from '@angular/core';

//import { User } from './shared/user'; 
import { UsersToken } from './shared/usersToken';
import { HttpClient } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';


//import { Observable } from 'rxjs'; 
//import { retry, map, catchError } from 'rxjs/operators'; 
//import { _throw as throwError } from 'rxjs/observable/throw'; 

//import { FormBuilder, FormGroup, Validators } from '@angular/forms'; 
@Component({

  selector: 'oauth-data',
    templateUrl: './oauth.component.html'

})






export class OAUTHComponent {
  //error: any; 


  //loginForm: FormGroup; 
  submitClick = false;
  submitted = false;
  returnUrl: string;
  error = '';
  // user: User; 
  usersToken: UsersToken;
  http: HttpClient;
  authCode: string;

  constructor(http: HttpClient, private route: ActivatedRoute,
    private router: Router) {

    // this.baseUrl = "https://localhost:44350/api/products"; 
    this.http = http;
    this.returnUrl = localStorage.getItem('returnUrl');

  }


  ngOnInit() {
    //localStorage.setItem('Login', "login");
    // this.user = new User(); 
    this.usersToken = new UsersToken();
    this.authCode = this.route.snapshot.queryParams['code'];

    let url = 'https://localhost:44350/api/autorization/token?' + `code=${this.authCode}` + `&client_sercret=secret&client_id=app&redirect_uri=http://localhost:44323/`;

    this.http.get<UsersToken>(url)
      .subscribe(result => {
        this.usersToken = result;
        localStorage.setItem('TokenInfo', JSON.stringify(this.usersToken));
        this.router.navigate([this.returnUrl]);
      }, error => { console.error(error); });
    // this.getProducts(3, 0); 
  }



}
