using LibCommon.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LibCommon.DataTransfer;
using LibCompanyService.Models.ViewCompany;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UncleTech.Encryption;
using WebAppColorStays.Models.ViewModel;
using WebAppColorStays.Areas.ColorStays.CommonMethods;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using LibCommon.APICommonMethods;
using System.Collections.Generic;
using LibAuthService.ModelView;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class HotelController : Controller
    {
        private readonly Paging paging;
        private readonly RyCSImage ryCsImage;

        public HotelController()
        {
            paging = new Paging();
            ryCsImage = new RyCSImage();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "Hotel";
        }
        //Ends

        [HttpPost]
        public async Task<IActionResult> SaveImage(string HId, string HName)
        {
            var TokenKey = Request.Cookies["JWToken"];

            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = HttpContext.Request.Form.Files;
            List<CsHotelImageVideo> photosList = new List<CsHotelImageVideo>();
            var RoomId = "null";
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("HotelImageVideo/Index/" + HId +"/" + RoomId))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    photosList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsHotelImageVideo>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
                List<string> ExistingImageList = null;
                List<string> NewImageList;
                if (photosList != null)
                {
                    ExistingImageList = photosList.Select(x => x.Name).ToList();
                }
                NewImageList = files.Select(x => x.FileName).ToList();

                var a = ExistingImageList.Intersect(NewImageList);

                foreach (var file in files)
                {
                    if (a.Contains(file.FileName))
                    {
                        var data = a.Select(x => new
                        {
                            id = x
                        }).ToList();
                        return Json(data);
                    }
                    var fileName = file.FileName;
                    //StringContent content = new StringContent(JsonSerializer.Serialize(file), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Hotel/SaveImage/?HId=" + HId + "&CompId=" + CompID + "&UserId=" + UserID + "&FileName=" + fileName, null))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            return View("Error");
                        }
                    }

                    if (file.Length > 0)
                    {
                        Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Hotel");
                        Task.WaitAll(TImgUpload);
                        if (TImgUpload.Result == "Error")
                        {
                            return View("Error");
                        }
                    }
                }
            }
            return Json(new { Message = "Saved" });
        }
        
        //ItemAutoComplete
        [HttpGet]
        public async Task<JsonResult> SuggestHotel(string term)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var UserID = Request.Cookies["UserID"];
            List<CsAutocomplete> list = new List<CsAutocomplete>();
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                HttpResponseMessage res = await client.GetAsync("Hotel/SuggestHotel/" + term + "/" + CompId, HttpCompletionOption.ResponseHeadersRead);
                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStreamAsync().Result;
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsAutocomplete>>(results, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    var data = list.Select(x => new
                    {
                        id = Base64UrlEncoder.Encode(Process.Encrypt(x.Id)),
                        value = x.Value,
                        label = x.Label,

                    }).ToList();
                    return Json(data);
                }
            }
            return null;
        }
        //Ends

        //Drop Down
        public async void DropDown(string CompId, string Token)
        {
            RyCrSsDropDown ry = new RyCrSsDropDown();
            string URLChainBrand = "HotelChainBrand/DropDown/" + CompId;
            string URLHotelType = "HotelType/DropDown/" + CompId;
            string URLCurrency = "Currency/DropDown/" + CompId;
            try
            {
                Task<List<SelectListItem>> ChainBrand = ry.DDColorStaysAPI(URLChainBrand, Token);
                Task<List<SelectListItem>> HotelType = ry.DDColorStaysAPI(URLHotelType, Token);
                Task<List<SelectListItem>> Currency = ry.DDColorStaysAPI(URLCurrency, Token);
                Task.WaitAll(ChainBrand, HotelType, Currency);
                ViewBag.ChainBrand = ChainBrand;
                ViewBag.HotelType = HotelType;
                ViewBag.Currency = Currency;
            }
            catch (Exception ex) { }

        }

        //Set the Pagination values to the ViewData
        private void PaginationViewData(int? PgSelectedNum, int? ListCount, int? PgSize)
        {
            Paging paging = new Paging(PgSelectedNum, ListCount, PgSize);
            ViewData["ShowNextBtn"] = paging.ShowPageNextLink;
            ViewData["ShowPrevBtn"] = paging.ShowPagePreviousLink;
            ViewData["PageStartNo"] = paging.PageStartNumber;
            ViewData["PageSizeSelectList"] = paging.PageSizeSelectList;
            ViewData["PageSelectButtonCount"] = paging.PageSelectButtonCount;
            ViewData["PageNumberSelected"] = paging.PageNumberSelected;
            ViewData["PageRecordSize"] = paging.PageRecordSize;
            ViewData["NetRecords"] = paging.NetRecords;
        }
        //Ends

        //Display the Pagination 
        [HttpGet]
        public IActionResult Pagination(int? PgSelectedNum, int? PgSize, string SearchType, string NetRecords)
        {         
			Title();
            switch (SearchType)
            {
                case "DateSearch":
                    ViewData["ActionName"] = "DateSearch";
                    ViewData["FormID"] = "DateSearchID";
                    ViewData["SearchType"] = "DateSearch";
                    break;
                case "TableSearch":
                    ViewData["ActionName"] = "TableSearch";
                    ViewData["FormID"] = "TableSearchID";
                    ViewData["SearchType"] = "TableSearch";
                    break;
                case "FilterSearch":
                    ViewData["ActionName"] = "FilterSearch";
                    ViewData["FormID"] = "FilterSearchID";
                    ViewData["SearchType"] = "FilterSearch";
                    break;
                case "NoSearch":
                    ViewData["ActionName"] = "Index";
                    ViewData["FormID"] = "NoSearchID";
                    ViewData["SearchType"] = "NoSearch";
                    break;

                default:
                    break;
            }

            int NetRecord = Convert.ToInt32(NetRecords);
            ViewData["NetRecord"] = NetRecord;
            PaginationViewData(PgSelectedNum, NetRecord, PgSize);
            return View("_Pagination");
        }
        //Ends

        //Give the list of the data
        public async Task<Tuple<int, List<CsHotel>>> AllDataList(int? PgSize, int? PgSelectedNum)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tuple<int, List<CsHotel>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/index/" + CompID + "/" + PgSize + "/" + PgSelectedNum + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsHotel>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else{ list = null; }
                }
            }
            return list;
        }
        //Ends


        //GET:/Hotel/
        [HttpGet]
        public async Task<IActionResult> Index(int? PgSelectedNum, int? PgSize, string PageCall, string? Id, string? Name)
        {         
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            //Display the Dropdown of the Table fields in Search Data Popup
            GetClassMember<CsHotel> getClassMember = new GetClassMember<CsHotel>();
            CsHotel CsHotel = new CsHotel();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsHotel), "Value", "DisplayName");
            //Ends

            try //Pagination and List of data Code
            {
                Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No
                Task<Tuple<int, List<CsHotel>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination

                ViewData["ActionName"] = "Index";
                ViewData["FormID"] = "NoSearchID";
                ViewData["SearchType"] = "NoSearch";
                ViewData["HId"] = Id;
                ViewData["HName"] = Name;

                if (PageCall == "ShowIxSh") { return View("_IndexSearch", ReturnDataList.Result.Item2); }

                if (PageCall != null) { return View("_IndexData", ReturnDataList.Result.Item2); }

                return View(ReturnDataList.Result.Item2);
            }
            catch(Exception ex)
            { 
                return View("Error");
            }
        }


        //This standard method used in index page for getting data between 2 dates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DateSearch(IFormCollection fc)
        {
            bool Success = false;
            int PgSelectedNum = Convert.ToInt32(fc["PageNoSelected"]);
            int PgSize = Convert.ToInt32(fc["PageSize"]);
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try //Pagination and List of data Code
            {
                Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No
                CIndexSearchDates cIndex = new CIndexSearchDates();
                cIndex.FromDate = Convert.ToDateTime(fc["FromDate"]);
                cIndex.ToDate = Convert.ToDateTime(fc["ToDate"]);
                cIndex.CurrentUserId = UserID;
                cIndex.PageSize = pagedata.Item1;
                cIndex.PageSelectedNum = pagedata.Item2;
                cIndex.CompId = CompID;
                Tuple<int, List<CsHotel>> list;
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    //Get the List of data
                    using (var response = await client.PostAsJsonAsync("Hotel/DateSearch/", cIndex))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsHotel>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (response.IsSuccessStatusCode)
                        { Success = true; }
                        else{ Success = false; }
                    }
                }
                if (Success == true)
                {
                    PaginationViewData(pagedata.Item2, list.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["ActionName"] = "DateSearch";
                    ViewData["FormID"] = "DateSearchID";
                    ViewData["SearchType"] = "DateSearch";
                    return View("_IndexData", list.Item2);
                }
                else { return View("Errors"); }
            }
            catch
            {
                return View("Error");
            }
        }
        //Ends


        //Search the Data according to the table fileds in the Index
        [HttpPost]
        public async Task<IActionResult> TableSearch(CsHotel CsHotel, IFormCollection fc)
        {
            bool Success = false;
            int PgSelectedNum = Convert.ToInt32(fc["PageNoSelected"]);
            int PgSize = Convert.ToInt32(fc["PageSize"]);
            int ListCount = 0;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No

            Tuple<int, List<CsHotel>> list;

            using (HttpClient client = APIColorStays.Initial())
            {
                CsHotel.CreatedBy = UserID;
                CsHotel.CompId = CompID;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(CsHotel), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Hotel/TableSearch/?PageSelectedNum=" + pagedata.Item2 + "&PageSize=" + pagedata.Item1, content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsHotel>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    if (response.IsSuccessStatusCode)
                    { Success = true; }
                    else { Success = false; }
                }

                //Pagination Code
                PaginationViewData(PgSelectedNum, list.Item1, PgSize);//Give the ViewData value for Pagination
                ViewData["ActionName"] = "TableSearch";
                ViewData["FormID"] = "TableSearchID";
                ViewData["SearchType"] = "TableSearch";
                if (Success == true)
                { return View("_IndexData", list.Item2); }
                else { return View("Errors"); }
            }
        }
        //Ends

        
        //Search the Data according to the Fields Selected in Search Data View
        [HttpPost]
        public async Task<IActionResult> FilterSearch(CIndexSearchFilter indexsearchfilter, IFormCollection fc)
        {
            GetClassMember<CsHotel> getClassMember = new GetClassMember<CsHotel>();
            CsHotel CsHotel = new CsHotel();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsHotel), "Value", "DisplayName");
            //Creating Search Filter List with class member Property Name
            Dictionary<string, string> fields = getClassMember.GetPropertyName(CsHotel);
            foreach (var item in indexsearchfilter.IndexSearchList)
            { item.Name = fields[item.Name]; }
            //Ends
            
            bool Success = false;
            int PgSelectedNum = Convert.ToInt32(fc["PageNoSelected"]);
            int PgSize = Convert.ToInt32(fc["PageSize"]);
            int ListCount = 0;

            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();
            Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No

            Tuple<int, List<CsHotel>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
               indexsearchfilter.CurrentUserId = UserID;
                indexsearchfilter.CompId = CompID;
                indexsearchfilter.PageSelectedNum = pagedata.Item2;
                indexsearchfilter.PageSize = pagedata.Item1;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(indexsearchfilter), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Hotel/FilterSearch", content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsHotel>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    if (response.IsSuccessStatusCode)
                    { Success = true; }
                    else { Success = false; }
                }

                //Pagination Code
                PaginationViewData(PgSelectedNum, list.Item1, PgSize);//Give the ViewData value for Pagination

                ViewData["ActionName"] = "FilterSearch";
                ViewData["FormID"] = "FilterSearchID";
                ViewData["SearchType"] = "FilterSearch";
                if (Success == true)
                { return View("_IndexData", list.Item2); }
                else { return View("Errors"); }
            }
        }
        //Ends


        //GET: /Hotel/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsHotel CsHotel = new CsHotel();
            DropDown(CompID, TokenKey);
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/details/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsHotel = await System.Text.Json.JsonSerializer.DeserializeAsync<CsHotel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_DetailOrDelete",CsHotel);
                    }
                    else
                    {
                        Tuple<CsHotel, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsHotel, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (data.Item2 != null && data.Item2.Message == "GlobalItem")
                        {
                            ViewBag.Message = "Sytem Entry, Can't be Changed !";
                            return View("_DetailOrDelete",data.Item1);
                        }
                        if (data.Item1 == null && data.Item2 == null) { ViewData["ErrorMessage"] = "Entry Could not be Found!"; return View("_ErrorGeneric"); }
                    }
                }
            }
            return View("Error");
        }


        //GET: /Hotel/CreateOrEdit
        [HttpGet]
        [ResponseCache(Duration = 0)]
        public async Task<IActionResult> CreateOrEdit(string Id)
        {
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ResponseName"] = "ShowValidation";
            DropDown(CompID, TokenKey);
            if (Id != null)
            {
                bool Success = false;
                var data = new CsHotel();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("Hotel/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsHotel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (response.IsSuccessStatusCode)
                        { Success = true; }
                        else { Success = false; }
                    }
                }
                if (Success == true)
                {
                    ViewData["ResponseName"] = "SuccessPop";
                    return View("_CreateOrEdit", data);
                }
                else { return View("Errors"); }
            }
            else
            {
                return View("_CreateOrEdit");
            }
        }
        
        //GET: /Hotel/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            DropDown(CompID, TokenKey);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
            Task<Tuple<int, List<CsHotel>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
            PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
            ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
            return View();
        } 
        

        //POST: /Hotel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CsHotel CsHotel)
        {       
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            DropDown(CompID, TokenKey);
            CsHotel.CompId = CompID;
            CsHotel.CreatedBy = UserID;
            CsHotel.ModifiedBy = UserID;
            CsHotel.Id = Guid.NewGuid().ToString();
            ViewData["ResponseName"] = "ShowValidation";
            var files = HttpContext.Request.Form.Files;
            CsHotel data = new CsHotel();
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    foreach (var file in files)
                    {
                        var fileName = file.FileName;

                        CsHotel.CoverImage = fileName;


                        if (file.Length > 0)
                        {
                            Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "HotelBanner");
                            Task.WaitAll(TImgUpload);
                            if (TImgUpload.Result == "Error")
                            {
                                return View("Error");
                            }
                        }
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(CsHotel), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Hotel/create", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsHotel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                {
                    data.Id = Base64UrlEncoder.Encode(Process.Encrypt(data.Id));
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", data.Id, data.Name });
                }
                else { return View("_CreateOrEdit", CsHotel); }
            }
            return View("_CreateorEdit",CsHotel);                
         }


        //GET: /Hotel/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            Title();
            ViewData["AnName"] = "Edit";
            ViewData["Id"] = id;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            DropDown(CompID, TokenKey);
            if (id == null)
            {
                ViewBag.Message = "Not Founded";
                return View();
            }

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            CsHotel CsHotel = new CsHotel();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsHotel>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;                   
                    if (response.IsSuccessStatusCode)
                    {
                        CsHotel = await System.Text.Json.JsonSerializer.DeserializeAsync<CsHotel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (responsemsg.Message == "NotFound") 
                        { ViewBag.Message = "Entry Not Exits!"; }
                        if (responsemsg.Message == "GlobalItem")
                        { ViewBag.Message = "Sytem Entry, Can't Change !"; }
                    }
                }
            }
            return View(CsHotel);
        }

                
        //POST: /Hotel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CsHotel CsHotel)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            DropDown(CompID, TokenKey);
            ViewData["ResponseName"] = "ShowValidation";
            CsHotel.CompId = CompID;
            CsHotel.ModifiedBy = UserID;
            var files = HttpContext.Request.Form.Files;
            if (CsHotel.CoverImageName != null)
            {
                CsHotel.CoverImage = CsHotel.CoverImageName;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = APIColorStays.Initial())
                    {
                        foreach (var file in files)
                        {
                            var fileName = file.FileName;

                            CsHotel.CoverImage = fileName;
                            //Delete the Images from the folder
                            Task<string> TDeleteImage = ryCsImage.DeleteImage(CsHotel.CoverImageName, TokenKey, "HotelBanner");
                            Task.WaitAll(TDeleteImage);

                            if (file.Length > 0)
                            {
                                Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "HotelBanner");
                                Task.WaitAll(TImgUpload);
                                if (TImgUpload.Result == "Error")
                                {
                                    return View("Error");
                                }
                            }
                        }


                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsHotel>("Hotel/edit", CsHotel))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsHotel, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsHotel,Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                                if (data.Item2 != null)
                                {
                                    if (data.Item2.Message == "Duplicate")
                                    {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                        ModelState.AddModelError("Name", "Duplicate Value, Already Exists !");
                                    }
                                    if (data.Item2.Message == "GlobalItem")
                                    {
                                        ViewBag.Message = "Sytem Entry, Can't Change !";
                                    }
                                    if (data.Item2.Message == "Verified")
                                    {
                                        ViewBag.Message = "You can not Edit Verified Entry !";
                                    }
                                }
                                return View("_CreateorEdit", data.Item1);
                            }
                        }
                    }
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", CsHotel.Id, CsHotel.Name });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreateorEdit");
                }
            }
            return View("_CreateorEdit",CsHotel);
        }
        
       
        //POST: /Hotel/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));

            List<CsHotelImageVideo> photosList = new List<CsHotelImageVideo>();
            CsRoomType photo = new CsRoomType();
            CsHotel csHotel = new CsHotel();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("RoomType/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    photo = await System.Text.Json.JsonSerializer.DeserializeAsync<CsRoomType>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    csHotel = await System.Text.Json.JsonSerializer.DeserializeAsync<CsHotel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }


            var roomid = "null";
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("HotelImageVideo/Index/" + id + "/" + roomid))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    photosList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsHotelImageVideo>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }

            //Delete the Images from the folder
            List<string> image = new List<string>();
            foreach (var item in photosList)
            {
                string img;
                img = item.Name;
                image.Add(img);
            }
            if (image.Count > 0)
            {
                Task<string> TDeleteImage = ryCsImage.DeleteMultiImage(image, "Hotel", TokenKey);
                Task.WaitAll(TDeleteImage);
                if (TDeleteImage.Result == "Error")
                {
                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                }
            }
            Task<string> TDeleteImage1 = ryCsImage.DeleteImage(csHotel.CoverImageName, TokenKey, "HotelBanner");
            if (photo != null)
            {
                Task<string> TDeleteImage2 = ryCsImage.DeleteImage(photo.CoverImageName, TokenKey, "Hotel");
                Task.WaitAll(TDeleteImage2);
                if (TDeleteImage2.Result == "Error")
                {
                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                }
            }
            Task.WaitAll(TDeleteImage1);

            if (TDeleteImage1.Result == "Error")
            {
                ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
            }

            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { PageCall="Show"});
                    }
                    else
                    {
                        Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (responsemsg.Message == "DeleteStop") { ViewData["ErrorMessage"] = "Sytem Entry, Can't be Del !"; return View("_ErrorGeneric"); }
                        else { return View("Error"); }
                    }
                }
            }
        }

        //POST: >Verify/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyData(string Id, string AnName)          
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			VerNActViewModel model = new VerNActViewModel();
            model.Id = Id;
            model.ActionName = AnName;
            model.CompId = CompID;
            if (model.ActionName == "Verify" || model.ActionName == "UnVerify") { model.VerifiedBy = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); }
            if (model.ActionName == "Activate" || model.ActionName == "Inactivate") { model.ActivatedBy = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); }
			CsHotel CsHotel = new CsHotel();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Hotel/verifydata/" , content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (responsemsg.Message == "GlobalItem")
                        {
                            ViewData["ErrorMessage"] = "Sytem Entry, Can't be Changed !"; return View("_ErrorGeneric");
                        }
                    }
                }
            }
            return RedirectToAction("Index", new { PageCall = "Show" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkStar(Dictionary<string, string> Id, int PageNo, int PageSize)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            StarUnstar starUnstar = new StarUnstar();
            starUnstar.Id = Id;
            starUnstar.Host = Request.Scheme + "://" + Request.Host;
            starUnstar.AreaName = "ColorStays";
            starUnstar.ControllerName = "Hotel";
            starUnstar.CreatedBy = UserId;
            using (HttpClient client = APIComp.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(starUnstar), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("UserClip/markstar/", content))
                {
                    if (response.IsSuccessStatusCode)
                    { Success = true; }
                }
            }
            if (Success == true) { return RedirectToAction("Index", new { PgSelected = PageNo, PgSize = PageSize, PageCall = "Show" }); }
            else { return View("Error"); }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UnMarkStar(List<string> Id, int PageNo, int PageSize)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            StarUnstar unstar = new StarUnstar();
            unstar.UnStarId = Id;
            unstar.CreatedBy = UserId;
            using (HttpClient client = APIComp.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(unstar), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("UserClip/unmarkstar/", content))
                {
                    if (response.IsSuccessStatusCode)
                    { Success = true; }
                }
            }
            if (Success == true) { return RedirectToAction("Index", new { PgSelected = PageNo, PgSize = PageSize, PageCall = "Show" }); }
            else { return View("Error"); }
        }

         //This method is to check duplicate values for specific columns......
        public async Task<JsonResult> CheckDuplicationHotel(string Name, string NameAction, string Id, string Fk_Place_Name)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/CheckDuplicationHotel/" + Name + "/" + NameAction + "/" + Fk_Place_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + Name + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }


        //This method is to check duplicate values for Rank columns......
        public async Task<JsonResult> CheckDuplicationHotelRank(string Name, string NameAction, string Id, string Fk_City_Name)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Hotel/CheckDuplicationHotelRank/" + Name + "/" + NameAction + "/" + Fk_City_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + Name + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }



        //GET: /Offer/Create
        [HttpGet]
        public async Task<IActionResult> HotelFacilityList(string HlId)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            HotelFacilityCheckbox facilityList = new HotelFacilityCheckbox();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("HotelFacilityMap/HotelFacilityList/" + HlId + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    facilityList = await System.Text.Json.JsonSerializer.DeserializeAsync<HotelFacilityCheckbox>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            return View("_CreateOrEditHotelFacility", facilityList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHotelFacility(HotelFacilityCheckbox model)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            model.CompId = CompID;

            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("HotelFacilityMap/AddHotelFacility", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            return RedirectToAction("GetHotelFacility", new { id = model.Fk_Hotel_Name });
                        }
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> GetHotelFacility(string HlId)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            List<CsHotelFacilityMap> data = new List<CsHotelFacilityMap>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("HotelFacilityMap/GetHotelFacility/" + HlId + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsHotelFacilityMap>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        ViewBag.HotelFacility = data;
                        return View();
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> DeleteFacilityOfHotel(string HlId ,string Id)
        {
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("HotelFacilityMap/DeleteConfirmed/" + Id +"/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetHotelFacility", new { id = HlId });
                    }
                }
            }
            return View("Error");
        }


        //GET: /Offer/Create
        [HttpGet]
        public async Task<IActionResult> HotelPlan(string HlId)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsPlanType planType = new CsPlanType();
            planType.Fk_Hotel_Name = HlId;
            return View("_CreateOrEditHotelPlan", planType);
        }

        //GET: /Offer/Create
        [HttpGet]
        public async Task<IActionResult> EditHotelPlan(string id)
        {
            Title();
            ViewData["AnName"] = "Edit";
            ViewData["Id"] = id;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                ViewBag.Message = "Not Founded";
                return View();
            }

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            CsPlanType CsPlanType = new CsPlanType();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PlanType/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                   
                    if (response.IsSuccessStatusCode)
                    {
                        CsPlanType = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPlanType>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (responsemsg.Message == "NotFound")
                        { ViewBag.Message = "Entry Not Exits!"; }
                        if (responsemsg.Message == "GlobalItem")
                        { ViewBag.Message = "Sytem Entry, Can't Change !"; }
                    }
                }
            }
            return View("_CreateOrEditHotelPlan", CsPlanType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHotelPlan(CsPlanType CsPlanType)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
			var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool Success = false;
			CsPlanType.CompId = CompID;
			CsPlanType.CreatedBy = UserID;
			CsPlanType.ModifiedBy = UserID;
			
			ViewData["ResponseName"] = "ShowValidation";
			CsPlanType data = new CsPlanType();

			if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    if (CsPlanType.Id == null)
                    {
                        CsPlanType.Id = Guid.NewGuid().ToString();
                        StringContent content = new StringContent(JsonSerializer.Serialize(CsPlanType), Encoding.UTF8, "application/json");

                        using (var response = await client.PostAsync("PlanType/create", content))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPlanType>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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

                    else
                    {
                        StringContent content = new StringContent(JsonSerializer.Serialize(CsPlanType), Encoding.UTF8, "application/json");

                        using (var response = await client.PostAsync("PlanType/edit", content))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPlanType>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                }
                if (Success == true)
                {
                    return RedirectToAction("GetHotelPlan", new { id = CsPlanType.Fk_Hotel_Name });
                }
                else { return View("_CreateOrEditHotelPlan", CsPlanType); }
            }
            return View("_CreateOrEditHotelPlan", CsPlanType);
        }


        [HttpGet]
        public async Task<IActionResult> GetHotelPlan(string HlId)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            List<CsPlanType> data = new List<CsPlanType>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PlanType/index/" + CompID + "/" + HlId, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPlanType>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View(data);
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> DeletePlanOfHotel(string HlId, string Id)
        {
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PlanType/DeleteConfirmed/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetHotelPlan", new { id = HlId });
                    }
                }
            }
            return View("Error");
        }

    }


}