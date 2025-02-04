using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsSEOHotelHyperLink
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        [DisplayName("Location")]
        public string? Fk_Location_Name { get; set; }
        [NotMapped]
        public string? Location { get; set; }
        [DisplayName("HotelType")]
        public string? Fk_HotelType_Name { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        public string? SEOTitle { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeywords { get; set; }
        public string? URL { get; set; }
        public string? Name { get; set; }
        [DisplayName("Video Link")]
        public string? Video { get; set; }
        public string? VideoImage { get; set; }
        [NotMapped]
        public string? VideoImageName { get; set; }
        public string? VideoImageAltTag { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverAltTag { get; set; }
        [NotMapped]
        public string? CoverImageName { get; set; }
        public string? TagLine { get; set; }
        [AllowHtml]
        public string? OtherDes { get; set; }
        [AllowHtml]
        public string? OtherDes1 { get; set; }
        [AllowHtml]
        public string? Description { get; set; }
        public string? Heading1 { get; set; }
        public string? Heading2 { get; set; }
        public string? StructuredData { get; set; }
        public int? StarRating { get; set; }
        public bool IsInBanner { get; set; }
        public bool BudgetHotels { get; set; }
        public bool LuxuryHotels { get; set; }
        public bool PremiumHotels { get; set; }
        public bool Spa { get; set; }
        public bool Parking { get; set; }
        public bool WiFi { get; set; }
        public bool Kitchenette { get; set; }
        public bool SwimmingPool { get; set; }
        public bool Under1000 { get; set; }
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
