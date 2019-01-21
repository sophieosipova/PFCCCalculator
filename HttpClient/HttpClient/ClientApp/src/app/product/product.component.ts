import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IProduct } from '../shared/product';
import { IProductItem } from '../shared/productItem';
import { IPager } from '../shared/pager.model';

import { Pager } from '../shared/pager/pager';

@Component({
  selector: 'product-data',
  templateUrl: './product.component.html',
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `]
})




export class ProductsComponent {
  error: any;
  serverError: boolean = false;
  products: IProduct;
  product: IProductItem = new IProductItem();
  nullProduct: IProductItem;
  paginationInfo: IPager;
  http: HttpClient;
  baseUrl: string;
  tableMode: boolean = true;
  editMode: boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.baseUrl = "https://localhost:44350/api/products";
    this.http = http;

  }


  ngOnInit() {
    this.getProducts(3, 0);
  }



  cancel() {
    this.error = null;
    this.product = new IProductItem();

    this.tableMode = true;
    this.editMode = false;
  }


  add(p: IProductItem) {

    this.error = null;
    this.cancel();
    this.tableMode = false;
  }


  save() {

    this.error = null;
    let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));

    let url = this.baseUrl + `/user/${tokenInfo.userId}`;
    if (this.product.productId == null) {
      this.http.post<IProductItem>(url, this.product)
        .subscribe(result => {
          this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
        }, error => { this.error = "Не корректные данные", console.error(error); });

    } else {
      this.http.put<any>(url, this.product)
        .subscribe(result => {
          this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
        }, error => { this.error = "Не корректные данные", console.error(error); });

    }

    this.cancel();
  }

  editProduct(p: IProductItem) {
    this.error = null;
    this.product = p;
    this.editMode = true;
  }

  deleteProduct(p: IProductItem) {

    let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));
    this.error = null;
    let url = 'https://localhost:44350/api/pfcccalculator' + `/user/${tokenInfo.userId}` + '/product/' + p.productId;
    this.http.delete(url)
      .subscribe(result => {
        this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
      }, error => { this.error = "Некорректные данные ", console.error(error); });

  }

  getProducts(pageSize: number, pageIndex: number) {

    localStorage.setItem('Get', "true") ;
    this.serverError = false;
    this.error = null;
    let url = this.baseUrl + '?pageSize=' + pageSize + '&pageIndex=' + pageIndex;

    this.http.get<IProduct>(url).subscribe(result => {
      this.products = result;
      this.paginationInfo = {
        actualPage: result.pageIndex,
        itemsPage: result.pageSize,
        totalItems: result.count,
        totalPages: Math.ceil(result.count / result.pageSize),
        items: result.pageSize
      }
    }, error => { this.serverError = true, console.error(error); });

  }

  onPageChanged(value: any) {
    this.serverError = false;
    this.error = null;
    event.preventDefault();
    this.paginationInfo.actualPage = value;
    this.getProducts(this.paginationInfo.itemsPage, value);
  }


}
