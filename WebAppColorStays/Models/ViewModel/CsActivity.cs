using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsActivity
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeywords { get; set; }

        [DisplayName("ActivityType")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_ActivityType_Name { get; set; }
        [NotMapped]

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? ActivityType { get; set; }

        [DisplayName("Place")]
        [StringLength(450)]
        public string? Fk_Place_Name { get; set; }
        [NotMapped]
        [StringLength(450)]
        public string? Place { get; set; }
        [DisplayName("City")]
        [StringLength(450)]
        public string? Fk_City_Name { get; set; }
        [DisplayName("State")]
        [StringLength(450)]
        public string? Fk_State_Name { get; set; }
        [DisplayName("Country")]
        [StringLength(450)]
        public string? Fk_Country_Name { get; set; }
        
        [StringLength(450)]
        public string? URL { get; set; }
        [Required(ErrorMessage = "Please enter Activity Name.")]
        [Remote("CheckDuplicationActivity", "Activity", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        public string Name { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? AltTag { get; set; }
        public string? Description { get; set; }
        [DisplayName("Video Link")]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Video { get; set; }
        public double? StartingPrice { get; set; }
        public int? Duration { get; set; }
        [Remote("CheckDuplicationActivityRank", "Activity", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        public int? Rank { get; set; }
        public double? Price { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? MustKnowBefore { get; set; }
        public string? BestTimeToDo { get; set; }
        public string? HowToReach { get; set; }
        public string? About { get; set; }
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
