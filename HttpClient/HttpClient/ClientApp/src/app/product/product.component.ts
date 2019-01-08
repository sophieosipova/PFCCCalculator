import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IProduct } from '../shared/product';
import { IProductItem } from '../shared/productItem';
import { IPager } from '../shared/pager.model';



@Component({
  selector: 'product-data',
  templateUrl: './product.component.html',
})




export class ProductsComponent {

  products: IProduct;
  product: IProductItem;
  nullProduct: IProductItem;
 // productArr : IProductItem [];

  paginationInfo: IPager;
  http: HttpClient;
  baseUrl: string;
  tableMode: boolean = true;
  editMode: boolean = false;

  //dataService: ProductsService;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.baseUrl = "https://localhost:44350/api/products";
    this.http = http;
  }


  ngOnInit() {
    this.getProducts(3,0);    // загрузка данных при старте компонента  
  }




  /*private extractData(res: Response) {
    if (res.status < 200 || res.status >= 300) {
      throw new Error('Bad response status: ' + res.status);
    }
    let body = res.json();
    return body.data || {};
  }
  */
  /*private handleError(error: any) {
    let errMsg = error.message || 'Server error';
    console.error(errMsg); // log to console instead 
    return throw(errMsg);
  }*/


  // сохранение данных



  cancel() {
  //  this.product.productId = null;
    this.product = this.nullProduct;

    this.tableMode = true;
    this.editMode = false;
  }

  /* delete(p: Concert) {
 
     this.dataService.deleteConcert(p.id)
 
       .subscribe(data => this.loadConcerts());
 
   }*/

  add(p: IProductItem) {
    
    this.cancel();
    this.tableMode = false;
  }


  save() {
    let url = this.baseUrl + '/user/0/'
    if (this.product.productId == null) {
      this.http.post<IProductItem>(url, this.product)
        .subscribe(result => {
          this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
        }, error => console.error(error));
       
    //  this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
    } else
    {
      this.http.put<IProductItem>(url, this.product)
        .subscribe(result => {
          this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);        
        }, error => console.error(error));

    //  this.http.put('https://localhost:44350/api/products/user/0', this.product);
     // this.http.put('https://localhost:44350/api/products/user/0/', this.product);
    }
   // this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
    this.cancel();


  }

  editProduct(p: IProductItem) {

    this.product = p;
    this.editMode = true;
  }

  deleteProduct(p: IProductItem) {

   
    let url = 'https://localhost:44350/api/pfcccalculator' + '/user/0' + '/product/' + p.productId;
    this.http.delete(url)
      .subscribe(result => {
        this.getProducts(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
      }, error => console.error(error));

  }

  getProducts(pageSize: number, pageIndex: number)/*: IProduct*/ {

    let url = this.baseUrl + '?pageSize=' + pageSize + '&pageIndex=' + pageIndex;


    /*this.http.get<IProduct>(url)
      .subscribe((data: IProduct) => this.products = data); */

    this.http.get<IProduct>(url).subscribe(result => {
      this.products = result;
      this.paginationInfo = {
        actualPage: result.pageIndex,
        itemsPage: result.pageSize,
        totalItems: result.count,
        totalPages: Math.ceil(result.count / result.pageSize),
        items: result.pageSize
      }
    }, error => console.error(error));
  }

  onPageChanged(value: any) {
    //console.log('catalog pager event fired' + value);
    event.preventDefault();
    this.paginationInfo.actualPage = value;
    this.getProducts(this.paginationInfo.itemsPage, value);
  }
 
  
}
