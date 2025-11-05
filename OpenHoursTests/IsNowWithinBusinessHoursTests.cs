using NUnit.Framework;
using OpenHours;
using System;

namespace OpenHoursTests
{
    [TestFixture]
    public class OpeningHoursTests_Pattern
    {
        [TestCase("Mo-Th 08:00-20:00", "2025-01-15 09:00", true)] // Inside inclusive range
        [TestCase("Mo-Th 08:00-20:00;Fr off", "2025-01-15 09:00", true)] // Wednesday, not excluded
        [TestCase("Mo-Th 08:00-20:00;We off", "2025-01-15 09:00", false)] // Wednesday, excluded → false
        [TestCase("Mo-Th 08:00-20:00;We off", "2025-01-15 07:00", false)] // Outside inclusive range
        [TestCase("Mo-Th 08:00-20:00;We off", "2025-01-15 09:00", false)] // Inside inclusive but excluded

        [TestCase("Aug 20;Aug 21 off", "2025-08-20", true)] // Is included but not excluded
        [TestCase("Aug 20;Aug 20 off", "2025-08-20", false)] // Included and excluded ////excluded operation is primary
        [TestCase("Aug 20;Aug 20 off;Aug 21 off", "2025-08-20", false)] // Included and all exclusions match
        [TestCase("Aug 20;Aug 20 off;Aug 21 off", "2025-08-21", false)] // Not in inclusive range

        [TestCase("Fr[2] 08:00-10:00;Fr[2] off", "2025-10-10 08:30", false)] // 2nd Friday, match hours interval but excluded day
        [TestCase("Fr[2] 08:00-10:00;Fr[2] off", "2025-10-10 07:30", false)] // 2nd Friday, outside hour
        [TestCase("Fr[2] 08:00-10:00;Fr[3] off", "2025-10-10 08:30", true)] // In included and in excluded interval

        [TestCase("08:00-10:00;Mo-Th off", "2025-01-15 08:30", false)] // Wednesday in hour range but in excluded week days
        [TestCase("08:00-10:00;Fr off", "2025-01-15 08:30", true)] // Wednesday not excluded
        [TestCase("08:00-10:00;We off", "2025-01-15 08:30", false)] // Wednesday excluded → false
        [TestCase("08:00-10:00;We off", "2025-01-15 07:30", false)] // Outside hour range
        [TestCase("00:00-24:00;", "2025-01-15 07:30", true)] //Whole Day
        [TestCase("10:00-08:00;Fr off", "2025-10-17 07:30", false)] // Friday is not included
        [TestCase("00:00-24:00", "2025-10-08 00:00", true)]
        [TestCase("23:00-02:00;Sa off", "2025-10-11 23:30", false)]
        [TestCase("Mo-Su 03:00-23:00; Oct 13 off; Nov 03 off; Nov 24 off; Dec 31 off; Jan 01-04 off", "2025-11-24 22:30", false)]
        [TestCase("Mo-Su 03:00-23:00; Oct 13 off; Nov 03 off; Nov 24 off; Dec 31 off; Jan 01-04 off", "2025-12-31 21:30", false)]
        [TestCase("Mo-Su 03:00-23:00; Oct 13 off; Nov 03 off; Nov 24 off; Dec 31 off; Jan 01-04 off", "2026-01-05 21:30", true)]
        [TestCase("Mo-Su 03:00-23:00; Oct 13 off; Nov 03 off; Nov 24 off; Dec 31 off; Jan 01-04 off", "2025-12-30 22:59", true)]
        [TestCase("Mo-Su 03:00-23:00; Oct 13 off; Nov 03 off; Nov 24 off; Dec 31 off; Jan 01-04 off", "2026-01-02 06:59", false)]

        [TestCase("Mo-Su 03:00-22:59; Mo[1],Mo[3] 06:15-22:59;", "2025-10-20 03:30", false)]
        [TestCase("Mo-Su 03:00-22:59; Mo[1],Mo[3] 06:15-22:59;", "2025-10-27 03:30", true)]
        [TestCase("Mo-Su 03:00-22:59; Mo[1],Mo[3] 06:15-22:59;", "2025-10-20 06:30", true)]
        [TestCase("Mo-Su 03:00-04:00; Mo[1],Mo[3] 06:15-22:59;", "2025-10-21 03:30", true)]
        [TestCase("Mo-Su 03:00-04:00; Mo[1],Mo[3],Th[2] 06:15-22:59;", "2025-10-09 03:30", false)]
        [TestCase("Mo-Su 03:00-04:00; Mo[1],Mo[3],Th[2] 06:15-22:59;", "2025-10-09 06:30", true)]
        [TestCase("Mo-Su 03:00-04:00; Mo[1],Mo[3],Th[2] 06:15-22:59;", "2025-10-09 06:14", false)]

        [TestCase("Mo-Su 00:00-23:59; Th[1] 00:00-22:00; Fr[1] 03:00-23:59;", "2025-11-06 21:59", true)]
        [TestCase("Mo-Su 00:00-23:59; Th[1] 00:00-22:00; Fr[1] 03:00-23:59;", "2025-11-06 22:00", false)]
        [TestCase("Mo-Su 00:00-23:59; Th[1] 00:00-22:00; Fr[1] 03:00-23:59;", "2025-11-07 02:11", false)]
        [TestCase("Mo-Su 00:00-23:59; Th[1] 00:00-22:00; Fr[1] 03:00-23:59;", "2025-11-07 03:11", true)]

        [TestCase("Mo-Su 00:00-23:59; Nov 06 22:00-00:00 off; Nov 07 00:00-03:00 off;", "2025-11-06 21:59", true)]
        [TestCase("Mo-Su 00:00-23:59; Nov 06 22:00-00:00 off; Nov 07 00:00-03:00 off;", "2025-11-06 22:00", false)]
        [TestCase("Mo-Su 00:00-23:59; Nov 06 22:00-00:00 off; Nov 07 00:00-03:00 off;", "2025-11-07 02:11", false)]
        [TestCase("Mo-Su 00:00-23:59; Nov 06 22:00-00:00 off; Nov 07 00:00-03:00 off;", "2025-11-07 03:11", true)]


        public void CheckMultipleRanges(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var result = OpeningHours.IsNowWithinBusinessHours(pattern, dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }

        [TestCase("Mo-Th 08:00-20:00;", "2025-01-15 09:00", true)] // Inside inclusive range
        [TestCase("Mo-Th 08:00-20:00;Fr off;", "2025-01-15 09:00", true)] // Wednesday, not excluded
        [TestCase("Mo-Th 08:00-20:00;We off;", "2025-01-15 09:00", false)] // Wednesday, excluded → false
        [TestCase("Mo-Th 08:00-20:00;We off;", "2025-01-15 07:00", false)] // Outside inclusive range
        [TestCase("Mo-Th 08:00-20:00;We off;", "2025-01-15 09:00", false)] // Inside inclusive but excluded

        [TestCase("Aug 20;Aug 21 off;", "2025-08-20", true)] // Is included but not excluded
        [TestCase("Aug 20;Aug 20 off;", "2025-08-20", false)] // Included and excluded ////excluded operation is primary
        [TestCase("Aug 20;Aug 20 off;Aug 21 off;", "2025-08-20", false)] // Included and all exclusions match
        [TestCase("Aug 20;Aug 20 off;Aug 21 off;", "2025-08-21", false)] // Not in inclusive range

        [TestCase("Fr[2] 08:00-10:00;Fr[2] off;", "2025-10-10 08:30", false)] // 2nd Friday, match hours interval but excluded day
        [TestCase("Fr[2] 08:00-10:00;Fr[2] off;", "2025-10-10 07:30", false)] // 2nd Friday, outside hour

        [TestCase("08:00-10:00;Mo-Th off;", "2025-01-15 08:30", false)] // Wednesday in hour range but in excluded week days
        [TestCase("08:00-10:00;We off;", "2025-01-15 08:30", false)] // Wednesday excluded → false
        [TestCase("08:00-10:00;We off;", "2025-01-15 07:30", false)] // Outside hour range
        [TestCase("Mo-Su 03:00-23:59;", "2025-10-13 07:30", true)] // Monday
        [TestCase("Su-Mo 03:00-23:59;", "2025-10-13 07:30", false)] // Monday, set wrong pattern

        public void CheckMultipleRangesAndTimeZone(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var result = OpeningHours.IsNowWithinBusinessHours(pattern, dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }

        [TestCase("Mo-Th 08:00-20:00;Fr off", "2025-10-15 09:00", true)]
        [TestCase("Mo-Th 08:00-20:00;We off", "2025-10-15 09:00", false)]
        [TestCase("Mo-Th 08:00-20:00;Jul We off;Oct We off", "2025-10-15 09:00", false)]
        [TestCase("Mo-Th 08:00-20:00;Jul We off", "2025-10-15 09:00", true)]
        public void CheckSameConditionButDifferentTime(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var result = OpeningHours.IsNowWithinBusinessHours(pattern, dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [TestFixture]
    public class OpeningHoursEdgeCaseTests_Pattern
    {
        [TestCase("00:00-24:00;We off", "2025-10-15 12:00", false)] // Full day open but Wednesday excluded
        [TestCase("00:00-24:00;We off", "2025-10-16 12:00", true)] // Thursday allowed
        [TestCase("00:00-24:00;", "2025-10-08 23:59", true)] // Still open full day in Tokyo
        [TestCase("00:00-24:00 off;", "2025-10-08 12:00", false)] // Off full day
        [TestCase("23:00-24:00;", "2025-10-08 13:59", false)] // Before window
        public void CheckMidnightAndFullDayRangesWithTZ(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var result = OpeningHours.IsNowWithinBusinessHours(pattern, dateTime);
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class OpeningHours_EdgeCases_00_24_Ranges_Tests_Pattern
    {
        // --- Pure Midnight Boundaries ---
        [TestCase("00:00-00:00", "2025-10-08 00:00", false)] // 0-length = closed
        [TestCase("00:00-00:00 off", "2025-10-08 00:00", true)] // inverted version = open

        [TestCase("00:00-12:00", "2025-10-08 00:00", true)] // opens at start of day
        [TestCase("00:00-12:00", "2025-10-08 11:59", true)]
        [TestCase("00:00-12:00", "2025-10-08 12:00", false)] // exactly closing boundary

        [TestCase("12:00-00:00", "2025-10-08 12:00", true)] // open until midnight
        [TestCase("12:00-00:00", "2025-10-08 23:59", true)]
        [TestCase("12:00-00:00", "2025-10-09 00:00", false)] // rolls to next day

        // --- 24:00 Boundaries ---
        [TestCase("00:00-24:00", "2025-10-08 00:00", true)] // full day open
        [TestCase("00:00-24:00", "2025-10-08 23:59", true)]
        [TestCase("00:00-24:00", "2025-10-09 00:01", true)]

        [TestCase("06:00-24:00", "2025-10-08 06:00", true)] // open morning to end of day
        [TestCase("06:00-24:00", "2025-10-08 23:59", true)]
        [TestCase("06:00-24:00", "2025-10-09 00:00", false)]

        [TestCase("00:00-06:00", "2025-10-08 01:00", true)]
        [TestCase("00:00-06:00 off", "2025-10-08 01:00", false)]

        // --- Cross-Day / Month Boundaries ---
        [TestCase("23:00-01:00", "2025-10-31 23:30", true)] // end of month, before midnight
        [TestCase("23:00-01:00", "2025-11-01 00:30", true)] // after midnight, next month
        [TestCase("23:00-01:00", "2025-11-01 01:30", false)] // after close
        [TestCase("23:00-01:00 off", "2025-10-31 23:30", false)]
        [TestCase("23:00-01:00 off", "2025-11-01 02:00", true)]

        // --- Multi-Day Transition ---
        [TestCase("00:00-24:00;Su off", "2025-10-12 00:00", false)] // Sunday closed
        [TestCase("00:00-24:00;Su off", "2025-10-13 00:00", true)]  // Monday reopened

        [TestCase("23:00-02:00;Sa off", "2025-10-11 23:30", false)] // Saturday off
        [TestCase("23:00-02:00;Sa off", "2025-10-12 01:00", true)] // Sunday 1 AM
        [TestCase("23:00-02:00;Su off", "2025-10-13 00:30", true)] // Monday night into Monday
        [TestCase("23:00-02:00;Su off", "2025-10-13 01:59", true)]
        [TestCase("23:00-02:00;Su off", "2025-10-13 02:00", false)] // after close
        [TestCase("23:00-02:00;Su off", "2025-10-14 00:30", true)]  // Tuesday OK

        // --- Timezone Edge: near midnight ---
        [TestCase("00:00-24:00;", "2025-10-08 15:00", true)] // always open
        [TestCase("23:00-01:00;", "2025-10-08 20:00", false)] // outside

        public void Check00And24HourEdgeRanges(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var result = OpeningHours.IsNowWithinBusinessHours(pattern, dateTime);
            Assert.That(result, Is.EqualTo(expected));
        }
    }

}