export class IIngredientItem {
  constructor(
    public ingredientId?: number,
    public productId?: number,
    public dishId?: number,
    public productName?: string,
    public userId?: number,
    public count?: number
  ) { }
}
