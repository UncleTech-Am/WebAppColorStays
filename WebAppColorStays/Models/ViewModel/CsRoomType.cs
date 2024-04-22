using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsRoomType
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }
        [NotMapped]
        public string? Hotel { get; set; }
        [DisplayName("RoomCategory")]
        public string? Fk_RoomCategory_Name { get; set; }
        public string? Description { get; set; }
        public int? TotalRooms { get; set; }
        public string? SizeArea { get; set; }
        public string? SizeLength { get; set; }
        public string? SizeBreadth { get; set; }
        public bool IsSizeFeet { get; set; }
        public bool IsSizeMeter { get; set; }
        public int? OyBaseAdults { get; set; }
        public int? OyMaxAdults { get; set; }
        public int? OyMaxChildren { get; set; }
        public string? OyMaxAdultChildren { get; set; }

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
