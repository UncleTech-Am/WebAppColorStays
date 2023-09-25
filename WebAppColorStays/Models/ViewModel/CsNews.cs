using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsNews
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        [NotMapped]
        public string? Country { get; set; }
        [DisplayName("Activity")]
        public string? Fk_Activity_Name { get; set; }
        [NotMapped]
        public string? Activity { get; set; }
        [DisplayName("NewsCategory")]
        public string? Fk_NewsCategory_Name { get; set; }
        [NotMapped]
        public string? NewsCategory { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? News { get; set; }
        public string? CoverPicture { get; set; }
        public int? LikeCount { get; set; }
        public int? ShareCount { get; set; }
        public int? DislikeCount { get; set; }
        public string? Video1 { get; set; }
        public string? Video2 { get; set; }
        public string? Video3 { get; set; }
        public string? Video4 { get; set; }
        public string? Video5 { get; set; }
        public string? Photo1 { get; set; }
        public string? Photo2 { get; set; }
        public string? Photo3 { get; set; }
        public string? Photo4 { get; set; }
        public string? Photo5 { get; set; }
        public string? PhotoDesc1 { get; set; }
        public string? PhotoDesc2 { get; set; }
        public string? PhotoDesc3 { get; set; }
        public string? PhotoDesc4 { get; set; }
        public string? PhotoDesc5 { get; set; }
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
