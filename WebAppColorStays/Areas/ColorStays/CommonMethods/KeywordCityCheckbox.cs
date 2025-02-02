using LibAuthService.ModelView;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class KeywordCityCheckbox
    {
        public string? Id { get; set; }
        [DisplayName("City")]
        public string? Fk_City_Name { get; set; }
        [NotMapped]
        public string? City { get; set; }
        public string? Fk_State_Name { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? Fk_KdGenerated_Name { get; set; }
        public KeywordCityCheckbox()
        {
            CheckCity = new List<KeywordCityCheckbox>();
        }
        //String name of a checkbox
        public bool IsSelected { get; set; }
        public List<KeywordCityCheckbox>? CheckCity { get; set; }
        public string? CompId { get; set; }
    }
}
