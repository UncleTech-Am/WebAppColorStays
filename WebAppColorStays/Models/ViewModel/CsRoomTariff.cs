using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsRoomTariff : CsRoomInventory
    {
        public CsRoomTariff()
        {
            RoomTariffList = new List<CsRoomInventory>();
            PlanList = new List<CsPlanType>();
            PriceList = new List<CsRoomTariff>();
            RoomTypeList = new List<CsRoomType>();
            RoomOccMapList = new List<CsOccupancyType>();
        }
        public List<CsRoomInventory>? RoomTariffList { get; set; }
        public List<CsPlanType>? PlanList { get; set; }
        public List<CsRoomTariff>? PriceList { get; set; }
        public List<CsRoomType>? RoomTypeList { get; set; }
        public List<CsOccupancyType>? RoomOccMapList { get; set; }
        public int? DayCount { get; set; }
        public int? BookedRoom { get; set; }
        public string? RoomTariffId { get; set; }
        public string? InventoryId { get; set; }
        public string? SeasonId { get; set; }
        [Required(ErrorMessage = "This Field is Required!")]
        public string? Fk_PlanType_Name { get; set; }
        public string? PlanType { get; set; }
        [Required(ErrorMessage = "This Field is Required!")]
        public Nullable<double> XAdultCost { get; set; }
        [Required(ErrorMessage = "This Field is Required!")]
        public Nullable<double> XChildCost { get; set; }
        public Nullable<int> RoomCount { get; set; }
        [Required(ErrorMessage = "This Field is Required!")]
        public DateTime? ToDate { get; set; }
        [Required(ErrorMessage = "This Field is Required!")]
        public DateTime? FromDate { get; set; }
        public string? Fk_City_Name { get; set; }
        public string? HotelName { get; set; }
        public string? Message { get; set; }
        public string? RoomType { get; set; }
        public string? Fk_OccupancyType_Name { get; set; }
        public double? OccupancyCost { get; set; }
        [NotMapped]
        public string? ShowXAdult { get; set; }
        [NotMapped]
        public string? ShowXChild { get; set; }
    }
}
