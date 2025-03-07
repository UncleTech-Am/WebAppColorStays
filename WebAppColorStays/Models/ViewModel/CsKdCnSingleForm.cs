using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppColorStays.Areas.ColorStays.CommonMethods;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsKdCnSingleForm
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        public string? SEOTitle { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeywords { get; set; }
        [DisplayName("PackageType")]
        public string? Fk_PackageType_Name { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverAltTag { get; set; }
        [NotMapped]
        public string? CoverImageName { get; set; }
        [ForeignKey("TblCountry")]
        public string? Fk_Country_Name { get; set; }
        [NotMapped]
        public string? Country { get; set; }
        [DisplayName("Category")]
        public string? Fk_KdCnCategory_Name { get; set; }
        [StringLength(450)]
        [Required(ErrorMessage = "Please enter Name.")]
        [Remote("CheckDuplicationKdCnSingleForm", "KdCnSingleForm", AdditionalFields = ("NameAction, Id"))]
        public string? Name { get; set; }
        public string? URL { get; set; }
        public string? Description { get; set; }
        [DisplayName("No_Of_Nights")]
        public int? Duration { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool Bus { get; set; }
        public bool Cab { get; set; }
        public bool Airplane { get; set; }
        public bool Train { get; set; }
        public int? NoOfPerson { get; set; }
        public double? FromPrice { get; set; }
        public double? ToPrice { get; set; }
        public bool IsStartingFrom { get; set; }
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

        [NotMapped]
        public KeywordCityCheckbox? CheckCityList { get; set; }
        [NotMapped]
        public KeywordStateCheckbox? CheckStateList { get; set; }

    }
}
