﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsMustExperience
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? URL { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public string? AltTag { get; set; }
        public string? TagLine { get; set; }
        public string? Description { get; set; }
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
