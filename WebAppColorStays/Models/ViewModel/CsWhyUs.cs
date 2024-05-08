using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsWhyUs
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ReviewTagLine { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? InstaId { get; set; }

        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }

        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }

        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }

        [NotMapped]
        public string? City { get; set; }


        [DisplayName("Hotel Type")]
        public string? Fk_HotelType_Name { get; set; }
        [DisplayName("Package Type")]
        public string? Fk_PackageType_Name { get; set; }
        [DisplayName("Activity Type")]
        public string? Fk_ActivityType_Name { get; set; }
   
        [DisplayName("InCountry")]
        public bool IsInCountry { get; set; }
        [DisplayName("InState")]
        public bool IsInState { get; set; }

        [DisplayName("InCity")]
        public bool IsInCity { get; set; }
  
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
