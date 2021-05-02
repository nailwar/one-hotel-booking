using System.Collections.Generic;

namespace OneHotelBooking.DbModels
{
    public class DbRoom
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public ICollection<DbReservation> Reservations { get; set; }
    }
}
