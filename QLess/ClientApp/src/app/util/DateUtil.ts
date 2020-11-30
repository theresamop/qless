import { unitOfTime } from 'moment';



/**
 * Returns the number of days between dates
 * @param date1 - date lesser than date2
 * @param date2 - date greater than date1
 */
export function diffBetweenDates(date1: Date, date2: Date, granularity?: unitOfTime.StartOf): number {
  const d1 = new Date(date1);
  const d2 = new Date(date2);

  const yr = d2.getFullYear() - d1.getFullYear();
  const month = d2.getMonth() - d1.getMonth();
  const date = d2.valueOf() - d1.valueOf();

  return granularity === 'months' ? month : (granularity === 'years' ? yr : date )
}

export function diffMonthsFromPurchaseDate(date1: Date, date2: Date, granularity?: unitOfTime.StartOf): number {
  const d1 = new Date(date1);
  const d2 = new Date(date2);

  const yr = d2.getFullYear() - d1.getFullYear();
  const month = d2.getMonth() - d1.getMonth();
  const date = d2.valueOf() - d1.valueOf();
 
  const months = yr >= 1 ? (month + (12 * yr)) : month 
  return months
}
