using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsRentalProperty
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Fk_City_Name { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? EmailOwner { get; set; }
        public string? EmailContactPerson1 { get; set; }
        public string? PhoneNoOwner { get; set; }
        public string? PhoneNoContactPerson1 { get; set; }
        public bool MountainView { get; set; }
        public bool CityView { get; set; }
        public bool LakeView { get; set; }
        public bool AttachedWashroom { get; set; }
        public bool Kitchen { get; set; }
        public int? NoOfRooms { get; set; }
        public bool CarParking { get; set; }
        public bool BikeParking { get; set; }
        public bool Balcony { get; set; }
        public bool SeaView { get; set; }
        public bool ForestView { get; set; }
        public bool Commission { get; set; }
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
