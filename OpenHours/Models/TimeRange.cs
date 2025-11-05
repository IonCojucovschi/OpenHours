using System;

namespace OpenHours.Models
{
    public class TimeRange
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool Includes(TimeSpan timeSpan)
        {
            if (StartTime < EndTime
                && StartTime <= timeSpan
                && timeSpan < EndTime
                )
            {
                return true;
            }
            if (StartTime > EndTime
                && (timeSpan >= StartTime
                        || timeSpan < EndTime)
                )
            {
                return true;
            }
            return false;
        }
    }

}
