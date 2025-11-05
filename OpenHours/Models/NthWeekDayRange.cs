using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenHours.Models
{

    public class NthWeekDayRange
    {
        public NthWeekDayRange()
        {
            NthWeekDays = new List<NthWeekDay>();
        }
        public List<NthWeekDay> NthWeekDays { get; set; }

        public bool Includes(DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            int nthOfGivenDay = dateTime.Day % 7 == 0 ? (dateTime.Day / 7) : 1 + (dateTime.Day / 7);
            return NthWeekDays.Any(day => day.DayOfWeek == dayOfWeek && day.Occurence == nthOfGivenDay);
        }
    }
}
