using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsAnState
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("State")]
        [Required(ErrorMessage = "Please enter State Name.")]
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        [Required(ErrorMessage = "Please enter An No.")]
        [Remote("CheckDuplicationAnStateAnNo", "AnState", AdditionalFields = ("NameAction, Fk_State_Name, Id"))]
        public int? AnNo { get; set; }
        [Required(ErrorMessage = "Please enter AccordianHeading.")]
        [Remote("CheckDuplicationAnState", "AnState", AdditionalFields = ("NameAction, Fk_State_Name, Id"))]
        public string? AccordianHeading { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
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
