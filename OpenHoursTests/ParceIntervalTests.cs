using NUnit.Framework;
using OpenHours;
using System;

namespace OpenHoursTests
{
    [TestFixture]
    public class OpeningHoursTests
    {
        [TestCase("08:30-20:00", "2025-03-15 20:00", false)]
        [TestCase("08:30-20:00", "2025-03-15 15:00", true)]
        [TestCase("08:30-20:00", "2025-03-15 21:00", false)]
        [TestCase("20:30-08:00", "2025-03-15 21:00", true)]
        [TestCase("08:30-20:00 off", "2025-03-15 20:00", true)]
        [TestCase("08:30-20:00 off", "2025-03-15 15:00", false)]
        [TestCase("08:30-20:00 off", "2025-03-15 21:00", true)]
        public void CheckHoursRange(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }

        [Test]
        [TestCase("20", "2025-08-20", true)]
        [TestCase("20", "2025-08-21", false)]
        [TestCase("20-23", "2025-08-21", true)]
        [TestCase("20-23", "2025-08-20", true)]
        [TestCase("20-23", "2025-08-23", true)]
        [TestCase("20-23", "2025-08-24", false)]

        [TestCase("20 off", "2025-08-20", false)]
        [TestCase("20 off", "2025-08-21", true)]
        [TestCase("20-23 off", "2025-08-21", false)]
        [TestCase("20-23 off", "2025-08-20", false)]
        [TestCase("20-23 off", "2025-08-23", false)]
        [TestCase("20-23 off", "2025-08-24", true)]
        public void CheckDate(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }

        [TestCase("Mo-Th 08:30-20:00", "2025-01-15 09:00", true)] // Wensday in January
        [TestCase("Mo-Th 08:30-20:00", "2025-01-15 08:30", true)] // Wensday in January
        [TestCase("Mo-Th 08:30-20:00", "2025-01-15 20:00", false)] // Wensday in January
        [TestCase("Mo-Th 08:30-20:00", "2025-01-15 08:20", false)] // Out of hours
        [TestCase("Mo-Th 08:30-20:00", "2025-01-15 21:00", false)] // Out of hours
        [TestCase("Mo-Th 08:30-20:00", "2025-01-17 20:00", false)] // Friday out of day range
        [TestCase("Mo-Th 08:30-20:00", "2025-01-19 20:00", false)] // Sunday out of day range

        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-15 09:00", false)] // Wensday in January
        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-15 08:30", false)] // Wensday in January
        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-15 20:00", true)] // Wensday in January
        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-15 08:20", true)] // Out of hours
        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-15 21:00", true)] // Out of hours
        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-17 20:00", true)] // Friday out of day rangef
        [TestCase("Mo-Th 08:30-20:00 off", "2025-01-19 20:00", true)] // Sunday out of day range
        public void CheckWeekDayRange(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }


        [TestCase("Fr[3]", "2025-10-17", true)] // 3-rd friday in Octomber
        [TestCase("Fr[3]", "2025-10-24", false)] // 4-th Friday in Octomber
        [TestCase("Fr[3] 08:30-20:22", "2025-10-17 09:00", true)] // 3-rd friday in Octomber in hour range
        [TestCase("Fr[3] 08:30-20:22", "2025-10-17 21:00", false)] // 3-rd Friday in Octomber out of hour range

        [TestCase("Fr[3] off", "2025-10-17", false)] // 3-rd friday in Octomber
        [TestCase("Fr[3] off", "2025-10-24", true)] // 4-th Friday in Octomber
        [TestCase("Fr[3] 08:30-20:22 off", "2025-10-17 09:00", false)] // 3-rd friday in Octomber in hour range
        [TestCase("Fr[3] 08:30-20:22 off", "2025-10-17 21:00", true)] // 3-rd Friday in Octomber out of hour range


        [TestCase("Mo[1],Mo[3] 06:15-22:59", "2025-10-19 21:00", false)] // Sunday
        [TestCase("Mo[1],Mo[3] 06:15-22:59", "2025-10-20 21:00", true)] // 3rd Mo
        [TestCase("Mo[1],Mo[3] 06:15-22:59", "2025-10-06 21:00", true)] // 1st Mo
        public void CheckNthWeekDayInMonth(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }

        [TestCase("Fr", "2025-10-17", true)] // 3-rd friday in Octomber
        [TestCase("Fr", "2025-10-24", true)] // 4-th Friday in Octomber
        [TestCase("Fr 08:30-20:22", "2025-10-17 09:00", true)] // 3-rd friday in Octomber in hour range
        [TestCase("Fr 08:30-20:22", "2025-10-17 21:00", false)] // 3-rd Friday in Octomber out of hour range

        [TestCase("Fr off", "2025-10-17", false)] // 3-rd friday in Octomber
        [TestCase("Fr off", "2025-10-24", false)] // 4-th Friday in Octomber
        [TestCase("Fr 08:30-20:22 off", "2025-10-17 09:00", false)] // 3-rd friday in Octomber in hour range
        [TestCase("Fr 08:30-20:22 off", "2025-10-17 21:00", true)] // 3-rd Friday in Octomber out of hour range

        public void CheckWeekDay(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }

        [TestCase("Aug", "2025-08-01", true)]
        [TestCase("Aug", "2025-09-01", false)]
        [TestCase("Aug off", "2025-08-01", false)]
        [TestCase("Aug off", "2025-09-01", true)]
        [TestCase("Aug-Sep", "2025-08-15", true)]
        [TestCase("Aug-Sep", "2025-09-15", true)]
        [TestCase("Aug-Sep", "2025-10-01", false)]
        [TestCase("Aug-Sep off", "2025-08-15", false)]
        [TestCase("Aug-Sep off", "2025-10-01", true)]
        [TestCase("Aug 20", "2025-08-20", true)]
        [TestCase("Aug 20", "2025-08-21", false)]
        [TestCase("Aug 20 off", "2025-08-20", false)]
        [TestCase("Aug 20 off", "2025-08-21", true)]
        [TestCase("Fr", "2025-10-10", true)] // Friday
        [TestCase("Fr", "2025-10-11", false)] // Saturday
        [TestCase("Fr off", "2025-10-10", false)]
        [TestCase("Fr off", "2025-10-11", true)]
        [TestCase("Fr[2]", "2025-10-10", true)] // 2-nd Friday
        [TestCase("Fr[2]", "2025-10-17", false)] // 3-rd Friday
        [TestCase("Fr[2] off", "2025-10-10", false)]
        [TestCase("Fr[2] off", "2025-10-17", true)]
        [TestCase("08:00-10:00", "2025-10-08 08:30", true)]
        [TestCase("08:00-10:00", "2025-10-08 07:59", false)]
        [TestCase("08:00-10:00 off", "2025-10-08 08:30", false)]
        [TestCase("08:00-10:00 off", "2025-10-08 07:59", true)]
        [TestCase("Aug Fr[2] 08:00-10:00", "2025-08-08 08:30", true)] // Second friday in august
        [TestCase("Aug Fr[2] 08:00-10:00", "2025-08-08 07:59", false)] // Out of hours
        [TestCase("Aug Fr[2] 08:00-10:00", "2025-08-15 08:30", false)] // 3-rd Friday
        [TestCase("Aug Fr[2] 08:00-10:00 off", "2025-08-08 08:30", false)]
        [TestCase("Aug Fr[2] 08:00-10:00 off", "2025-08-15 08:30", true)]
        [TestCase("20", "2025-08-20", true)]
        [TestCase("20", "2025-08-21", false)]
        [TestCase("20 off", "2025-08-20", false)]
        [TestCase("20 off", "2025-08-21", true)]
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-15 09:00", true)] // Wensday in January
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-15 08:30", true)] // Wensday in January
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-15 20:00", false)] // Wensday in January
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-15 08:20", false)] // Out of hours
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-15 21:00", false)] // Out of hours
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-17 20:00", false)] // Friday in January out of day range
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-01-19 20:00", false)] // Sunday in January out of day range
        [TestCase("Jan-Feb Mo-Th 08:30-20:00", "2025-03-15 20:00", false)] // March out of month range
        [TestCase("Mo-Th 08:00-09:00", "2025-10-07 08:30", true)] // Thursday
        [TestCase("Mo-Th 08:00-09:00", "2025-10-07 07:59", false)] // Before interval
        public void CheckIfDateIsInOpenHours_ShouldReturnExpected(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [TestFixture]
    public class OpeningHoursEdgeCaseTests
    {
        [TestCase("00:00-24:00", "2025-10-08 00:00", true)]  // Start of day
        [TestCase("00:00-24:00", "2025-10-08 23:59", true)]  // End of day
        [TestCase("00:00-24:00", "2025-10-09 00:00", true)] // Next day boundary
        [TestCase("00:00-24:00 off", "2025-10-08 00:00", false)] // Whole-day off
        [TestCase("00:00-24:00 off", "2025-10-08 23:59", false)] // Still off
        [TestCase("00:00-24:00 off", "2025-10-09 00:00", false)]

        [TestCase("00:00-00:00", "2025-10-08 00:00", false)] // 0-length interval (closed)
        [TestCase("00:00-00:00 off", "2025-10-08 00:00", true)] // Off range inverts (open)
        [TestCase("23:00-00:00", "2025-10-08 23:30", true)] // Overnight range (wraps past midnight)
        [TestCase("23:00-00:00", "2025-10-09 00:00", false)] // Exactly at boundary next day
        [TestCase("23:00-00:00 off", "2025-10-08 23:30", false)] // Overnight but off
        [TestCase("23:00-00:00 off", "2025-10-09 00:30", true)] // After midnight, open

        [TestCase("Mo-Su 00:00-24:00", "2025-10-08 00:00", true)] // Always open full day
        [TestCase("Mo-Su 00:00-24:00 off", "2025-10-08 00:00", false)] // Always closed full day
        [TestCase("Mo-Fr 00:00-24:00", "2025-10-12 12:00", false)] // Sunday closed
        [TestCase("Mo-Fr 00:00-24:00", "2025-10-11 23:59", false)] // Saturday closed
        [TestCase("Mo-Fr 00:00-24:00", "2025-10-10 23:59", true)] // Friday open
        [TestCase("Mo-Fr 00:00-24:00 off", "2025-10-10 23:59", false)] // Friday but off

        [TestCase("00:00-06:00", "2025-10-08 05:59", true)] // Before morning
        [TestCase("00:00-06:00", "2025-10-08 06:00", false)] // Boundary closed
        [TestCase("00:00-06:00 off", "2025-10-08 05:59", false)] // Off variant
        [TestCase("00:00-06:00 off", "2025-10-08 06:30", true)] // Out of range but off => true
        public void CheckMidnightAndFullDayRanges(string pattern, string dateTimeString, bool expected)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            var rule = OpeningHours.ParceInterval(pattern);
            var result = rule.IsOpen(dateTime);
            Assert.That(result, Is.EqualTo(expected));
        }
    }


}