using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsCity
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? URL { get; set; }
        public string Name { get; set; }
        public string? Area { get; set; }
        public string? History { get; set; }
        public string? Fact { get; set; }
        public string? Stoories { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public int? Rank { get; set; }
        public int? NoOfDays { get; set; }
        public int? NoOfPlaces { get; set; }
        public string? MonthTemperature { get; set; }
        public string? ClothingToCarry { get; set; }
        public string? Footwear { get; set; }
        public string? Fk_Terrain_Name { get; set; }
        public string? PeakSeason { get; set; }
        public string? OffSeason { get; set; }
        public string? MidSeason { get; set; }
        public int? Rating { get; set; }
        public double? LowestPackage { get; set; }
        public int? PinCode { get; set; }
        public string? OurTips1 { get; set; }
        public string? OurTips2 { get; set; }
        public string? OurTips3 { get; set; }
        public string? OutTips4 { get; set; }
        public string? OurTips5 { get; set; }
        public string? NightLife { get; set; }
        public int? TelephoneCode { get; set; }
        public string? TopThingsToKnow { get; set; }
        public string? Itinerary { get; set; }
        public string? BestTimeToGo1 { get; set; }
        public string? BestTimeToGo2 { get; set; }
        public string? BestTimeToGo3 { get; set; }
        public string? BestTimeToGo4 { get; set; }
        public string? BestTimeToGo5 { get; set; }
        public bool? FreezeStatus { get; set; }
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
