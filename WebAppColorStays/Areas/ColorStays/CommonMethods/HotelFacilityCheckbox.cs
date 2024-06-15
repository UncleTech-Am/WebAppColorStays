using LibAuthService.ModelView;
using System.ComponentModel;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class HotelFacilityCheckbox
    {
        public string? Id { get; set; }
        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }

        public string? Hotel { get; set; }
        [DisplayName("HotelFacility")]
        public string? Fk_HotelFacility_Name { get; set; }

        public string? HotelFacility { get; set; }
        public HotelFacilityCheckbox()
        {
            CheckFacility = new List<HotelFacilityCheckbox>();
        }
        //String name of a checkbox
        public bool IsSelected { get; set; }
        public List<HotelFacilityCheckbox>? CheckFacility { get; set; }
        public string? CompId { get; set; }
    }
}
