﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPackage
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
        [DisplayName("Currency")]
        public string? Fk_Currency_Name { get; set; }
        [NotMapped]
        public string? Currency { get; set; }
        public bool Sightseeing { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [Remote("CheckDuplicationPackage", "Package", AdditionalFields = ("NameAction, Id"))]
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? StructuredData { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverAltTag { get; set; }
        [NotMapped]
        public string? CoverImageName { get; set; }
        public int? NoOfPerson { get; set; }
        public string? URL { get; set; }
        public string? Transport { get; set; }
        public string? Stay { get; set; }
        [AllowHtml]
        public string? PackageHighlight { get; set; }
        [AllowHtml]
        public string? Overview { get; set; }
        [DisplayName("No_Of_Nights")]
        public int? Duration { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public string? LocationDuration { get; set; }
        [DisplayName("PackageType")]
        public string? Fk_PackageType_Name { get; set; }
        [NotMapped]
        public string? PackageType { get; set; }
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
        public double? Price { get; set; }
        public int? Discount { get; set; }
        [DisplayName("Video Link")]
        public string? Video { get; set; }
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
