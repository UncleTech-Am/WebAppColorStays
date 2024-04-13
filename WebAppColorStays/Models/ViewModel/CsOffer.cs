using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsOffer
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }

        [StringLength(450)]
        [Required(ErrorMessage = "Please enter Name.")]
        [Remote("CheckDuplicationOffer", "Offer", AdditionalFields = ("NameAction, Id"))]
        public string? Name { get; set; }

        public string? Fk_Hotel_Name { get; set; }

        [NotMapped]
        public string? Hotel { get; set; }

        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }

        [NotMapped]
        public string? Package { get; set; }

        public string? Image { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? Title { get; set; }
        public string? TagLine { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountPrice { get; set; }
        public string? GeneratedCode { get; set; }
        public string? CommonCode { get; set; }
      
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
