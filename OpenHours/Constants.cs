using System;
using System.Collections.Generic;

namespace OpenHours
{
    public static class Constants
    {
        public static readonly string _regexMonthRangePattern = @"^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b\s*";
        public static readonly string _regexMonthPattern = @"^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b\s*";
        public static readonly string _regexWeekDaysRange = @"^(Mo|Tu|We|Th|Fr|Sa|Su)-(Mo|Tu|We|Th|Fr|Sa|Su)\b\s*";
        public static readonly string _regexNthWeekDayInMonth = @"^(Mo|Tu|We|Th|Fr|Sa|Su)\[(\d+)\]\s*";
        public static readonly string _regexSpecificDayOfWeek = @"^(Mo|Tu|We|Th|Fr|Sa|Su)\b\s*";
        public static readonly string _regexSpecificDatesRange = @"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|[12][0-9]|3[01])\b\s*";
        public static readonly string _regexSpecificHoursRange = @"^(?:[01]\d|2[0-3]):[0-5]\d-(?:[01]\d|2[0-3]|24):[0-5]\d\b\s*";
        public static readonly string _regexSpecificDateRange = @"^(0[1-9]|[12][0-9]|3[01])\s*";

        public static readonly Dictionary<string, DayOfWeek> dayMap = new Dictionary<string, DayOfWeek>
        {
            { "Mo", DayOfWeek.Monday },
            { "Tu", DayOfWeek.Tuesday },
            { "We", DayOfWeek.Wednesday },
            { "Th", DayOfWeek.Thursday },
            { "Fr", DayOfWeek.Friday },
            { "Sa", DayOfWeek.Saturday },
            { "Su", DayOfWeek.Sunday }
        };


        public static readonly Dictionary<string, int> monthMap = new Dictionary<string, int> {
            {"Jan",1},
            {"Feb",2},
            {"Mar",3},
            {"Apr",4},
            {"May",5},
            {"Jun",6},
            {"Jul",7},
            {"Aug",8},
            {"Sep",9},
            {"Oct",10},
            {"Nov",11},
            {"Dec",12}
        };
    }
}
