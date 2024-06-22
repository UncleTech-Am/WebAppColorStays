using LibCommon.DataTransfer;
using LibCommon.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UncleTech.Encryption;
using WebAppColorStays.Areas.ColorStays.CommonMethods;
using WebAppColorStays.Models;
using WebAppColorStays.Models.ViewModel;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{
    [Area("ColorStays")]
	[SessionCheck]
	[Authorize]
	public class HRmTariffNInventoryController : Controller
	{
		private readonly Paging paging;
		private readonly CountryTime countryTime;

		public HRmTariffNInventoryController()
		{
			paging = new Paging();
			countryTime = new CountryTime();
		}

		//Show the Title in View
		private void Title()
		{
			ViewBag.Title = "Room Inventory & Tariff";
		}
        //Ends

        public async Task<IActionResult> HotelList()
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Tuple<int, List<CsHotel>> list=null;
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("Hotel/Index/" + CompID + "/" + 0 + "/" + 0 + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsHotel>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        }
                        else { list = null; }
                    }
                }
                if (list.Item2.Count > 0)
                {
                    return View(list.Item2);
                }
                else
                {
                    ViewData["NoData"] = "Yes";
                }
            return View();
        }

        private void DropDown(string Token, string UserID, string CompID)
		{
            RyCrSsDropDown ry = new RyCrSsDropDown();
			Task<List<SelectListItem>> Hotel = ry.HotelDropDown(Token, CompID);
			Task.WaitAll(Hotel);
			ViewBag.DDHotel = Hotel;
		}

		//SHow the Tariff of a Particular date in RoomShift
		[HttpGet]
		public async Task<IActionResult> GetTariff(int UniqueId,string ToRoomTypeId, string BookingId, string TariffDate)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			CsRoomTariff csRoomTariff = new CsRoomTariff();
			using (HttpClient client = APIColorStays.Initial())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				using (var response = await client.GetAsync("HRmTariffNInventory/GetTariff/" + UniqueId + "/" + ToRoomTypeId + "/" + BookingId + "/" + TariffDate + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
				{
					var apiResponse = await response.Content.ReadAsStreamAsync();
					if (response.IsSuccessStatusCode)
					{
						csRoomTariff = await System.Text.Json.JsonSerializer.DeserializeAsync<CsRoomTariff>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
						return Json(csRoomTariff);
					}
				}
			}
			return null;
		}
		//ENds

		//Show the Occupancy Chart
		[HttpGet]
		public async Task<IActionResult> OccupancyChart(string? FromDate, string? ToDate)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var HotelID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["Hotel"]));

			OccupancyChart occupancyChartList = new OccupancyChart();
			var TimeZone = HttpContext.User.FindFirstValue(ClaimTypes.Country);
			if (FromDate == null)
			{
				occupancyChartList.FromDate = countryTime.GetTime(TimeZone).Date;
			}
			else
			{
				occupancyChartList.FromDate = Convert.ToDateTime(FromDate);
			}
			if (ToDate == null)
			{
				occupancyChartList.ToDate = countryTime.GetTime(TimeZone).Date.AddDays(+11);
			}
			else
			{
				occupancyChartList.ToDate = Convert.ToDateTime(ToDate);
			}

			CIndexSearchDates cIndex = new CIndexSearchDates();
			cIndex.FromDate = (DateTime)occupancyChartList.FromDate;
			cIndex.ToDate = (DateTime)occupancyChartList.ToDate;
			cIndex.CompId = CompID;
			cIndex.Condition = HotelID;

			string URLRoomType = "RoomType/RoomTypeList/" + CompID + "/" + HotelID;
			//string URLRoomNo = "HRoomNo/RoomNoList/" + CompID + "/" + HotelID;
			//string URLBooking = "Booking/BookingList/" + CompID + "/" + HotelID + "/" + occupancyChartList.FromDate + "/" + occupancyChartList.ToDate;
			//string URLBgRoomWise = "Booking/BgRoomWiseList/" + CompID + "/" + HotelID + "/" + occupancyChartList.FromDate + "/" + occupancyChartList.ToDate;

			Task<List<CsRoomType>> TRoomType = RoomTypeList(URLRoomType, TokenKey);
			//Task<List<CsRoomNo>> TRoomNo = RoomNoList(URLRoomNo, TokenKey);
			//Task<List<CsBooking>> TBooking = BookingList(URLBooking, TokenKey);
			//Task<List<CsBookingRoomWise>> TBgRoomWise = BgRoomWiseList(cIndex, TokenKey);

			Task.WaitAll(TRoomType);

			//occupancyChartList.RoomNo = TRoomNo.Result;
			occupancyChartList.RoomTypes = TRoomType.Result;
			//occupancyChartList.Bookings = TBooking.Result;
			//occupancyChartList.BgRoomWise = TBgRoomWise.Result;

			//Calculate the Count of Date from FromDate to ToDate.......................
			TimeSpan TS = Convert.ToDateTime(occupancyChartList.ToDate) - Convert.ToDateTime(occupancyChartList.FromDate);
			int daysDiff = Math.Abs(TS.Days);
			int dcount = Math.Abs(Convert.ToInt32(daysDiff) + 1);
			occupancyChartList.DayCount = dcount;
			//Ends..........................................

			return View("Occupancy", occupancyChartList);
		}
		//Ends


		//Get the Room Type List
		public async Task<List<CsRoomType>> RoomTypeList(string URL, string TokenKey)
		{
			List<CsRoomType> list = new List<CsRoomType>();
			using (HttpClient client = APIColorStays.Initial())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				using (var dresp = await client.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead))
				{
					var dapiResp = await dresp.Content.ReadAsStreamAsync();
					list = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsRoomType>>(dapiResp, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
				}
			}
			return list;
		}
		//Ends

		//GET:/HRmTariffNInventory/
		[HttpGet]
		public async Task<IActionResult> Index(string HotelId, string PageCall)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			Title();
			ViewData["ActionName"] = "Index";
			DropDown(TokenKey, UserID, CompID);
			var TimeZone = HttpContext.User.FindFirstValue(ClaimTypes.Country);
			CIndexSearchDates cIndex = new CIndexSearchDates();
			cIndex.FromDate = countryTime.GetTime(TimeZone).Date;
			cIndex.ToDate = countryTime.GetTime(TimeZone).Date.AddDays(+31);
			cIndex.CompId = CompID;
			cIndex.Condition = HotelId;
			ViewData["TodayDate"] = countryTime.GetTime(TimeZone).Date;

			CsRoomTariff data = new CsRoomTariff();
			using (HttpClient client = APIColorStays.Initial())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(cIndex), Encoding.UTF8, "application/json");
				using (var response = await client.PostAsync("HRmTariffNInventory/Search", content))
				{
					if (response.IsSuccessStatusCode)
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsRoomTariff>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                       
					}
					else { data = null; }
				}
			}
			if (PageCall != null) { return View("_IndexSearch", data); }
			return View("Index", data);
		}


		//Search the Inventory and Tariff of the Hotel
		//With or Without Dates
		[HttpPost]
		public async Task<IActionResult> Search(string? HotelId, string? FromDate, string? ToDate, IFormCollection fc)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			DropDown(TokenKey, UserID, CompID);

			CIndexSearchDates cIndex = new CIndexSearchDates();

			var TimeZone = HttpContext.User.FindFirstValue(ClaimTypes.Country);
			cIndex.FromDate = countryTime.GetTime(TimeZone).Date;
			cIndex.ToDate = countryTime.GetTime(TimeZone).Date.AddDays(+31);
			ViewData["TodayDate"] = countryTime.GetTime(TimeZone).Date;

			if (FromDate == null && fc["FromDate"] != "")
			{
				cIndex.FromDate = Convert.ToDateTime(fc["FromDate"]);
			}
			if (FromDate != null)
			{
				cIndex.FromDate = Convert.ToDateTime(FromDate);
			}

			if (ToDate == null && fc["ToDate"] != "")
			{
				cIndex.ToDate = Convert.ToDateTime(fc["ToDate"]);
			}
			if (ToDate != null)
			{
				cIndex.ToDate = Convert.ToDateTime(ToDate);
			}

			cIndex.CompId = CompID;
			if (HotelId == null)
			{
				cIndex.Condition = Convert.ToString(fc["HotelId"]);
			}
			else
			{
				cIndex.Condition = HotelId;
			}

			CsRoomTariff data = new CsRoomTariff();
			using (HttpClient client = APIColorStays.Initial())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(cIndex), Encoding.UTF8, "application/json");
				using (var response = await client.PostAsync("HRmTariffNInventory/Search", content))
				{
					if (response.IsSuccessStatusCode)
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsRoomTariff>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
					}
					else { data = null; }
				}
			}
			return View("_IndexSearch", data);
		}
		//Ends


		//Update the Invnetory of a Particular Date and Room
		//Single Update
		[HttpPost]
		public async Task<IActionResult> UpdateInventory(string? InventoryID, int CurrentRoom, string RoomTypeId, string HotelId, DateTime Dated)
		{
			Title();
			ViewData["AnName"] = "Create";
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool Success = false;
			CsRoomInventory tblroominventory = new CsRoomInventory();
			tblroominventory.Id = InventoryID;
			tblroominventory.Dated = Dated;
			tblroominventory.TotalInventory = CurrentRoom;
			tblroominventory.Fk_RoomType_Name = RoomTypeId;
			tblroominventory.Fk_Hotel_Name = Process.Decrypt(Base64UrlEncoder.Decode(HotelId));
			tblroominventory.CompId = CompID;
			tblroominventory.ModifiedBy = UserID;
			tblroominventory.CreatedBy = UserID;
			ViewData["ResponseName"] = "ShowValidation";
			CsRoomInventory data = new CsRoomInventory();
			if (ModelState.IsValid)
			{
				using (HttpClient client = APIColorStays.Initial())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
					StringContent content = new StringContent(JsonSerializer.Serialize(tblroominventory), Encoding.UTF8, "application/json");
					using (var response = await client.PostAsync("HRmTariffNInventory/UpdateInventory", content))
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						if (response.IsSuccessStatusCode)
						{
							data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsRoomInventory>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
							Success = true;
						}
						else
						{
							Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
							if (responsemsg != null && responsemsg.Message == "Duplicate")
							{   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
								ModelState.AddModelError("Name", "Duplicate Value, Already Exists !");
							}
							Success = false;
						}
					}
				}
				if (Success == true)
				{ return View("_FormSuccessMsg"); }
				else { return View("_ErrorGeneric"); }
			}
			return View("_ErrorGeneric");
		}
		//Ends


		//Update the Traiff of a Particular Date and Room
		//Single Update
		[HttpPost]
		public async Task<IActionResult> UpdateTariff(string? RoomTariffId, string OccTypeId, double OccRate, DateTime Date, string RoomTypeId, string PlanTypeId, string HotelId)
		{
			Title();
			ViewData["AnName"] = "Create";
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool Success = false;
			CsRoomTariff chotelroomtariff = new CsRoomTariff();
			chotelroomtariff.Id = RoomTariffId;
			chotelroomtariff.Dated = Date;
			chotelroomtariff.Fk_RoomType_Name = RoomTypeId;
			chotelroomtariff.Fk_PlanType_Name = PlanTypeId;
			chotelroomtariff.Fk_OccupancyType_Name = OccTypeId;
			chotelroomtariff.OccupancyCost = OccRate;
			chotelroomtariff.Fk_Hotel_Name = HotelId;
			chotelroomtariff.CompId = CompID;
			chotelroomtariff.ModifiedBy = UserID;
			ViewData["ResponseName"] = "ShowValidation";
			if (ModelState.IsValid)
			{
				using (HttpClient client = APIColorStays.Initial())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
					StringContent content = new StringContent(JsonSerializer.Serialize(chotelroomtariff), Encoding.UTF8, "application/json");
					using (var response = await client.PostAsync("HRmTariffNInventory/UpdateTariff", content))
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						if (response.IsSuccessStatusCode)
						{
							Success = true;
						}
						else
						{
							Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
							if (responsemsg != null && responsemsg.Message == "Duplicate")
							{   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
								ModelState.AddModelError("Name", "Duplicate Value, Already Exists !");
							}
							Success = false;
						}
					}
				}
				if (Success == true)
				{ return View("_FormSuccessMsg"); }
				else { return View("_ErrorGeneric"); }
			}
			return View("_ErrorGeneric");
		}
		//Ends


		//Show the PopUp for Bulk Tariff Update
		[HttpGet]
		public async Task<IActionResult> SwBkTfUePopUp(string PlanId, string PlanName, string RoomTypeId, string RoomType, string HotelId)
		{
			var TokenKey = Request.Cookies["JWToken"];
			CsRoomTariff csroomtariff = new CsRoomTariff();
			csroomtariff.Fk_PlanType_Name = PlanId;
			csroomtariff.PlanType = PlanName;
			csroomtariff.Fk_RoomType_Name = RoomTypeId;
			csroomtariff.Fk_Hotel_Name = HotelId;
			csroomtariff.CompId = Request.Cookies["CompanyID"];
			csroomtariff.RoomType = RoomType;
			List<CsOccupancyType> rmoymaplist = new List<CsOccupancyType>();
			using (HttpClient client = APIColorStays.Initial())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				using (var response = await client.GetAsync("RoomOccupancyMap/RoomMappedOcc/" + RoomTypeId, HttpCompletionOption.ResponseHeadersRead))
				{
					if (response.IsSuccessStatusCode)
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						csroomtariff.RoomOccMapList = await JsonSerializer.DeserializeAsync<List<CsOccupancyType>>(apiResponse, new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
					}
				}
			}
			return View("_BulkTariffUpdate", csroomtariff);
		}
		//Ends


		//Save the Bulk Tariff of Room and Plan
		[HttpPost]
		public async Task<IActionResult> BulkTariffUpdate(CsRoomTariff csroomtariff)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool Success = false;
			csroomtariff.CompId = CompID;
			csroomtariff.ModifiedBy = UserID;
			csroomtariff.CreatedBy = UserID;
			
			var TimeZone = HttpContext.User.FindFirstValue(ClaimTypes.Country);
			DateTime TodayDate = countryTime.GetTime(TimeZone).Date;

			if (csroomtariff.FromDate < TodayDate)
			{
				ViewData["ResponseName"] = "WrongInv";
				return View("_ErrorGeneric");
			}

			if (ModelState.IsValid)
			{
				using (HttpClient client = APIColorStays.Initial())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
					StringContent content = new StringContent(JsonSerializer.Serialize(csroomtariff), Encoding.UTF8, "application/json");
					using (var response = await client.PostAsync("HRmTariffNInventory/BulkTariffUpdate?FromDate=" + csroomtariff.FromDate + "&ToDate=" + csroomtariff.ToDate, content))
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						if (response.IsSuccessStatusCode)
						{
							Success = true;
						}
					}
				}
				if (Success == true)
				{ return View("_FormSuccessMsg"); }
				else { return View("_ErrorGeneric"); }
			}
			return View("_ErrorGeneric");
		}
		//Ends


		//Show the PopUp for Bulk Invnetory Update
		[HttpGet]
		public IActionResult SwBkIyUePopUp(string RoomTypeId, string Totalroomcount, string RoomType, string HotelId)
		{
			CsRoomInventory csroominventory = new CsRoomInventory();
			csroominventory.Fk_RoomType_Name = RoomTypeId;
			csroominventory.Fk_Hotel_Name = HotelId;
			csroominventory.CompId = Request.Cookies["CompanyID"];
			ViewData["Totalroomcount"] = Totalroomcount;
			ViewData["RoomType"] = RoomType;
			return View("_BulkInventoryUpdate", csroominventory);
		}
		//Ends


		//Save the Bulk Tariff of Room and Plan
		[HttpPost]
		public async Task<IActionResult> BulkInventoryUpdate(CsRoomInventory csroominventory)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool Success = false;
			csroominventory.CompId = CompID;
			csroominventory.ModifiedBy = UserID;
			csroominventory.CreatedBy = UserID;

			var TimeZone = HttpContext.User.FindFirstValue(ClaimTypes.Country);
			DateTime TodayDate = countryTime.GetTime(TimeZone).Date;

			if (csroominventory.FromDate < TodayDate)
			{
				ViewData["ResponseName"] = "WrongInv";
				return View("_ErrorGeneric");
			}

			if (ModelState.IsValid)
			{
				using (HttpClient client = APIColorStays.Initial())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
					StringContent content = new StringContent(JsonSerializer.Serialize(csroominventory), Encoding.UTF8, "application/json");
					using (var response = await client.PostAsync("HRmTariffNInventory/BulkInventoryUpdate?FromDate=" + csroominventory.FromDate + "&ToDate=" + csroominventory.ToDate, content))
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						if (response.IsSuccessStatusCode)
						{
							Success = true;
						}
						else
						{
							ViewData["ResponseName"] = "WrongInv";
							return View("_ErrorGeneric");
						}
					}
				}
				if (Success == true)
				{ return View("_FormSuccessMsg"); }
				else
				{
					ViewData["ResponseName"] = "Error";
					return View("_ErrorGeneric");
				}
			}
			return View("_ErrorGeneric");
		}
		//Ends


		//Block all Rooms of the Hotel on a Particular Date
		public async Task<IActionResult> BlockAllRooms(string HotelId, string Dated, string RoomTypeId, string RoomBlockStatus, string TotalRoomCount)
		{
			var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool Success = false;
			CsRoomInventory csroominventory = new CsRoomInventory();
			csroominventory.CompId = CompID;
			csroominventory.ModifiedBy = UserID;
			csroominventory.CreatedBy = UserID;
			csroominventory.Fk_Hotel_Name = Process.Decrypt(Base64UrlEncoder.Decode(HotelId));
			csroominventory.Fk_RoomType_Name = Process.Decrypt(Base64UrlEncoder.Decode(RoomTypeId));
			csroominventory.Dated = Convert.ToDateTime(Dated);
			csroominventory.TotalInventory = Convert.ToInt32(TotalRoomCount);
			if (ModelState.IsValid)
			{
				using (HttpClient client = APIColorStays.Initial())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
					StringContent content = new StringContent(JsonSerializer.Serialize(csroominventory), Encoding.UTF8, "application/json");
					using (var response = await client.PostAsync("HRmTariffNInventory/BlockAllRooms?RoomBlockStatus=" + RoomBlockStatus, content))
					{
						var apiResponse = await response.Content.ReadAsStreamAsync();
						if (response.IsSuccessStatusCode)
						{
							Success = true;
						}
					}
				}
				if (Success == true)
				{ return View("_FormSuccessMsg"); }
				else { return View("_ErrorGeneric"); }
			}
			return View("_ErrorGeneric");
		}
		//Ends
	}
}