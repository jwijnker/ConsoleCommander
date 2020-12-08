using System;
using System.Globalization;

namespace ConsoleCommander
{
    public static class GeneralHelpers
    {
        public static string StringLimit(this string s, int maxLength, string posttext = "...")
        {
            if (s == null || s.Length <= maxLength)
            {
                return s;
            }
            return $"{s.Substring(0, maxLength - posttext.Length)}{posttext}";
        }

        public static int GetWeeksInYear(int year)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = new DateTime(year, 12, 31);
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static string ToShortDateString(this Nullable<DateTime> date)
        {
            return (date.HasValue)
                ? date.Value.ToShortDateString()
                : null;
        }

        public static string GetMonthName(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        }
    }
}
