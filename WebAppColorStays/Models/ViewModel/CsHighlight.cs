using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsHighlight 
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
     
        [ForeignKey("TblCity")]
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        [ForeignKey("TblState")]
        public string? Fk_State_Name { get; set; }
        [ForeignKey("TblCountry")]
        public string? Fk_Country_Name { get; set; }
        [ForeignKey("TblPlaceType")]
        public string? Fk_PlaceType_Name { get; set; }
        public string? StructuredData { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public string? Image { get; set; }
        public string? AltTag { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public bool ShowCityHighlight { get; set; }
        public bool ShowStateHighlight { get; set; }
        public bool ShowCountryHighlight { get; set; }
        [AllowHtml]
        public string? Description { get; set; }
        public string? TimeToVisit { get; set; }
        public string? Crowd { get; set; }
        public string? Seasonal { get; set; }
        public int? Ratings { get; set; }
        public int? Rank { get; set; }
        [AllowHtml]
        public string? History { get; set; }
        [AllowHtml]
        public string? Facts { get; set; }
        [AllowHtml]
        public string? Stories { get; set; }
        [AllowHtml]
        public string? TopThingsToKnow { get; set; }
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
