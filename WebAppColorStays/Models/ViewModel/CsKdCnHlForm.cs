using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppColorStays.Areas.ColorStays.CommonMethods;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsKdCnHlForm
    {
        public string? Id { get; set; }
        public string? Label { get; set; }
        [DisplayName("Root")]
        public string? Fk_KdRoot_Name { get; set; }
        [DisplayName("Prefix")]
        public string? Fk_KdPrefix_Name { get; set; }
        [DisplayName("Suffix")]
        public string? Fk_KdSuffix_Name { get; set; }
        [DisplayName("Category")]
        public string? Fk_KdCnCategory_Name { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [NotMapped]
        public string? Country { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        [DisplayName("HotelType")]
        public string? Fk_HotelType_Name { get; set; }
        public string? Heading { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public double? FromPrice { get; set; }
        public double? ToPrice { get; set; }
        public int? NoOfPerson { get; set; }
        [NotMapped]
        public string? HotelCategory { get; set; }
        public bool BudgetHotels { get; set; }
        public bool LuxuryHotels { get; set; }
        public bool PremiumHotels { get; set; }
        [NotMapped]
        public string? Terms { get; set; }
        public bool IsFreeCancellation { get; set; }
        public bool IsBookWithoutCreditCard { get; set; }
        public bool IsNoPrePayment { get; set; }
        public bool IsFeaturedProperty { get; set; }
        public string? DistanceFromCityCenter { get; set; }
        [NotMapped]
        public string? IsAmenity { get; set; }
        [NotMapped]
        public string? VariablePosition { get; set; }
        public bool IsVeWdBeforePrefix { get; set; }
        public bool IsVeWdBeforeSuffix { get; set; }
        public bool IsVeWdBeforeRoot { get; set; }
        public bool IsVeWdAfterRoot { get; set; }
        public bool IsVeWdAfterPrefix { get; set; }
        public bool IsVeWdAfterSuffix { get; set; }
        public bool IsURLHavingState { get; set; }
        public bool IsURLHavingCountry { get; set; }
        public bool IsStateKeyword { get; set; }
        public bool IsCountryKeyword { get; set; }
        public string? SEOTitle { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeywords { get; set; }
        public bool FreezeStatus { get; set; }
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
        [NotMapped]
        public KeywordCityCheckbox? CheckCityList { get; set; }
        [NotMapped]
        public string[]? Amenity { get; set; }

    }
}
