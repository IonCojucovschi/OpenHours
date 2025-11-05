using System;
namespace OpenHours.Models
{
    public class WeekRange
    {
        public DayOfWeek StartDayOfWeek { get; set; }
        public DayOfWeek EndDayOfWeek { get; set; }

        public bool Includes(DayOfWeek dayOfWeek)
        {
            return NormalizeDay(StartDayOfWeek) <= NormalizeDay(dayOfWeek) && NormalizeDay(EndDayOfWeek) >= NormalizeDay(dayOfWeek);
        }

        private int NormalizeDay(DayOfWeek day) => day == DayOfWeek.Sunday ? 7 : (int)day;
    }
}
