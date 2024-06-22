using WebAppColorStays.Models.ViewModel;

namespace WebAppColorStays.Models
{
    public class OccupancyChart
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DayCount { get; set; }
        //public List<CsRoomNo> RoomNo { get; set; }
        public List<CsRoomType> RoomTypes { get; set; }
        //public List<CsBooking> Bookings { get; set; }
        //public List<CsBookingRoomWise>? BgRoomWise { get; set; }

    }
}
