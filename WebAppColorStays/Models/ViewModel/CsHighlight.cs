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
        public string? Image { get; set; }
        public string? AltTag { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public bool ShowCityHighlight { get; set; }
        public bool ShowStateHighlight { get; set; }
        public bool ShowCountryHighlight { get; set; }
        public string? Description { get; set; }
        public string? TimeToVisit { get; set; }
        public string? Crowd { get; set; }
        public string? Seasonal { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Ratings { get; set; }
        public string? PinCode { get; set; }
        public int? Rank { get; set; }
        public string? History { get; set; }
        public string? Facts { get; set; }
        public string? Stories { get; set; }
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
