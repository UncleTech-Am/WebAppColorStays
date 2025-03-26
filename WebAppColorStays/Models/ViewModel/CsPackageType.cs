using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPackageType
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? URL { get; set; }
        public bool IsCompleteURL { get; set; }
        [StringLength(60, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(155, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public string? StructuredData { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public string? ImageUrl { get; set; }
        public string? AltTag { get; set; }
        public string? Description { get; set; }
        public double? StartingPrice { get; set; }
        public double? EndPrice { get; set; }
        [DisplayName("Video")]
        public string? BigImage { get; set; }
        public string? AltTag1 { get; set; }
        [NotMapped]
        public string? ImageUrl1 { get; set; }
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
