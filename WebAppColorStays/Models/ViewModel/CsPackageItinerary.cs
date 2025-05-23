﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPackageItinerary
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }
        [NotMapped]
        public string? Package { get; set; }
        [Required(ErrorMessage = "Please enter DayNo.")]
        [Remote("CheckDuplicationPackageItineraryDayNo", "PackageItinerary", AdditionalFields = ("NameAction, Fk_Package_Name, Id"))]
        public int? DayNo { get; set; }
        [Required(ErrorMessage = "Please enter DayTitle.")]
        [Remote("CheckDuplicationPackageItinerary", "PackageItinerary", AdditionalFields = ("NameAction, Fk_Package_Name, Id"))]
        public string? DayTitle { get; set; }
        [AllowHtml]
        public string? DayActivity { get; set; }
        [DisplayName("Heading1")]
        public string? DayActivity1 { get; set; }
        public bool Sightseen { get; set; }
        [Remote("CheckDuplicationPackageItineraryImg1", "PackageItinerary", AdditionalFields = ("NameAction, Id"))]
        public string? Photo1 { get; set; }
        public string? AltTag1 { get; set; }
        public string? Description1 { get; set; }
        [NotMapped]
        public string? ImageUrl1 { get; set; }
        [NotMapped]
        public string? ImageUrl2 { get; set; }
        [NotMapped]
        public string? ImageUrl3 { get; set; }
        [NotMapped]
        public string? ImageUrl4 { get; set; }
        [NotMapped]
        public string? ImageUrl5 { get; set; }
        [Remote("CheckDuplicationPackageItineraryImg2", "PackageItinerary", AdditionalFields = ("NameAction, Id"))]
        public string? Photo2 { get; set; }
        public string? AltTag2 { get; set; }
        public string? Description2 { get; set; }
        [Remote("CheckDuplicationPackageItineraryImg3", "PackageItinerary", AdditionalFields = ("NameAction, Id"))]
        public string? Photo3 { get; set; }
        public string? AltTag3 { get; set; }
        public string? Description3 { get; set; }
        [Remote("CheckDuplicationPackageItineraryImg4", "PackageItinerary", AdditionalFields = ("NameAction, Id"))]
        public string? Photo4 { get; set; }
        public string? AltTag4 { get; set; }
        public string? Description4 { get; set; }
        [Remote("CheckDuplicationPackageItineraryImg5", "PackageItinerary", AdditionalFields = ("NameAction, Id"))]
        public string? Photo5 { get; set; }
        public string? AltTag5 { get; set; }
        public string? Description5 { get; set; }
        [DisplayName("Video Link")]
        public string? Video { get; set; }
        public bool Stay { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
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
