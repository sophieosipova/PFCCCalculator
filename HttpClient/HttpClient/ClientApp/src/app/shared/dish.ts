import { IDishItem } from './dishItem';

export interface IDish{
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IDishItem[];
}
