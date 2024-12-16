using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsSEOPackageHyperLink
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        [DisplayName("Location")]
        public string? Fk_Location_Name { get; set; }
        [NotMapped]
        public string? Location { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        [StringLength(60, ErrorMessage = "You can enter only 60 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(155, ErrorMessage = "You can enter only 155 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public string? URL { get; set; }
        public string? Name { get; set; }
        public string? Video { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverAltTag { get; set; }
        [NotMapped]
        public string? CoverImageName { get; set; }
        public string? TagLine { get; set; }
        [AllowHtml]
        public string? OtherDes { get; set; }
        [AllowHtml]
        public string? Description { get; set; }
        public string? StructuredData { get; set; }
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
