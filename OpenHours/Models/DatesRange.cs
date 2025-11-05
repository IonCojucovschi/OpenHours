namespace OpenHours.Models
{
    public class DatesRange
    {
        public int StartDate { get; set; }
        public int EndDate { get; set; }

        public bool Includes(int month)
        {
            return month >= StartDate && month <= EndDate;
        }
    }
}
