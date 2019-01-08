import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IRecipeItem } from '../shared/recipeItem';
import { IRecipe } from '../shared/recipe';
import { IIngredientItem } from '../shared/ingredientItem';
import { IDishItem } from '../shared/dishItem';
import { IDish } from '../shared/dish';
import { IPFCCIngredientItem } from '../shared/pFCCIngredientItem';
import { IPager } from '../shared/pager.model';
//import { IProduct } from '../shared/product';
// import { IProductItem } from '../shared/productItem';

@Component({
  selector: 'recipe-data',
  templateUrl: './recipe.component.html',
})




export class RecipeComponent {

 

  dish: IDishItem;
  recipe: IRecipeItem;
  ingredient: IIngredientItem;
  pfccIngredient: IPFCCIngredientItem;
  dishes: IDish;
  recipes: IRecipe;
 // products: IProduct;
  //product: IProductItem;
  // productArr : IProductItem [];

  paginationInfo: IPager;
  http: HttpClient;
  baseUrl: string;
  tableMode: boolean = true;
  editMode: boolean = false;
  fullMode: boolean = false;

  //dataService: ProductsService;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.baseUrl = "https://localhost:44350/api/pfcccalculator/recipe";
    this.http = http;
  }


  ngOnInit() {
    this.getRecipes(3, 0);    // загрузка данных при старте компонента  
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
    this.dish = null;

    this.tableMode = true;
    this.editMode = false;

    this.fullMode = false;

  }

  /* delete(p: Concert) {
 
     this.dataService.deleteConcert(p.id)
 
       .subscribe(data => this.loadConcerts());
 
   }*/

  add() {

    this.cancel();
    this.tableMode = false;
  }


  save() {
    let url = this.baseUrl + '/user/0/'
    if (this.dish.dishId == null) {
      this.http.post<IDishItem>(url, this.dish)
        .subscribe(result => {
          this.getRecipes(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
        }, error => console.error(error));


      this.cancel();


    }
  }

  fullRecipe(r: IRecipeItem) {
    this.fullMode = true;
    this.recipe = r;
  }

  cutRecipe(r: IRecipeItem) {
    this.fullMode = false;
    this.recipe = null;
  }

  deleteDish(r: IDishItem) {


    let url = 'https://localhost:44350/api/dishes' + '/user/0' + '/product/';
    this.http.delete(url)
      .subscribe(result => {
        this.getRecipes(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
      }, error => console.error(error));

  }

  getRecipes(pageSize: number, pageIndex: number)/*: IProduct*/ {

    let url = this.baseUrl + '?pageSize=' + pageSize + '&pageIndex=' + pageIndex;


    /*this.http.get<IProduct>(url)
      .subscribe((data: IProduct) => this.products = data); */

    this.http.get<IRecipe>(url).subscribe(result => {
      this.recipes = result;
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
    this.getRecipes(this.paginationInfo.itemsPage, value);
  } 

}
