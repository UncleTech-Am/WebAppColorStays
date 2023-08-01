using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPlace
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? SEOTitle { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeywords { get; set; }
        public string? URL { get; set; }
        public string? Name { get; set; }
        public string? History { get; set; }
        public string? Fact { get; set; }
        public string? Stories { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public int? Rank { get; set; }
        public string? Fk_PlaceType_Name { get; set; }
        public int? PinCode { get; set; }
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? TopThingsToKnow { get; set; }
        public int? Rating { get; set; }
        public string? StateAttraction { get; set; }
        public string? BestTimeToVisit { get; set; }
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
