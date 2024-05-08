using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsNetReport
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }

        [DisplayName("ReviewFor")]
        public string? Fk_ReviewFor_Name { get; set; }

        [DisplayName("ReviewQuestion")]
        public string? Fk_ReviewQuestion_Name { get; set; }

        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }

        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }

        [DisplayName("Place")]
        public string? Fk_Place_Name { get; set; }

        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }

        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }

        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        public int? ReviewCount { get; set; }
        public double? TotalRating { get; set; }
        public double? ScoredRating { get; set; }
        public double? TotalBool { get; set; }
        public double? TotalTrueBool { get; set; }
        public bool? FreezeStatus { get; set; }
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
