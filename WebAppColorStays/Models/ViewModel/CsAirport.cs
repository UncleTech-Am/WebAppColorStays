using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsAirport
    {
        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [Remote("CheckDuplicationAirport", "Airport", AdditionalFields = ("NameAction, Fk_Place_Name, Id"))]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string Name { get; set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string? Place { get; set; }
        [DisplayName("Place")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_Place_Name { get; set; }

        [DisplayName("City")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_City_Name { get; set; }

        [DisplayName("State")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_State_Name { get; set; }

        [DisplayName("Country")]
        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? Fk_Country_Name { get; set; }

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? URL { get; set; }
        public bool International { get; set; }
        public bool Domestic { get; set; }
        public int? PinCode { get; set; }

        [StringLength(1000, ErrorMessage = "You can enter only 1000 characters long!")]
        public string? Address { get; set; }
        public int? ContactNumber1 { get; set; }
        public int? ContactNumber2 { get; set; }
        public int? ContactNumber3 { get; set; }
        public bool DutyFreeShop { get; set; }
        public bool Lounge { get; set; }
        public int? TotalRunways { get; set; }
        public int? TotalTerminals { get; set; }

        [StringLength(50, ErrorMessage = "You can enter only 50 characters long!")]
        public string? Nearby { get; set; }
        public string? History { get; set; }
        public string? VehicleParking { get; set; }

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? OwnedBy { get; set; }

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
        public string? OperatedBy { get; set; }

        [StringLength(10, ErrorMessage = "You can enter only 10 characters long!")]
        public string? AirportCode { get; set; }
        public string? Facilities { get; set; }
        public string? Terminals { get; set; }
        public string? LocationDescription { get; set; }
        public string? ConnectivityDescription { get; set; }
        public string? RunWayDescription { get; set; }
        public string? HowToReach { get; set; }
        public string? Upcoming { get; set; }

        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Photo1 { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AltTag1 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
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

        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Photo2 { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AltTag2 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Description2 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Photo3 { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AltTag3 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Description3 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Photo4 { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AltTag4 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Description4 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Photo5 { get; set; }

        [StringLength(250, ErrorMessage = "You can enter only 250 characters long!")]
        public string? AltTag5 { get; set; }
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? Description5 { get; set; }
        [DisplayName("Video Link1")]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Video1 { get; set; }
        [DisplayName("Video Link2")]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Video2 { get; set; }
        [DisplayName("Video Link3")]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Video3 { get; set; }
        [DisplayName("Video Lin4")]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Video4 { get; set; }
        [DisplayName("Video Link5")]
        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]

        public string? Video5 { get; set; }

        [StringLength(500, ErrorMessage = "You can enter only 500 characters long!")]
        public string? OutTransportation { get; set; }
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
