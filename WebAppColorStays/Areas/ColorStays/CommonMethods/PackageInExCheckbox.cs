using LibAuthService.ModelView;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class PackageInExCheckbox
    {
        public string? Id { get; set; }
        [DisplayName("Package")]
        public string? Fk_Package_Name { get; set; }
        [NotMapped]
        public string? Package { get; set; }
        public string? Fk_PackageExclusion_Name { get; set; }
        public string? Fk_PackageInclusion_Name { get; set; }
        public string? Name { get; set; }
        public PackageInExCheckbox()
        {
            CheckInEx = new List<PackageInExCheckbox>();
        }
        //String name of a checkbox
        public bool IsSelected { get; set; }
        public List<PackageInExCheckbox>? CheckInEx { get; set; }
        public string? CompId { get; set; }
    }
}
