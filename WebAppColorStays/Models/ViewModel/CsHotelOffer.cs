using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsHotelOffer
    {
        public CsHotelOffer()
        {
            CsHotelOfferPlanRoom = new List<CsHotelOfferPlanRoom>();
            CsRoomType = new List<CsRoomType>();
        }

        [Key]
        [StringLength(450)]
        public string? Id { get; set; }
        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }
        [NotMapped]
        public string? Hotel { get; set; }
        [DisplayName("HotelOfferType")]
        public string? Fk_HotelOfferType_Name { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public string? ImageName { get; set; }
        public DateTime? SyStartDate { get; set; }
        public DateTime? SyEndDate { get; set; }
        public bool IsNoSyEndDate { get; set; }
        public DateTime? BgStartDate { get; set; }
        public DateTime? BgEndDate { get; set; }
        public bool IsNoBgEndDate { get; set; }
        public bool IsRefundable { get; set; }
        public bool IsPayAtHotel { get; set; }
        public bool IsApplyForAll { get; set; }
        public bool IsB2C { get; set; }
        public bool ISBlackOutDate { get; set; }
        public string Title { get; set; }
        public bool IsApplyDis { get; set; }
        public double? DiscountPercent { get; set; }
        public double? FixedDiscountPrice { get; set; }
        public double? DisForLoggedInUser { get; set; }
        public string? TagLine { get; set; }
        public string? GeneratedCode { get; set; }
        public string? CommonCode { get; set; }
        public bool FreezeStatus { get; set; }
        public string? FreezedBy { get; set; }

        [StringLength(450, ErrorMessage = "You can enter only 450 characters long!")]
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
        public List<CsHotelOfferPlanRoom>? CsHotelOfferPlanRoom { get; set; }
        [NotMapped]
        public List<CsRoomType>? CsRoomType { get; set; }

    }
}
