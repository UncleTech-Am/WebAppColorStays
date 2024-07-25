using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsReviewQuestion
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        [Remote("CheckDuplicationReviewQuestion", "ReviewQuestion", AdditionalFields = ("NameAction, Fk_ReviewFor_Name, Id"))]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Name.")]
        [DisplayName("ReviewFor")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_ReviewFor_Name { get; set; }
        [NotMapped]
        public string? ReviewFor { get; set; }
        public bool IsRating { get; set; }
        public bool IsBool { get; set; }
        public bool IsText { get; set; }
        public int? SrNo { get; set; }
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
