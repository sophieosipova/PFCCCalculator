import { IIngredientItem } from './ingredientItem';

export interface IDishItem {
  dishId: number;
  dishName: string;
  recipe: string;
  fat: number;
  protein: number;
  carbohydrates: number;
  calories: number;
  data: IIngredientItem[];
  totalWeight: number;
}
