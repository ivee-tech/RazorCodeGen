using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Calendar
{
    public static class DateTimeExtensions
    {

        public static DateTime? UnixToLocal(this DateTime? dateTime)
        {
            if (dateTime == null) return null;
            long ticks = dateTime.GetValueOrDefault().Ticks;
            DateTime unixEpoch = new DateTime(ticks, DateTimeKind.Utc);
            DateTime? d = unixEpoch.ToLocalTime();
            return d;
        }

        public static DateTime UnixToLocal(this DateTime dateTime)
        {
            long ticks = dateTime.Ticks;
            DateTime unixEpoch = new DateTime(ticks, DateTimeKind.Utc);
            DateTime d = unixEpoch.ToLocalTime();
            return d;
        }

        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
        }

        public static DateTime FirstDayInMonth(this DateTime dateTime, DayOfWeek day)
        {
            int y = dateTime.Year;
            int m = dateTime.Month;

            DateTime date;
            date = new DateTime(y, m, 1, System.Globalization.CultureInfo.CurrentCulture.Calendar);
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        public static DateTime LastDayInMonth(this DateTime dateTime, DayOfWeek day)
        {
            int y = dateTime.Year;
            int m = dateTime.Month;

            DateTime date;
            date = new DateTime(y, m, DateTime.DaysInMonth(y, m), System.Globalization.CultureInfo.CurrentCulture.Calendar);
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(-1);
            }
            return date;
        }
    }
}
