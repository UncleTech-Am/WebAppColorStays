﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsMapKdGeneratedCity
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Label { get; set; }
        [StringLength(450)]
        public string? Fk_KdGenerated_Name { get; set; }
        public string? Fk_KdCnForm_Name { get; set; }
        public string? Fk_KdCnSingleForm_Name { get; set; }
        public string? Fk_KdHlGenerated_Name { get; set; }
        public string? Fk_KdCnHlForm_Name { get; set; }
        public string? Fk_City_Name { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
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
