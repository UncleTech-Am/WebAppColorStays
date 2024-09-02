
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsHotel

    {

        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [DisplayName("Place")]
        public string? Fk_Place_Name { get; set; }
        [DisplayName("HotelType")]
        public string? Fk_HotelType_Name { get; set; }
        [DisplayName("Currency")]
        public string? Fk_Currency_Name { get; set; }
        [DisplayName("ChainBrand")]
        public string? Fk_ChainBrand_Name { get; set; }
        [DisplayName("Location")]
        public string? Fk_Location_Name { get; set; }
        [NotMapped]
        public string? Location { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public string? Label { get; set; }
        public int? LikeCount { get; set; }
        public string? RankLabel { get; set; }
        [Remote("CheckDuplicationHotel", "Hotel", AdditionalFields = ("NameAction, Id, Fk_Location_Name"))]
		public string? Name { get; set; }
        [Remote("CheckDuplicationHotelRank", "Hotel", AdditionalFields = ("NameAction, Id, Fk_City_Name"))]
        public int? Rank { get; set; }
        public string? StructuredData { get; set; }
        public string? Video { get; set; }
        public string? TagLine { get; set; }
        public string? URL { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverAltTag { get; set; }
        [NotMapped]
        public string? CoverImageName { get; set; }
        [NotMapped]
        public int? ImageCount { get; set; }
        public string? DistanceFromCityCenter { get; set; }
        public bool BudgetHotels { get; set; }
        public bool LuxuryHotels { get; set; }
        public bool PremiumHotels { get; set; }
        public string? Address { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactPerson { get; set; }
        public string? GstNo { get; set; }
        public string? Landmark { get; set; }
        public int? ReviewRating { get; set; }
        public int? StarRating { get; set; }
        public bool IsFreeCancellation { get; set; }
        public bool IsBookWithoutCreditCard { get; set; }
        public bool IsNoPrePayment { get; set; }
        public bool IsFeaturedProperty { get; set; }
        [AllowHtml]
        public string? Description { get; set; }
        [AllowHtml]
        public string? NearByPlace { get; set; }
        public int? PropertyCode { get; set; }
        public string? MapLink { get; set; }
        public string? FileHotelRegistration { get; set; }
        public int? BuiltYear { get; set; }
        public int? NoOfRooms { get; set; }
        public string? TimeZone { get; set; }
        public string? CheckInTime { get; set; }
        public bool Is24HrCheckIn { get; set; }
        public string? CheckOutTime { get; set; }
        public int? NoOfFloors { get; set; }
        public int? NoOfRestaurant { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Website { get; set; }
        public string? CustomerCareNum { get; set; }
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
