using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPlace
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public string? URL { get; set; }
        public string? Label { get; set; }
        public string? RankLabel { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [Remote("CheckDuplicationPlace", "Place", AdditionalFields = ("NameAction, Id, Fk_City_Name"))]
        public string? Name { get; set; }
        public string? StructuredData { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? AltTag { get; set; }
        [AllowHtml]
        public string? History { get; set; }
        [AllowHtml]
        public string? Fact { get; set; }
        [AllowHtml]
        public string? Stories { get; set; }
        public string? Longitude { get; set; }
        public string? DistanceFromCityCenter { get; set; }
        public string? Latitude { get; set; }
        [Remote("CheckDuplicationPlaceRank", "Place", AdditionalFields = ("NameAction, Fk_City_Name, Id"))]
        public int? Rank { get; set; }
        [DisplayName("PlaceType")]
        public string? Fk_PlaceType_Name { get; set; }
        public int? PinCode { get; set; }
        [DisplayName("Video Link")]
        public string? Video { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [AllowHtml]
        public string? TopThingsToKnow { get; set; }
        public int? Rating { get; set; }
        public bool CountryAttraction { get; set; }
        public bool StateAttraction { get; set; }
        public string? BestTimeToVisit { get; set; }
        [DisplayName("TransportType")]
        public string? HowToReach { get; set; }
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
