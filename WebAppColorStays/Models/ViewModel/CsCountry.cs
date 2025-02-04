using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsCountry
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
        [DisplayName("Continent")]
        public string? Fk_Continent_Name { get; set; }
        public string? StructuredData { get; set; }
        public string? URL { get; set; }
        public string? Label { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [Remote("CheckDuplicationCountry", "Country", AdditionalFields = ("NameAction, Id"))]
        public string Name { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? AltTag { get; set; }
        [AllowHtml]
        public string? History { get; set; }
        public string? Quote { get; set; }
        public string? Quote1 { get; set; }
        [AllowHtml]
        public string? Fact { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        [DisplayName("Video Link")]
        public string? Video { get; set; }
        public string? VideoImage { get; set; }
        [NotMapped]
        public string? VideoImageName { get; set; }
        public string? VideoImageAltTag { get; set; }
        public string? Area { get; set; }
        public string? Population { get; set; }
        public string? PopularReligion { get; set; }
        public int? Rating { get; set; }
        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? RequiredDays { get; set; }
        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? Icon { get; set; }
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
