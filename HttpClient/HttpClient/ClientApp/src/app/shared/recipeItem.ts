import { IIngredientItem } from './ingredientItem';

export interface IRecipeItem {
   dishId: number;
   dishName: string;
   recipe: string;
   fat: number;
   protein: number;
   carbohydrates: number;
   calories: number;
   pFCCIngredients: IIngredientItem[];
   totalWeight: number;
}
