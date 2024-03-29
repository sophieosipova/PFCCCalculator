import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import { catchError } from 'rxjs/operators';

//import { AutorizationService } from '../autorization/autorization.service';
import { Router } from '@angular/router';
import { UsersToken } from '../shared/usersToken';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(/*private authenticationService: AutorizationService,*/ private http: HttpClient, private router: Router) { }

  intercept(request: HttpRequest<any>, newRequest: HttpHandler): Observable<HttpEvent<any>> {

    let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));

    request = request.clone({
      setHeaders: {
       // Authorization: `Bearer ${tokenInfo.accessToken}`,
       // 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8'
      }
    });
    return newRequest.handle(request).pipe(catchError(err => {

  /*    if (err.status == 200)
      {
        let u = err.message.substring(err.message.indexOf("https"), err.message.length);
       // this.router.navigate(u);
        window.location.href = u;
      } */
      if (err.status == 401 && tokenInfo != null)
      {
         localStorage.removeItem('TokenInfo');
        let url = 'https://localhost:44350/api/refreshtoken';
        
        this.http.post<UsersToken>(url, tokenInfo)
          .subscribe(result => {
            localStorage.setItem('TokenInfo', JSON.stringify(result));
          }, error => { console.error(error); });

      }

      if (err.status === 401) {
        this.router.navigate(['/login']);
        //localStorage.removeItem('TokenInfo');
      }

      const error = err.error.message || err.statusText;
      return Observable.throw(error);
    }));
  }
}
