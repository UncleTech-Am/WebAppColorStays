﻿using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsPackage
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? SEOTitle { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeywords { get; set; }
        public string? Fk_TravelAgent_Name { get; set; }
        public string? Transport { get; set; }
        public double? Commission { get; set; }
        public string? Stay { get; set; }
        public string? Sightseeing { get; set; }
        public string? PackageHighlight { get; set; }
        public string? Overview { get; set; }
        public int? Duration { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public int? LocationDuration { get; set; }
        public string? StartingFrom { get; set; }
        public double? Price { get; set; }
        public string? Fk_Currency_Name { get; set; }
        public string? Photo1 { get; set; }
        public string? Photo2 { get; set; }
        public string? Photo3 { get; set; }
        public string? Photo4 { get; set; }
        public string? Photo5 { get; set; }
        public string? Photo6 { get; set; }
        public string? Photo7 { get; set; }
        public string? Photo8 { get; set; }
        public string? Photo9 { get; set; }
        public string? Photo10 { get; set; }
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