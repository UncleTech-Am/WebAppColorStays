using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsCountry
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? URL { get; set; }
        public string Name { get; set; }
        public string? History { get; set; }
        public string? Fact { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Area { get; set; }
        public int? Population { get; set; }
        public string? PopularReligion { get; set; }
        public string? Fk_Airport_Name { get; set; }
        public string? Fk_Railway_Name { get; set; }
        public int? Rating { get; set; }
        public int? InternationalAirportCount { get; set; }
        public int? DomesticAirportCount { get; set; }
        public int? RailwayStationCount { get; set; }
        public bool? FreezeStatus { get; set; }
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
