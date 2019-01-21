export class IProductItem {
  constructor(public productId?: number,
    public productsCategoryId?: number,
    public productName?: string,
    public fat?: number,
    public protein?: number,
    public carbohydrates?: number,
    public calories?: number,
    public userId?: string
  ) { }
}
