
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IRecipeItem } from '../shared/recipeItem';
import { IRecipe } from '../shared/recipe';
import { IIngredientItem } from '../shared/ingredientItem';
import { IDishItem } from '../shared/dishItem';
import { IDish } from '../shared/dish';
import { IPFCCIngredientItem } from '../shared/pFCCIngredientItem';
import { IPager } from '../shared/pager.model';
import { IProduct } from '../shared/product';
import { IProductItem } from '../shared/productItem';

@Component({
  selector: 'recipe-data',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css'],
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `]
})




export class RecipeComponent {
  error: any;
  serverError: boolean = false;
  dish: IDishItem =  new IDishItem();
  recipe: IRecipeItem;
  ingredient: IIngredientItem = new IIngredientItem();
  ingredients: IIngredientItem[] = [];
  pfccIngredient: IPFCCIngredientItem;
  dishes: IDish;
  recipes: IRecipe;
  product: IProductItem = new IProductItem();
  productArr: IProductItem[];
  paginationInfo: IPager;
  http: HttpClient;
  baseUrl: string;
  productUrl: string;
  dishesUrl: string;
  tableMode: boolean = true;
  editMode: boolean = false;
  fullMode: boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.baseUrl = "https://localhost:44350/api/pfcccalculator/recipe";
    this.productUrl = 'https://localhost:44350/api/products/all';
    this.dishesUrl = 'https://localhost:44350/api/dishes';
    this.http = http;
  }


  ngOnInit() {
    this.getRecipes(3, 0);    
  }


  cancel() {

    this.product = new IProductItem();
    this.ingredient = new IIngredientItem();
    this.dish = new IDishItem();
    this.dish.ingredients = [];

    this.tableMode = true;
    this.editMode = false;

    this.fullMode = false;

  }


  add() {
    this.serverError = false;
    this.error = null;
    this.getProducts();
    this.cancel();
    this.tableMode = false;
  }


  save() {
    this.serverError = false;
    this.error = null;
    let url = this.dishesUrl + '/user/0/';
 
    if (this.dish.dishId == null) {
      this.http.post<IDishItem>(url, this.dish)
        .subscribe(result => {
          this.getRecipes(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
        }, error => { this.error = "Не корректные данные", console.error(error); });


      this.cancel();
    }
  }


  fullRecipe(r: IRecipeItem) {
    this.serverError = false;
    this.error = null;
    this.fullMode = true;
    this.recipe = r;
  }

  addIngredient() {
    this.serverError = false;
    this.error = null;
    this.ingredient.productName = this.productArr.find(IProductItem => IProductItem.productId == this.ingredient.productId).productName;
    this.dish.ingredients.push(this.ingredient);
    this.ingredient = new IIngredientItem();
  }

  cutRecipe(r: IRecipeItem) {
    this.serverError = false;
    this.error = null;
    this.fullMode = false;
    this.recipe = null;
  }



  deleteDish(r: IRecipeItem) {
    this.serverError = false;
    this.error = null;
    let url = 'https://localhost:44350/api/pfcccalculator' + '/user/0' + '/recipe/' + r.dishId;
    this.http.delete(url)
      .subscribe(result => {
        this.getRecipes(this.paginationInfo.itemsPage, this.paginationInfo.actualPage);
      }, error => { this.error = "Нельзя удалить", console.error(error); });

  }

  getRecipes(pageSize: number, pageIndex: number){
    this.serverError = false;
    this.error = null;
    let url = this.baseUrl + '?pageSize=' + pageSize + '&pageIndex=' + pageIndex;

    this.http.get<IRecipe>(url).subscribe(result => {
      this.recipes = result;
      this.paginationInfo = {
        actualPage: result.pageIndex,
        itemsPage: result.pageSize,
        totalItems: result.count,
        totalPages: Math.ceil(result.count / result.pageSize),
        items: result.pageSize
      }
    }, error => { this.serverError = true, console.error(error); });
  }


  getProducts() {
    this.serverError = false;
    this.error = null;
    let url = this.productUrl;

    this.http.get<IProductItem[]>(url).subscribe(result => {
      this.productArr = result;
    }, error => console.error(error));
  }

  onPageChanged(value: any) {
    this.serverError = false;
    this.error = null;
    event.preventDefault();
    this.paginationInfo.actualPage = value;
    this.getRecipes(this.paginationInfo.itemsPage, value);
  }

}
