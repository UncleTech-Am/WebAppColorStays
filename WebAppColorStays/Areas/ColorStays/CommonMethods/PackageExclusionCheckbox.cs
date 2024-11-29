using LibAuthService.ModelView;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class PackageExclusionCheckbox
    {
        public string? Id { get; set; }
        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }
        [NotMapped]
        public string? Package { get; set; }
        public string? Fk_PackageInclusion_Name { get; set; }
        public string? Name { get; set; }
        public PackageExclusionCheckbox()
        {
            CheckInEx = new List<PackageExclusionCheckbox>();
        }
        //String name of a checkbox
        public bool IsSelected { get; set; }
        public List<PackageExclusionCheckbox>? CheckInEx { get; set; }
        public string? CompId { get; set; }
    }
}
