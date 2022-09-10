import { IRecipeItem } from './recipeItem';

export interface IRecipe {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IRecipeItem[];
}
