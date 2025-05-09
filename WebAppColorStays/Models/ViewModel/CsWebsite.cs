using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsWebsite
    {
        [Key]
        public string? Id { get; set; }
        public string? Label { get; set; }
        [Remote("CheckDuplicationWebsite", "Website", AdditionalFields = ("NameAction, Id"))]
        public string? WebsiteName { get; set; }
        public string? Domain { get; set; }
        public string? HostingName { get; set; }
        public string? HostNumber { get; set; }
        public string? FtpuserName { get; set; }
        public string? Ftppassword { get; set; }
        public DateTime? HostingExpireOn { get; set; }
        public string? WebsiteProtocol { get; set; }
        public string? Remarks { get; set; }
        public bool FreezeStatus { get; set; }
        public string? FreezedBy { get; set; }
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
