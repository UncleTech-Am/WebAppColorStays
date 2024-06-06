using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Model.ViewModel
{
    public partial class CsBlogImage 
    {
        public string? Id { get; set; }
        [DisplayName("Image_Name")]
        public string? ImageName { get; set; }
        [DisplayName("URL")]
        public string? Url { get; set; }
        [DisplayName("Blog")]
        public string? Fk_Blog_Id { get; set; }
        [DisplayName("Alt_Tag")]
        public string? AltTag { get; set; }
        [DisplayName("Cover_Image")]
        public bool CoverPic { get; set; }
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
