using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsLocation
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
        [NotMapped]
        public string? City { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [NotMapped]
        public string? Country { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
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
