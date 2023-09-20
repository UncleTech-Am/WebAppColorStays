using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsTravelAgent
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
        public double? Commission { get; set; }
        public string? EmailOwner { get; set; }
        public string? EmailContactPerson1 { get; set; }
        public string? EmailContactPerson2 { get; set; }
        public string? PhoneNoOwner { get; set; }
        public string? PhoneNoContactPerson1 { get; set; }
        public string? PhoneNoContactPerson2 { get; set; }
        public bool TrekkingPackage { get; set; }
        public bool Hotel { get; set; }
        public bool Activities { get; set; }
        public bool RentalCar { get; set; }
        public bool RentalBike { get; set; }
        public bool Bus { get; set; }
        public bool Air { get; set; }
        public bool Train { get; set; }
        public bool Visa { get; set; }
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
