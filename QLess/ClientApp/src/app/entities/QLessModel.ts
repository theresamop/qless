import { CardType } from "./enums";

export class QLessModel {
  id: number;
  value: number;
  expirationDate: Date;
  type: CardType;
  noOfUseToday: number;
  dateLastUsed: Date;
  active: boolean;
  isEntry: boolean;
  serialNo: string;
  discount: number;
}
