using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSI.Repositories
{
    public static class DateExtensions
    {
        public static DateTime SetFirstWorkingDay(this DateTime data)
        {
            data = new DateTime(data.Year, data.Month, 1);

            while (data.IsHoliday() || data.IsWeekend())
            {
                data = data.AddDays(1);
            }
            return data;
        }

        public static bool IsWeekend(this DateTime data)
        {
            return new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(data.DayOfWeek);
        }

        public static bool IsHoliday(this DateTime data)
        {
            var holidays = new List<string> { "0101", "0105", "0206", "0111" };

            return holidays.Contains(data.ToString("ddMM"));
        }
    }
}
