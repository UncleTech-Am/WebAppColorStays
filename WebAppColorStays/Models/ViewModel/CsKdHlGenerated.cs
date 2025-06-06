using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsKdHlGenerated
    {
        public string? Id { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverAltTag { get; set; }
        [NotMapped]
        public string? CoverImageName { get; set; }
        public string? URL { get; set; }
        public string? Fk_KdCnCategory_Name { get; set; }
        public string? Fk_KdCnHlForm_Name { get; set; }
        public string? Fk_HotelType_Name { get; set; }
        public string? Heading { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? FromPrice { get; set; }
        public bool? ToPrice { get; set; }
        public bool? NoOfPerson { get; set; }
        public bool? BudgetHotels { get; set; }
        public bool? LuxuryHotels { get; set; }
        public bool? PremiumHotels { get; set; }
        public bool? IsFreeCancellation { get; set; }
        public bool? IsBookWithoutCreditCard { get; set; }
        public bool? IsNoPrePayment { get; set; }
        public bool? IsFeaturedProperty { get; set; }
        public string? DistanceFromCityCenter { get; set; }
        public string? DescriptionGenerated { get; set; }
        public string? DescriptionManual { get; set; }
        public string? StructuredData { get; set; }
        public string? SEOTitle { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeywords { get; set; }
        public bool? FreezeStatus { get; set; }
        public string? FreezedBy { get; set; }
        [StringLength(450)]
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
