using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsCity
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
        public string? StructuredData { get; set; }
        [DisplayName("State")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        [DisplayName("Country")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_Country_Name { get; set; }
        public string? URL { get; set; }
        [Remote("CheckDuplicationCityPlaceURL", "City", AdditionalFields = ("NameAction, Fk_Country_Name, Id"))]
        public string? VisitPlaceURL { get; set; }
        [Remote("CheckDuplicationCity", "City", AdditionalFields = ("NameAction, Id, Fk_State_Name"))]
        [Required(ErrorMessage = "Please enter Name.")]
        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DisplayName("TransportType")]
        public string? HowToReach { get; set; }
        public string? Label { get; set; }
        public string? RankLabel { get; set; }
        public string? CityCenter { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? AltTag { get; set; }
        [DisplayName("Video Link")]
        public string? Video { get; set; }
        public string? VideoImage { get; set; }
        public string? VideoImageName { get; set; }
        public string? VideoImageAltTag { get; set; }
        public string? Area { get; set; }
        [AllowHtml]
        public string? History { get; set; }
        [AllowHtml]
        public string? Fact { get; set; }
        [AllowHtml]
        public string? Stories { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public bool PopularDestination { get; set; }
        [Remote("CheckDuplicationCityRank", "City", AdditionalFields = ("NameAction, Fk_Country_Name, Id"))]
        public int? Rank { get; set; }
        public int? NoOfDays { get; set; }
        public int? NoOfPlaces { get; set; }
        [StringLength(100, ErrorMessage = "You can enter only 100 characters long!")]
        public string? TagLine { get; set; }
        public string? ClothingToCarry { get; set; }
        public string? Footwear { get; set; }
        [DisplayName("Terrain")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_Terrain_Name { get; set; }
        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? PeakSeason { get; set; }
        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? OffSeason { get; set; }
        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? MidSeason { get; set; }
        public int? Rating { get; set; }
        public double? LowestPackage { get; set; }
        public int? PinCode { get; set; }
        public bool January { get; set; }
        public bool February { get; set; }
        public bool March { get; set; }
        public bool April { get; set; }
        public bool May { get; set; }
        public bool June { get; set; }
        public bool July { get; set; }
        public bool August { get; set; }
        public bool September { get; set; }
        public bool October { get; set; }
        public bool November { get; set; }
        public bool December { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? OurTips1 { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]

        public string? OurTips2 { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]

        public string? OurTips3 { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]

        public string? OutTips4 { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]

        public string? OurTips5 { get; set; }
        public int? TelephoneCode { get; set; }
        [AllowHtml]
        public string? TopThingsToKnow { get; set; }
        public string? Itinerary { get; set; }
        public string? BestTimeToVisit { get; set; }
        public bool FreezeStatus { get; set; }
        public string? FreezedBy { get; set; }
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]

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
