using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPhoto
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Fk_Place_Name { get; set; }
        public string? Fk_City_Name { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? Title { get; set; }
        [NotMapped]
        public string? ImageUrl { get; set; }
        public string? AltTag { get; set; }
        public string? Description { get; set; }
        public string? UploadedFolder { get; set; }
        public bool ArchiveStatus { get; set; }
        public bool ShowPlaceGallery { get; set; }
        public bool ShowCityGallery { get; set; }
        public bool ShowStateGallery { get; set; }
        public bool ShowCountryGallery { get; set; }
        public string? Fk_Category_Name { get; set; }
        public string? Fk_SubCategory_Name { get; set; }
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
