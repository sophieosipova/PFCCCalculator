import { IIngredientItem } from './ingredientItem';

export class IDishItem {
  constructor(
    public dishId?: number,
    public userId?: string,
    public dishName?: string,
    public recipe?: string,
   // public fat?: number,
 //   public protein?: number,
   // public carbohydrates?: number,
   // public calories?: number,
    public ingredients?: IIngredientItem[],
    public totalWeight?: number
  ) { }
}
