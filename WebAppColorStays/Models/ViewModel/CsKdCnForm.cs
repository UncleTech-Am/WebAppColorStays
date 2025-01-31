using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsKdCnForm
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [NotMapped]
        public string? Country { get; set; }
        public string? Fk_KdCnCategory_Name { get; set; }
        public string? Fk_KdRoot_Name { get; set; }
        [NotMapped]
        public string? Root { get; set; }
        public string? Fk_KdPrefix_Name { get; set; }
        public string? Fk_KdSuffix_Name { get; set; }
        public bool IsVeWdBeforePrefix { get; set; }
        public bool IsVeWdBeforeSuffix { get; set; }
        public bool IsVeWdBeforeRoot { get; set; }
        public bool IsVeWdAfterSuffix { get; set; }
        public bool IsURLHavingState { get; set; }
        public bool IsURLHavingCountry { get; set; }
        public bool IsStateKeyword { get; set; }
        public bool IsCountryKeyword { get; set; }
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
