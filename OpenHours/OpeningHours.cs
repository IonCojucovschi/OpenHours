using OpenHours.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenHours
{
    public static class OpeningHours
    {

        /// <summary>
        /// Determines whether the specified UTC time falls within the defined business hours pattern.
        /// </summary>
        /// <param name="openHoursPattern">A string pattern representing business hours (e.g., "Mon-Fri 09:00-17:00; Mo[3] off; Nov 03 21:00-22:30;").</param>
        /// <param name="givenTime">The UTC time to evaluate. If null, the current UTC time is used.</param>
        /// <returns>True if the given UTC time is within business hours; otherwise, false.</returns>
        public static bool IsNowWithinBusinessHours(string openHoursPattern, DateTime? givenTime = null)
        {
            var time = givenTime ?? DateTime.UtcNow;
            // Split and classify patterns
            var timeRanges = openHoursPattern.Split(';').Select(el => el.Trim());

            var inclusiveRules = timeRanges.Where(r => !r.Contains("off")).Select(pattern => { var rule = Parce(pattern); rule.IsOpen(time); return rule; }).ToList();
            var exclusiveRules = timeRanges.Where(r => r.Contains("off")).Select(pattern => { var rule = Parce(pattern); rule.IsOpen(time); return rule; }).ToList();

            var maxSeverityLevel = inclusiveRules.Count > 0 ? inclusiveRules.Max(r => r.SeverityLevel) : 0;
            // Evaluate inclusive rules
            bool isInRange = inclusiveRules.Count == 0 || inclusiveRules
                .Where(r => r.SeverityLevel == maxSeverityLevel)
                .LastOrDefault()?.IsOpen(time) == true;

            // Evaluate exclusive rules
            bool isClosed = exclusiveRules.Count == 0 || exclusiveRules.All(r => r.IsOpen(time));

            return isInRange && isClosed;
        }

        public static TimeIntervalRule Parce(string openHoursPattern, TimeIntervalRule timeIntervalRule = null)
        {
            if (timeIntervalRule == null)
            {
                timeIntervalRule = new TimeIntervalRule();
            }


            openHoursPattern = openHoursPattern.Trim();

            if (openHoursPattern == "off")
            {
                timeIntervalRule.IsOff = true;
                return timeIntervalRule;
            }

            foreach (var (pattern, handler) in Patterns)
            {
                var match = Regex.Match(openHoursPattern, pattern);
                if (match.Success)
                {
                    return handler(match, timeIntervalRule, openHoursPattern);
                }
            }

            return timeIntervalRule;
        }

        private static (string pattern, Func<Match, TimeIntervalRule, string, TimeIntervalRule> handler)[] Patterns = new (string pattern, Func<Match, TimeIntervalRule, string, TimeIntervalRule> handler)[]
            {
                    // Month range: Jan-Feb
                    (Constants.RegexMonthRangePattern, (match, rule, input) =>
                    {
                        var parts = match.Value.Trim().Split('-');
                        rule.MonthRange = new MonthRange
                        {
                            StartMonth = Constants.MonthsMap[parts[0]],
                            EndMonth = Constants.MonthsMap[parts[1]]
                        };
                        return Recurse(input, match.Value, rule);
                    }),

                    // Single month: Jan
                    (Constants.RegexMonthPattern, (match, rule, input) =>
                    {
                        var month = match.Value.Trim();
                        rule.MonthRange = new MonthRange
                        {
                            StartMonth = Constants.MonthsMap[month],
                            EndMonth = Constants.MonthsMap[month]
                        };
                        return Recurse(input, match.Value, rule);
                    }),

                    // Weekday range: Mo-Th
                    (Constants.RegexWeekDaysRange, (match, rule, input) =>
                    {
                        var parts = match.Value.Trim().Split('-');
                        rule.WeekRange = new WeekRange
                        {
                            StartDayOfWeek = Constants.DaysMap[parts[0]],
                            EndDayOfWeek = Constants.DaysMap[parts[1]]
                        };
                        return Recurse(input, match.Value, rule);
                    }),

                    // Nth weekday: Mo[3]
                    (Constants.RegexNthWeekDayInMonth, (match, rule, input) =>
                    {
                        var day = match.Groups[1].Value;
                        var nth = int.Parse(match.Groups[2].Value);
                        rule.NthWeekDayRange = rule.NthWeekDayRange==null ? new NthWeekDayRange():rule.NthWeekDayRange;
                        rule.NthWeekDayRange.NthWeekDays.Add(new NthWeekDay
                        {
                            DayOfWeek = Constants.DaysMap[day],
                            Occurence = nth
                        });
                        return Recurse(input, match.Value, rule, true);
                    }),

                    // Single weekday: Mo
                    (Constants.RegexSpecificDayOfWeek, (match, rule, input) =>
                    {
                        var day = match.Value.Trim();
                        rule.WeekRange = new WeekRange
                        {
                            StartDayOfWeek = Constants.DaysMap[day],
                            EndDayOfWeek = Constants.DaysMap[day]
                        };
                        return Recurse(input, match.Value, rule);
                    }),

                    // Date range: 02-23
                    (Constants.RegexSpecificDatesRange, (match, rule, input) =>
                    {
                        if (int.TryParse(match.Groups[1].Value, out int start) && int.TryParse(match.Groups[2].Value, out int end))
                        {
                            rule.DateRange = new DatesRange { StartDate = start, EndDate = end };
                        }
                        return Recurse(input, match.Value, rule);
                    }),

                    // Time range: 02:30-23:20
                    (Constants.RegexSpecificHoursRange, (match, rule, input) =>
                    {
                        var parts = match.Value.Trim().Split('-');
                        var start = parts[0];
                        var end = parts[1] == "24:00" ? "23:59:59.999" : parts[1];
                        if (TimeSpan.TryParse(start, out var startTime) && TimeSpan.TryParse(end, out var endTime))
                        {
                            rule.TimeRange = new TimeRange { StartTime = startTime, EndTime = endTime };
                        }
                        return Recurse(input, match.Value, rule);
                    }),

                    // Single date: 02
                    (Constants.RegexSpecificDateRange, (match, rule, input) =>
                    {
                        if (int.TryParse(match.Groups[1].Value, out int date))
                        {
                            rule.DateRange = new DatesRange { StartDate = date, EndDate = date };
                        }
                        return Recurse(input, match.Value, rule);
                    })
            };

        private static TimeIntervalRule Recurse(string input, string matched, TimeIntervalRule rule, bool trimComma = false)
        {
            var next = Regex.Replace(input, Regex.Escape(matched), "").Trim();
            if (trimComma && next.StartsWith(",")) next = next.Substring(1).Trim();
            return string.IsNullOrEmpty(next) ? rule : Parce(next, rule);
        }
    }

}
