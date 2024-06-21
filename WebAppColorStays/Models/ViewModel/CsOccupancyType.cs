using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsOccupancyType
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("Occupancy_Type")]
        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? Name { get; set; }
        [StringLength(200, ErrorMessage = "You can enter only 200 characters long!")]
        public string? Description { get; set; }
        [DisplayName("No_Of_Persons")]
        [Remote("CheckDuplicationOccupancyType", "OccupancyType", AdditionalFields = ("NameAction , Id, Name"))]
        public int? TotalNoOfPersons { get; set; }
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
