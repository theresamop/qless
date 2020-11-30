import { CardType } from "./enums";

export class QLessModel {
  id: number;
  value: number;
  type: CardType;
  noOfUseToday: number;
  dateLastUsed: Date;
  active: boolean;
  isEntry: boolean;
  serialNo: string;
  discount: number;
  purchaseDate: Date;

}
