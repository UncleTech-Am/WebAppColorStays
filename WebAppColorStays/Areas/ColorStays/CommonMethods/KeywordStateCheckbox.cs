using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class KeywordStateCheckbox
    {
        public string? Id { get; set; }
        [DisplayName("State")]
        public string? Fk_State_Name { get; set; }
        [NotMapped]
        public string? State { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? Fk_KdGenerated_Name { get; set; }
        public KeywordStateCheckbox()
        {
            CheckState = new List<KeywordStateCheckbox>();
        }
        //String name of a checkbox
        public bool IsSelected { get; set; }
        public List<KeywordStateCheckbox>? CheckState { get; set; }
        public string? CompId { get; set; }
    }
}
