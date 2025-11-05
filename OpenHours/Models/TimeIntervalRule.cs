using System;

namespace OpenHours.Models
{
    public class TimeIntervalRule
    {
        public DateTime GivenTime { get; set; }

        public MonthRange MonthRange { get; set; }

        public DatesRange DateRange { get; set; }

        public WeekRange WeekRange { get; set; }

        public NthWeekDayRange NthWeekDayRange { get; set; }

        public TimeRange TimeRange { get; set; }

        public bool IsExcludingRange { get; set; }

        public int SeverityLevel { get; set; }

        public bool IsOpen(DateTime? dateTime = null)
        {
            SeverityLevel = 0;

            if (dateTime.HasValue)
            {
                GivenTime = dateTime.Value;
            }

            if (GivenTime == default)
            {
                return IsExcludingRange;
            }

            bool Matches<T>(T range, Func<bool> condition)
            {
                if (range == null) return true;

                SeverityLevel++;
                if (condition())
                {
                    return true;
                }
                return false;
            }

            if (!Matches(MonthRange, () => MonthRange.Includes(GivenTime.Month))) return IsExcludingRange;
            if (!Matches(DateRange, () => DateRange.Includes(GivenTime.Day))) return IsExcludingRange;
            if (!Matches(WeekRange, () => WeekRange.Includes(GivenTime.DayOfWeek))) return IsExcludingRange;
            if (!Matches(NthWeekDayRange, () => NthWeekDayRange.Includes(GivenTime))) return IsExcludingRange;
            if (!Matches(TimeRange, () => TimeRange.Includes(GivenTime.TimeOfDay))) return IsExcludingRange;

            return !IsExcludingRange;
        }
    }

}
