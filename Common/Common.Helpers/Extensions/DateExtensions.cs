using System;

namespace Common.Helpers.Extensions
{
    public static class DateExtensions
    {
        /// <summary>
        /// Returns a DateTime adjusted to the beginning of the week.
        /// </summary>
        /// <param name="dateTime">The DateTime to adjust</param>
        /// <returns>A DateTime instance adjusted to the beginning of the current week</returns>
        /// <remarks>the beginning of the week is controlled by the FirstDayOfWeek constant <see cref="Constants.FirstDayOfWeek"/></remarks>
        public static DateTime FirstDayOfWeek(this DateTime dateTime)
        {
            const DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;
            var offset = dateTime.DayOfWeek - FirstDayOfWeek < 0 ? 7 : 0;
            var numberOfDaysSinceBeginningOfTheWeek = dateTime.DayOfWeek + offset - FirstDayOfWeek;

            return dateTime.AddDays(-numberOfDaysSinceBeginningOfTheWeek);
        }

        /// <summary>
        /// Returns a DateTime adjusted to the beginning of the week.
        /// </summary>
        /// <param name="dateTime">The DateTime to adjust</param>
        /// <returns>A DateTime instance adjusted to the beginning of the current week</returns>
        /// <remarks>the beginning of the week is controlled by the FirstDayOfWeek constant <see cref="Constants.FirstDayOfWeek"/></remarks>
        public static DateTime LastDayOfWeek(this DateTime dateTime)
        {
            const int DaysToLastDay = 6;
            return dateTime.FirstDayOfWeek().AddDays(DaysToLastDay);
        }

        /// <summary>
        /// Returns the Start of the given day (the first millisecond of the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Returns the timezone-adjusted very end of the given day (the last millisecond of the last hour for the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime EndOfDay(this DateTime date)
        {
            const int EndOfDayHour = 23;
            const int EndOfDayMins = 59;
            const int EndOfDaySecs = 59;
            const int EndOfDayMsecs = 999;
            return new DateTime(date.Year, date.Month, date.Day, EndOfDayHour, EndOfDayMins, EndOfDaySecs, EndOfDayMsecs, date.Kind);
        }

        /// <summary>
        /// Returns the Start of the given hour (the first millisecond of the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime StartOfHour(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Returns the timezone-adjusted very end of the given hour (the last millisecond of the last minute for the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime EndOfHour(this DateTime date)
        {
            const int EndOfDayMins = 59;
            const int EndOfDaySecs = 59;
            const int EndOfDayMsecs = 999;
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, EndOfDayMins, EndOfDaySecs, EndOfDayMsecs, date.Kind);
        }
    }
}
