using System;

namespace OneHotelBooking.DateTimeHelpers
{
    public static class DateTimeExtension
    {
        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date;
        }
    }
}
