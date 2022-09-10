import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProductsComponent } from './product/product.component';
import { RecipeComponent } from './recipe/recipe.component';

import { Pager } from './shared/pager/pager';

import { LoginComponent } from './autorization/login.component';

import { httpInterceptor } from './interceptor/interceptor';
import { ErrorInterceptor } from './interceptor/errorInterceptor';

import { AuthorizationCheck } from './autorization/autorizationCheck';

import { ProductService } from './product/product.service';
import { OAUTHComponent} from './OAUTH';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ProductsComponent,
    RecipeComponent,
    Pager,
    LoginComponent,
    OAUTHComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
   //   { path: 'counter', component: CounterComponent  },
     // { path: 'fetch-data', component: FetchDataComponent  },
      { path: 'recipes', component: RecipeComponent, canActivate: [AuthorizationCheck] },
      { path: 'oauth', component: OAUTHComponent},
      { path: 'products', component: ProductsComponent, canActivate: [AuthorizationCheck]},
      { path: 'login', component: LoginComponent },
    ])
  ],

  exports: [

    Pager,

  ],

  providers: [
  { provide: HTTP_INTERCEPTORS, useClass: httpInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    AuthorizationCheck//, ProductService //, AutorizationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
