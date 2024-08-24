using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace WebAppColorStays.Models.ViewModel
{
	public class CsHotelCityList
	{
		[Key]
		[StringLength(450)]
		public string? Id { get; set; }
        public string? Label { get; set; }
        [DisplayName("City")]
        [Remote("CheckDuplicationHotelCityList", "HotelCityList", AdditionalFields = ("NameAction, Id"))]
        public string? Fk_City_Name { get; set; }
		[NotMapped]
		public string? City { get; set; }
        public string? Fk_Country_Name { get; set; }
        public string? SEOTitle { get; set; }
		public string? SEODescription { get; set; }
		public string? SEOKeywords { get; set; }
		public string? URL { get; set; }
		public string? Name { get; set; }
		public string? Video { get; set; }
		public string? CoverImage { get; set; }
		public string? CoverAltTag { get; set; }
		[NotMapped]
		public string? CoverImageName { get; set; }
        public string? TagLine { get; set; }
        public string? OtherDes { get; set; }
        public string? Description { get; set; }
        public string? StructuredData { get; set; }
        public bool FreezeStatus { get; set; }
		public string? FreezedBy { get; set; }
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
