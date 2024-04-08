
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsHotel

    {

        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? Fk_City_Name { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Place_Name { get; set; }
        [NotMapped]
        public string? Place { get; set; }
        public string? Fk_HotelType_Name { get; set; }
        public string? Fk_Currency_Name { get; set; }
        public string? Fk_ChainBrand_Name { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? TagLine { get; set; }
        public string? CoverImage { get; set; }
        public string? Address { get; set; }
        public string? EmailAddress { get; set; }
        public string? Phone1 { get; set; }
        public string? ContactPerson { get; set; }
        public string? GstNo { get; set; }
        public string? Landmark { get; set; }
        public int? ReviewRating { get; set; }
        public int? StarRating { get; set; }
        public bool IsFreeCancellation { get; set; }
        public bool IsBookWithoutCreditCard { get; set; }
        public bool IsNoPrePayment { get; set; }
        public bool IsFeaturedProperty { get; set; }
        public string? Description { get; set; }
        public int? PropertyCode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? FileHotelRegistration { get; set; }
        public int? BuiltYear { get; set; }
        public int? NoOfRooms { get; set; }
        public string? TimeZone { get; set; }
        public string? CheckInTime { get; set; }
        public bool Is24HrCheckIn { get; set; }
        public string? CheckOutTime { get; set; }
        public int? NoOfFloors { get; set; }
        public int? NoOfRestaurant { get; set; }
        public int? Phone { get; set; }
        public int? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public int? CustomerCareNum { get; set; }
        public bool FreezeStatus { get; set; }
        public string? FreezedBy { get; set; }
        public string? Remarks { get; set; }
        public bool? GlobalStatus { get; set; }
        public bool? SelectStatus { get; set; }
        public bool? VerifiedStatus { get; set; }
        public DateTime? VerifiedOn { get; set; }
        [StringLength(450)]
        public string? VerifiedBy { get; set; }
        public bool? ActiveStatus { get; set; }
        public DateTime? ActivatedOn { get; set; }
        [StringLength(450)]
        public string? ActivatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(450)]
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [StringLength(450)]
        public string? ModifiedBy { get; set; }
        public int? ModificationFrequency { get; set; }
        [StringLength(450)]
        public string? GCompId { get; set; }

        [StringLength(450)]
        public string? CompId { get; set; }
        [Timestamp]
        [ConcurrencyCheck]
        public byte[]? RowVersion { get; set; }

    }
}
