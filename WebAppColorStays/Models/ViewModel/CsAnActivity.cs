using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsAnActivity
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? LabelNo { get; set; }
        public string? LabelHg { get; set; }
        [DisplayName("Activity")]
        [Required(ErrorMessage = "Please enter Activity Name.")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_Activities_Name { get; set; }
        [NotMapped]
        public string? Activity { get; set; }
        [Required(ErrorMessage = "Please enter An No.")]
        [Remote("CheckDuplicationAnActivityAnNo", "AnActivity", AdditionalFields = ("NameAction, Fk_Activities_Name, Id"))]
        public int? AnNo { get; set; }
        [Required(ErrorMessage = "Please enter AccordianHeading.")]
        [Remote("CheckDuplicationAnActivity", "AnActivity", AdditionalFields = ("NameAction, Fk_Activities_Name, Id"))]
        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AccordianHeading { get; set; }
        [AllowHtml]
        public string? Description { get; set; }

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        [DisplayName("Icon")]
        public string? Fk_Icon_Name { get; set; }
        [NotMapped]
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
