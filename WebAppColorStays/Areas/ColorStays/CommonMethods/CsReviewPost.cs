using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.CommonMethod
{
    public class CsReviewPost
    {
        public CsReviewPost()
        {
            QuestionList = new List<CsReviewPost>();
        }
        [Key]
        [StringLength(450)]
        public string? QuestionId { get; set; }
        [DisplayName("ReviewerName")]
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? QuestionName { get; set; }
        public string? Fk_ReviewFor_Name { get; set; }
        public string? ReviewFor { get; set; }
        public bool? IsRating { get; set; }
        public int? SrNo { get; set; }
        public bool? IsBool { get; set; }
        public bool? IsText { get; set; }
        public string? Type { get; set; }
        public string? Statement { get; set; }
        public bool? IsBoolReview { get; set; }
        public string? ReviewId { get; set; }
        public string? Fk_ReviewQuestion_Name { get; set; }
        public string? Fk_Package_Name { get; set; }
        public string? Fk_Hotel_Name { get; set; }
        [DisplayName("Place")]
        public string? Fk_Place_Name { get; set; }
        public string? Place { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        public string? City { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        public string? State { get; set; }
        [DisplayName("Country")]
        public string? Fk_Country_Name { get; set; }
        public string? Country { get; set; }
        public string? ReviewName { get; set; }
        public int? Rating { get; set; }
        public string? Text { get; set; }
        public bool? IsPhotoUploaded { get; set; }
        public bool? IsRejected { get; set; }
        public string? Condition { get; set; }


        public DateTime? CreatedOn { get; set; }
        [StringLength(450)]
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [StringLength(450)]
        public string? ModifiedBy { get; set; }
        [StringLength(450)]
        public string? CompId { get; set; }
        [Timestamp]
        [ConcurrencyCheck]
        public byte[]? RowVersion { get; set; }
        public List<CsReviewPost> QuestionList { get; set; }
    }
}
