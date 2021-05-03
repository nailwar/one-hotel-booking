using System;

namespace OneHotelBooking.DateTimeHelpers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime TodayStartOfDayDate => DateTime.Today.StartOfDay();
    }
}