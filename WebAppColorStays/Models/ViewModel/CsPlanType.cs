using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPlanType
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Fk_Hotel_Name { get; set; }
        public string? Label { get; set; }
        [Remote("CheckDuplicationPlanType", "PlanType", AdditionalFields = ("NameAction, Id, Fk_Hotel_Name"))]
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool BreakFast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool StayOnly { get; set; }
		public string? Priority { get; set; }
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

        [NotMapped]
        public bool SelectPlan { get; set; }
    }
}
