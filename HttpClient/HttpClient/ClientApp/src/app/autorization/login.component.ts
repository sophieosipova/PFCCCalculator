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


   // this.submitted = true;

    //this.submitClick = true;


    let url = 'https://localhost:44350/api/autorization';

    this.http.post<User>(url, this.user)
      .subscribe(result => {
        this.usersToken = result;
        localStorage.setItem('TokenInfo', JSON.stringify(result));
        this.router.navigate([this.returnUrl]);
      }, error => { console.error(error); });



  }




}
