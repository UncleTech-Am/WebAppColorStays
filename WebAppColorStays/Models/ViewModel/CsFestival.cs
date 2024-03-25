﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsFestival
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Fk_Place_Name { get; set; }
        [NotMapped]
        public string? Place { get; set; }
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        public string? Fk_Country_Name { get; set; }
        [NotMapped]
        public string? Country { get; set; }
        public string? Fk_FestivalType_Name { get; set; }
        public string? PopularReligion { get; set; }
        public string? History { get; set; }
        public string? CelebratedFor { get; set; }
        public string? Month { get; set; }
        public string? Region { get; set; }
        public string? Description { get; set; }
        public string? Facts { get; set; }
        public string? Rituals { get; set; }
        public string? SpecialAboutFestival { get; set; }
        public int? TotalDays { get; set; }
        public bool ShowPlaceFestival { get; set; }
        public bool ShowCityFestival { get; set; }
        public bool ShowStateFestival { get; set; }
        public bool ShowCountryFestival { get; set; }
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
