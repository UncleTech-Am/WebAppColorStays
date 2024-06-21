using LibAuthService.ModelView;
using System.ComponentModel;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class HotelPlanCheckbox
    {
        public string? Id { get; set; }
        [DisplayName("Hotel")]
        public string? Fk_Hotel_Name { get; set; }
        public string? Hotel { get; set; }
        [DisplayName("PlanType")]
        public string? Fk_PlanType_Name { get; set; }
        public string? PlanType { get; set; }
        [DisplayName("HotelPlan")]
        public string? Fk_HotelPlanType_Name { get; set; }
        public string? HotelPlan { get; set; }
        public HotelPlanCheckbox()
        {
            CheckFacility = new List<HotelPlanCheckbox>();
        }
        //String name of a checkbox
        public bool IsSelected { get; set; }
        public List<HotelPlanCheckbox>? CheckFacility { get; set; }
        public string? CompId { get; set; }
    }
}
