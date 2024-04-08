﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsHotelRatingCSPerson
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Fk_Hotel_Name { get; set; }
        public int? OwnerAttitude { get; set; }
        public int? Location { get; set; }
        public int? ViewRating { get; set; }
        public int? Parking { get; set; }
        public int? StaffProfessionalism { get; set; }
        public int? HotelInterior { get; set; }
        public int? RoomInterior { get; set; }
        public int? Restaurant { get; set; }
        public int? Amenity { get; set; }
        public int? HotelResponseRate { get; set; }
        public int? Cleaniness { get; set; }
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

        public virtual CsHotel? TblHotel { get; set; }

    }
}
