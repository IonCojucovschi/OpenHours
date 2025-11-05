namespace OpenHours.Models
{
    public class MonthRange
    {
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }

        public bool Includes(int month)
        {
            return month >= StartMonth && month <= EndMonth;
        }
    }
}
