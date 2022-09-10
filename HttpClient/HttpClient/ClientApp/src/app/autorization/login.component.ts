import { Component, Inject, Injectable } from '@angular/core';

import { User } from '../shared/user';
import { UsersToken } from '../shared/usersToken';
import { HttpClient } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';


import { Observable } from 'rxjs';
import { retry, map, catchError } from 'rxjs/operators';
import { _throw as throwError } from 'rxjs/observable/throw';

//import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'login-data',
  templateUrl: './login.component.html',
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `]
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

   // localStorage.setItem('Login', "login");
   // if (localStorage.)
   localStorage.removeItem('TokenInfo');

  }


  ngOnInit() {

    this.user = new User();
    this.usersToken = new UsersToken();
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    localStorage.setItem('returnUrl', this.returnUrl);
    // this.getProducts(3, 0);
  }



  add() {

    let url = 'https://localhost:44350/api/autorization';
    

    this.http.post<UsersToken>(url, this.user)
      .subscribe(result => {
        this.usersToken = result;
        localStorage.setItem('TokenInfo', JSON.stringify(this.usersToken));
        this.router.navigate([this.returnUrl]);
      }, error => { /*console.error(error);*/ });


  }

 
  logout() {
    localStorage.removeItem('TokenInfo');
    this.router.navigate(['/login']);
  }

}
