using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsAirport
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string? Place { get; set; }
        public string? Fk_Place_Name { get; set; }
        public string? Fk_City_Name { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
        public bool? International { get; set; }
        public bool? Domestic { get; set; }
        public int? PinCode { get; set; }
        public string? Address { get; set; }
        public int? ContactNumber1 { get; set; }
        public int? ContactNumber2 { get; set; }
        public int? ContactNumber3 { get; set; }
        public string? LocationDescription { get; set; }
        public string? ConnectivityDescription { get; set; }
        public string? RunWayDescription { get; set; }
        public string? HowToReach { get; set; }
        public string? Upcoming { get; set; }
        public string? PhotoCover { get; set; }
        public string? Photo1 { get; set; }
        public string? Photo2 { get; set; }
        public string? Photo3 { get; set; }
        public string? Photo4 { get; set; }
        public string? Photo5 { get; set; }
        public string? Video1 { get; set; }
        public string? Video2 { get; set; }
        public string? Video3 { get; set; }
        public string? Video4 { get; set; }
        public string? Video5 { get; set; }
        public string? OutTransportation { get; set; }
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
