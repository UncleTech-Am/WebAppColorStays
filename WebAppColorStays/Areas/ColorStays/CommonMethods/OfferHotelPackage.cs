using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using LibAuthService.ModelView;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class OfferHotelPackage
    {
        public string? Id { get; set; }
        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }

        public string? Hotel { get; set; }

        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }

        public string? Package { get; set; }
        [DisplayName("Offer")]
        public string? Fk_Offer_Name { get; set; }
        public string? Offer { get; set; }

        public OfferHotelPackage()
        {
            CheckHotel = new List<OfferHotelPackage>();
        }

        public List<OfferHotelPackage>? CheckHotel { get; set; }
        public bool IsSelected { get; set; }
    }
}
