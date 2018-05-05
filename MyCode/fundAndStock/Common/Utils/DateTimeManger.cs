
namespace Common.Utils
{
    public enum DateInterval
    {
        Second, Minute, Hour, Day, Week, Month, Quarter, Year
    }

    public class DateTimeManger
    {
        public static long DateDiff(DateInterval interval, System.DateTime startDate, System.DateTime endDate)
        {
            long lngDateDiffValue = 0;
            var ts = new System.TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long)ts.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)ts.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)ts.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long)ts.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(ts.Days / 7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(ts.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((ts.Days / 30) / 3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(ts.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
    }
}
