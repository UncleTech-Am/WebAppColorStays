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
using WebAppColorStays.CommonMethod;
using LibCommon.APICommonMethods;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly Paging paging;
        private readonly RyCSImage ryCsImage;

        public ReviewController()
        {
            paging = new Paging();
            ryCsImage = new RyCSImage();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "Review";
        }
        //Ends


        [HttpPost]
        public async Task<IActionResult> SaveImage(string LId)
        {
            var TokenKey = Request.Cookies["JWToken"];

            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = HttpContext.Request.Form.Files;

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                foreach (var file in files)
                {
                    var fileName = "Review-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5) + Path.GetExtension(file.FileName);
                    using (var response = await client.PostAsync("Review/SaveImage/?LId=" + LId + "&CompId=" + CompID + "&UserId=" + UserID + "&FileName=" + fileName, null))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            return View("Error");
                        }
                    }

                    if (file.Length > 0)
                    {
                        Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Review");
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
        public async Task<Tuple<int, List<CsReview>>> AllDataList(int? PgSize, int? PgSelectedNum, string condition)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tuple<int, List<CsReview>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/index/" + CompID + "/" + PgSize + "/" + PgSelectedNum + "/" + UserID + "/" + condition, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsReview>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else{ list = null; }
                }
            }
            return list;
        }
        //Ends


        //GET:/Review/
        [HttpGet]
        public async Task<IActionResult> Index(int? PgSelectedNum, int? PgSize, string PageCall, string condition, string Label)
        {         
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            //Display the Dropdown of the Table fields in Search Data Popup
            GetClassMember<CsReview> getClassMember = new GetClassMember<CsReview>();
            CsReview CsReview = new CsReview();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsReview), "Value", "DisplayName");
            //Ends

            try //Pagination and List of data Code
            {
                Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No
                Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, condition);//Give the List of data
                PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination

                ViewData["ActionName"] = "Index";
                ViewData["FormID"] = "NoSearchID";
                ViewData["SearchType"] = "NoSearch";
                ViewData["Action"] = "Review" + condition;
                ViewData["Type"] = condition;
                ViewData["LId"] = Label;

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
                Tuple<int, List<CsReview>> list;
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    //Get the List of data
                    using (var response = await client.PostAsJsonAsync("Review/DateSearch/", cIndex))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsReview>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        public async Task<IActionResult> TableSearch(CsReview CsReview, IFormCollection fc)
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

            Tuple<int, List<CsReview>> list;

            using (HttpClient client = APIColorStays.Initial())
            {
                CsReview.CreatedBy = UserID;
                CsReview.CompId = CompID;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(CsReview), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Review/TableSearch/?PageSelectedNum=" + pagedata.Item2 + "&PageSize=" + pagedata.Item1, content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsReview>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            GetClassMember<CsReview> getClassMember = new GetClassMember<CsReview>();
            CsReview CsReview = new CsReview();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsReview), "Value", "DisplayName");
            //Creating Search Filter List with class member Property Name
            Dictionary<string, string> fields = getClassMember.GetPropertyName(CsReview);
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

            Tuple<int, List<CsReview>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
               indexsearchfilter.CurrentUserId = UserID;
                indexsearchfilter.CompId = CompID;
                indexsearchfilter.PageSelectedNum = pagedata.Item2;
                indexsearchfilter.PageSize = pagedata.Item1;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(indexsearchfilter), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Review/FilterSearch", content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsReview>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Review/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id, string condition)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsReview CsReview = new CsReview();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/details/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsReview = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReview>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        ViewData["Type"] = condition;
                        return View("_DetailOrDelete",CsReview);
                    }
                    else
                    {
                        Tuple<CsReview, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsReview, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Review/CreateOrEdit
        [HttpGet]
        [ResponseCache(Duration = 0)]
        public async Task<IActionResult> CreateOrEdit(string Id)
        {
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ResponseName"] = "ShowValidation";
            if (Id != null)
            {
                bool Success = false;
                var data = new CsReview();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("Review/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReview>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        
        

        //GET: /Festival/Create
        [HttpGet]
        public async Task<IActionResult> CreateCountryWise()
        {
            Title();
            ViewData["AnName"] = "Create";
            ViewData["Fields"] = "Country";

            var TokenKey = Request.Cookies["JWToken"];
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsReviewPost csReviewPost = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/Create/"+ CompId+"/"+"Country", HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, "Country");//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        csReviewPost = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            ViewData["Type"] = "Country";
            csReviewPost.Condition = "Country";
            ViewData["Action"] = "ReviewCountry";
            return View("Create", csReviewPost);
        }

        [HttpGet]
        public async Task<IActionResult> CreateStateWise()
        {
            Title();
            ViewData["AnName"] = "Create";
            ViewData["Fields"] = "State";

            var TokenKey = Request.Cookies["JWToken"];
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsReviewPost csReviewPost = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/Create/" + CompId + "/" + "State", HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, "State");//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        csReviewPost = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            ViewData["Type"] = "State";
            csReviewPost.Condition = "State";
            ViewData["Action"] = "ReviewState";
            return View("Create", csReviewPost);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCityWise()
        {
            Title();
            ViewData["AnName"] = "Create";
            ViewData["Fields"] = "City";

            var TokenKey = Request.Cookies["JWToken"];
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsReviewPost csReviewPost = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/Create/" + CompId + "/" + "City", HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, "City");//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        csReviewPost = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            ViewData["Type"] = "City";
            csReviewPost.Condition = "City";
            ViewData["Action"] = "ReviewCity";
            return View("Create", csReviewPost);
        }
        [HttpGet]
        public async Task<IActionResult> CreatePlaceWise()
        {
            Title();
            ViewData["AnName"] = "Create";
            ViewData["Fields"] = "Place";

            var TokenKey = Request.Cookies["JWToken"];
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsReviewPost csReviewPost = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/Create/" + CompId + "/" + "Place", HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, "Place");//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        csReviewPost = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            ViewData["Type"] = "Place";
            csReviewPost.Condition = "Place";
            ViewData["Action"] = "ReviewPlace";
            return View("Create", csReviewPost);
        }

        //GET: /Festival/Create
        [HttpGet]
        public async Task<IActionResult> CreateHotelWise()
        {
            Title();
            ViewData["AnName"] = "Create";
            ViewData["Fields"] = "Hotel";

            var TokenKey = Request.Cookies["JWToken"];
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsReviewPost csReviewPost = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/Create/" + CompId + "/" + "Hotel", HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, "Hotel");//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        csReviewPost = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            ViewData["Type"] = "Hotel";
            csReviewPost.Condition = "Hotel";
            ViewData["Action"] = "ReviewHotel";
            return View("Create", csReviewPost);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePackageWise()
        {
            Title();
            ViewData["AnName"] = "Create";
            ViewData["Fields"] = "Package";

            var TokenKey = Request.Cookies["JWToken"];
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsReviewPost csReviewPost = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/Create/" + CompId + "/" + "Package", HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, "Package");//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        csReviewPost = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            ViewData["Type"] = "Package";
            csReviewPost.Condition = "Package";
            ViewData["Action"] = "ReviewPackage";
            return View("Create", csReviewPost);
        }




        //POST: /Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CsReviewPost csReviewPost)
        {       
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            csReviewPost.CompId = CompID;
            csReviewPost.CreatedBy = UserID;
            csReviewPost.ModifiedBy = UserID;
            ViewData["ResponseName"] = "ShowValidation";
            CsReview data = new CsReview();
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
				    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(csReviewPost), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Review/create", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReview>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", condition = csReviewPost.Condition, Label = data.Label }); 
                }
                else { return View("_CreateOrEdit", csReviewPost); }
            }
            return View("_CreateorEdit", csReviewPost);                
         }


        //GET: /Review/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(string id, string condition)
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

            CsReviewPost CsReview = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsReview>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2, condition);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;                   
                    if (response.IsSuccessStatusCode)
                    {
                        CsReview = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            ViewData["Type"] = condition;
            ViewData["Fields"] = condition;
            ViewData["Action"] = "Review"+condition;
            CsReview.Condition = condition;

            return View(CsReview);
        }

                
        //POST: /Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CsReviewPost CsReview)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            CsReview.CompId = CompID;
            CsReview.ModifiedBy = UserID;
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = APIColorStays.Initial())
                    {
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsReviewPost>("Review/edit", CsReview))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsReviewPost, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsReviewPost,Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", condition = CsReview.Condition, Label = CsReview.Label });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreateorEdit");
                }
            }
            return View("_CreateorEdit",CsReview);
        }
        
       
        //POST: /Review/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id , string condition)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsReviewPost CsReview = new CsReviewPost();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsReview = await System.Text.Json.JsonSerializer.DeserializeAsync<CsReviewPost>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            List<CsReviewPhotoVideo> photolist = new List<CsReviewPhotoVideo>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("ReviewPhotoVideo/index/" + CsReview.Label + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    photolist = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsReviewPhotoVideo>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            //Delete the Images from the folder
            List<string> image = new List<string>();
            foreach (var item in photolist)
            {
                string img;
                img = item.Name;
                image.Add(img);
            }
            if (image.Count > 0)
            {
                Task<string> TDeleteImage = ryCsImage.DeleteMultiImage(image, "Review", TokenKey);
                Task.WaitAll(TDeleteImage);

                if (TDeleteImage.Result == "Error")
                {
                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                }
            }

            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { PageCall = "Show", condition = condition });
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
			CsReview CsReview = new CsReview();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Review/verifydata/" , content))
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
            starUnstar.ControllerName = "Review";
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
        public async Task<JsonResult> CheckDuplicationReview(string Name, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Review/CheckDuplicationReview/" + Name + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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
    }
}