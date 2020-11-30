using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLess
{
    public static class DateHelper
    {
        public static int CardAge(this DateTime puchaseDate)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - puchaseDate.Month;
            int years = today.Year - puchaseDate.Year;

            if (today.Day < puchaseDate.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }



            return years;
        }
    }
}
