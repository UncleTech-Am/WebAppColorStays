namespace WebAppColorStays.Model.ViewModel
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CsBlogAuthor
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "This field is Reqiured!")]
        [DisplayName("User Detail")]
        public string? Fk_An_AspNetUsers_FullName { get; set; }
        [Required(ErrorMessage = "This field is Reqiured!")]
        public string? EmployeeName { get; set; }
        [DisplayName("Author Name")]
        [StringLength(100, ErrorMessage = "You can enter only 100 characters long!")]
        [Required(ErrorMessage = "This field is Reqiured!")]
        [Remote("CheckDuplicationBgAuthor", "BgAuthor", AdditionalFields = ("NameAction , Id, Fk_An_AspNetUsers_FullName"))]
        public string? Name { get; set; }
        [DisplayName("Image_URL")]
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? ImageUrl { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? About { get; set; }
        [DisplayName("Words_From")]
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? WordsFrom { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? Facebook { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? Instagram { get; set; }
        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? LinkedIn { get; set; }
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
