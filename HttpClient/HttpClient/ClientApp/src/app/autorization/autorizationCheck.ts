import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
//import { request } from 'https';

@Injectable()
export class AuthorizationCheck implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    //If token data exist, user may login to application

  /*  if (localStorage.getItem('Get'))
    {
      localStorage.removeItem('Get');
      return true;
    }*/
    if (localStorage.getItem('TokenInfo'))
    {
      return true;
    }

    // otherwise redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
