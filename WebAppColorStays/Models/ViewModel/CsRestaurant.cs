using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsRestaurant
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
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [Remote("CheckDuplicationRestaurantRank", "Restaurant", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        public int? Rank { get; set; }
        [Required(ErrorMessage = "Please enter Restaurant Name.")]
        [Remote("CheckDuplicationRestaurant", "Restaurant", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        public string Name { get; set; }
        public bool Chinese { get; set; }
        public bool SouthIndian { get; set; }
        public bool Italion { get; set; }
        public bool Gujrati { get; set; }
        public bool PureVeg { get; set; }
        public bool Bakery { get; set; }
        public bool Sweets { get; set; }
        public int? ContactNumber { get; set; }
        public string? About { get; set; }
        public string? CoverPicture { get; set; }
        public bool Alcohal { get; set; }
        public bool WashRoom { get; set; }
        public string? Address { get; set; }
        public string? Website { get; set; }
        public string? Owner { get; set; }
        public string? Email { get; set; }
        public string? Timing { get; set; }
        public int? Days { get; set; }
        public DateTime? OpeningDate { get; set; }
        public bool OpenStatus { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
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
