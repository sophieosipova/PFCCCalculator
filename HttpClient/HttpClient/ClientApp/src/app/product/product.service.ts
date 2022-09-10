import { Injectable } from '@angular/core';
import { Response } from '@angular/http';

import { DataService } from '../shared/services/data.service';

import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { IProduct } from '../shared/product';
import { IProductItem } from '../shared/productItem';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ProductService {
  private baseUrl: string = '';
  private http: HttpClient;


  constructor( http: HttpClient) {

    this.baseUrl = "https://localhost:44350/api/products";
    this.http = http;
  }

  get(pageIndex: number, pageSize: number, ): Observable<IProduct> {

    let url = this.baseUrl + '?pageIndex=' + pageIndex + '&pageSize=' + pageSize;
    return this.http.get<IProduct>(url);
  }
}

