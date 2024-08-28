using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsRestaurant
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("Place")]
        public string? Fk_Place_Name { get; set; }
        [NotMapped]
        public string? Place { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        public string? Label { get; set; }
        public string? RankLabel { get; set; }
        [Remote("CheckDuplicationRestaurantRank", "Restaurant", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        public int? Rank { get; set; }
        [Required(ErrorMessage = "Please enter Restaurant Name.")]
        [Remote("CheckDuplicationRestaurant", "Restaurant", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        public string Name { get; set; }
        public string? URL { get; set; }
        [AllowHtml]
        public string? Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? AltTag { get; set; }
        public bool ShowInCity { get; set; }
        public bool ShowInState { get; set; }
        public bool ShowInCountry { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LandMark { get; set; }
        public string? DistanceFromLandMark { get; set; }
        public string? TagLine { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public bool Chinese { get; set; }
        public bool NorthIndian { get; set; }
        public bool Barbeque { get; set; }
        public bool SouthIndian { get; set; }
        public bool Italion { get; set; }
        public bool Gujrati { get; set; }
        public bool PureVeg { get; set; }
        public bool Bakery { get; set; }
        public bool Sweets { get; set; }
        public string? ContactNumber { get; set; }
        public string? About { get; set; }
        public bool Alcohal { get; set; }
        public bool WashRoom { get; set; }
        public string? Address { get; set; }
        public string? Website { get; set; }
        public string? Owner { get; set; }
        public string? Email { get; set; }
        public string? Timing { get; set; }
        public int? Days { get; set; }
        public DateTime? OpeningDate { get; set; }
        public bool OpenStatus { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
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
    }
}
