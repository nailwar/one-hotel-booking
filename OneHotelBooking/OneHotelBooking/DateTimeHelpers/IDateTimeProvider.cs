using System;

namespace OneHotelBooking.DateTimeHelpers
{
    public interface IDateTimeProvider
    {
        public DateTime TodayStartOfDayDate { get;}
    }
}