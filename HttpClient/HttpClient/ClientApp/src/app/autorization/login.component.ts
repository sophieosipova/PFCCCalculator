import { Component, Inject, Injectable } from '@angular/core';

import { User } from '../shared/user';
import { UsersToken } from '../shared/usersToken';
import { HttpClient } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
//import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'login-data',
  templateUrl: './login.component.html'
})






export class LoginComponent {
  //error: any;


  //loginForm: FormGroup;
  submitClick = false;
  submitted = false;
  returnUrl: string;
  error = '';
  user: User;
  usersToken: UsersToken;
  http: HttpClient;
  authCode: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute,
    private router: Router) {

  //  this.baseUrl = "https://localhost:44350/api/products";
    this.http = http;

  }


  ngOnInit() {
    this.user = new User();
    this.usersToken = new UsersToken();
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    // this.getProducts(3, 0);
  }



  add() {

    let url = 'https://localhost:44350/api/autorization';

    this.http.post<UsersToken>(url, this.user)
      .subscribe(result => {
        this.usersToken = result;
        localStorage.setItem('TokenInfo', JSON.stringify(result));
        this.router.navigate([this.returnUrl]);
      }, error => { console.error(error); });


  }

  oauth() {

    let url = 'https://localhost:44350/api/autorization/oauth?client_id=app&redirect_uri=https://localhost:44323&response_type=code';

 //   http1: HttpClient();

    this.http.get(url)
      .subscribe(result => {
        this.authCode = result;
       // let url1 = $'https://localhost:44350/api/autorization/token?code={result}client_id=app&redirect_uri=https://localhost:44323&response_type=code';

      }, error => { console.error(error); });


  }


  logout() {
    localStorage.removeItem('TokenInfo');
  }

}
