using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utils
{
    public class  DateUtils
    {
        public static DateTime GetMinDateTime()
        {
            return new DateTime(1800,01,01);
        }
        public static DateTime GetDateTimeNoWeekEnd(int timeSpanDay)
        {
            return GetDateTimeNoWeekEnd(DateTime.Today, timeSpanDay);
        }
        public static DateTime GetDateTimeNoWeekEnd(DateTime date, int timeSpanDay)
        {
            var day = date;
            var d = (int)day.DayOfWeek;
            if (d <= 0)
            {
                d = -2;
            }
            else if (d >= 6)
            {
                d = -1;
            }
            else {
                d = 0;
            }
            day = day.AddDays(timeSpanDay+d);

            return day;
        }
    }
}
