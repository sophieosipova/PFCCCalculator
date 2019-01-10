import { IPFCCIngredientItem } from './pFCCingredientItem';

export interface IRecipeItem {
   dishId: number;
   dishName: string;
   recipe: string;
   fat: number;
   protein: number;
   carbohydrates: number;
   calories: number;
   pFCCIngredients: IPFCCIngredientItem[];
   totalWeight: number;
}
