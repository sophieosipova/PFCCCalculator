import { IProductItem } from './productItem';

export interface IProduct{
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IProductItem[];
}


