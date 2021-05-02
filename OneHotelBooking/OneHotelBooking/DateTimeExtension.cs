using System;

namespace OneHotelBooking
{
    public static class DateTimeExtension
    {
        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date;
        }
    }
}
