using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsActivityType
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }

        [StringLength(100, ErrorMessage = "You can enter only 100 characters long!")]
        public string? Name { get; set; }

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? ImageName { get; set; }
        [NotMapped]
        public string? ImageUrl { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AltTag { get; set; }
        public string? StructuredData { get; set; }
        [AllowHtml]
        public string? Description { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? Icon { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
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
    }
}
