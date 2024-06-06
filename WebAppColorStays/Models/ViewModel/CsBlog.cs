using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Model.ViewModel
{
    public partial class CsBlog 
    {
        public string[]? Tags { get; set; }
        public string? Id { get; set; }
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        [Remote("CheckDuplicationBg", "Bg", AdditionalFields = ("NameAction , Id"))]
        public string? Title { get; set; }
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        [DisplayName("Title_Description")]
        public string? TitleDescription { get; set; }
        [DisplayName("Category")]
        public string? Fk_BlogCategory_Name { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Description { get; set; }
        [DisplayName("Author")]
        public string? Fk_BlogAuthor_Name { get; set; }
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
        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }
        [NotMapped]
        public string? Package { get; set; }
        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }
         [NotMapped]
        public string? Hotel { get; set; }
        public bool ShowInCity { get; set; }
        public bool ShowInState { get; set; }
        public bool ShowInCountry { get; set; }
        [DisplayName("Blog_URL")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? BlogURL { get; set; }
        public int? Views { get; set; }
        public int? Liked { get; set; }
        public int? Unlike { get; set; }
        public int? Heart { get; set; }
        [DisplayName("SEO_Keyword")]
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? SEOKeyword { get; set; }
        [DisplayName("SEO_Description")]
        [StringLength(170, ErrorMessage = "You can enter only 170 characters long!")]
        public string? SEODescription { get; set; }
        [DisplayName("SEO_Title")]
        [StringLength(70, ErrorMessage = "You can enter only 70 characters long!")]
        public string? SEOTitle { get; set; }
        [DisplayName("Image_Status")]
        public bool? ImageStatus { get; set; }
        public string? ImageUrl { get; set; }
        [DisplayName("Video_Status")]
        public bool? VideoStatus { get; set; }
        [DisplayName("Image_Crousel")]
        public int? ImageCrouselKey { get; set; }
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
