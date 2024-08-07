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
using LibCommon.APICommonMethods;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class AirportController : Controller
    {
        private readonly Paging paging;
        private readonly RyCSImage ryCsImage;

        public AirportController()
        {
            paging = new Paging();
            ryCsImage = new RyCSImage();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "Airport";
        }
        //Ends

        //GET: /Airport/Add image in Airport
        [HttpGet]
        public async Task<ActionResult> AddImage(string Id)
        {
            Title();
            ViewData["AnName"] = "Edit";
            ViewData["Id"] = Id;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Id == null)
            {
                ViewBag.Message = "Not Founded";
                return View();
            }

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            CsAirport CsAirport = new CsAirport();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsAirport>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        CsAirport = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return PartialView("_AddImage", CsAirport);
        }


        [HttpPost]
        public async Task<IActionResult> SaveImage(CsAirport CsAirport)
        {
            var TokenKey = Request.Cookies["JWToken"];

            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = HttpContext.Request.Form.Files;
            //var uploads = Path.Combine(Environment.WebRootPath, "Image\\Airport");
            CsAirport.CompId = CompID;
            CsAirport.ModifiedBy = UserID;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                foreach (var file in files)
                {
                    var fileName = "Airport-"+ file.FileName;
                    if ("Photo1" == file.Name)
                    {
                        CsAirport.Photo1 = fileName;
                    }
                    if ("Photo2" == file.Name)
                    {
                        CsAirport.Photo2 = fileName;
                    }
                    if ("Photo3" == file.Name)
                    {
                        CsAirport.Photo3 = fileName;
                    }
                    if ("Photo4" == file.Name)
                    {
                        CsAirport.Photo4 = fileName;
                    }
                    if ("Photo5" == file.Name)
                    {
                        CsAirport.Photo5 = fileName;
                    }

                    if (file.Length > 0)
                    {
                        Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Airport");
                        Task.WaitAll(TImgUpload);
                        if (TImgUpload.Result == "Error")
                        {
                            return View("Error");
                        }
                    }
                }
                using (var response = await client.PostAsJsonAsync<CsAirport>("Airport/edit", CsAirport))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }
                }
            }
            return Json(new { Message = "Saved" });
        }


        //Edit Image Detail like AltTag and Description
        //POST: /Airport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImageDetail(CsAirport CsAirport)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            CsAirport.CompId = CompID;
            CsAirport.ModifiedBy = UserID;
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = APIColorStays.Initial())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsAirport>("Airport/edit", CsAirport))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsAirport, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsAirport, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                                return View("_CreatePhoto", data.Item1);
                            }
                        }
                    }
                    return RedirectToAction("UploadedImage", new { Id = CsAirport.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreatePhoto");
                }
            }
            return View("_CreatePhoto", CsAirport);
        }


        //show upload the image
        [HttpGet]
        public async Task<IActionResult> UploadedImage(string Id, string? UpdateDetail, string? ShowBtn)
        {

            var TokenKey = Request.Cookies["JWToken"];
            CsAirport CsActivityImages = new CsAirport();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/UploadedImage/" + Id, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    CsActivityImages = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            if (ShowBtn == "false")
            {
                ViewBag.ShowBtn = "false";
            }
            if (UpdateDetail == "Yes") { return PartialView("_AddImageDetail", CsActivityImages); }
            return PartialView("UploadedImage", CsActivityImages);
        }
        //ends

        //Delete the upload image
        public async Task<IActionResult> ImageDelete(string Id, string Field, string ImgName)
        {
            var TokenKey = Request.Cookies["JWToken"];

            CsAirport CsActivityImages = new CsAirport();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/ImageDelete/" + Id +"/" + Field, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }
                    //Delete the Images from the folder
                    Task<string> TDeleteImage = ryCsImage.DeleteImage(ImgName, TokenKey, "Airport");
                    Task.WaitAll(TDeleteImage);
                }


            }
            return RedirectToAction("UploadedImage", new { Id });
        }
        //ends


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
        public async Task<Tuple<int, List<CsAirport>>> AllDataList(int? PgSize, int? PgSelectedNum)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tuple<int, List<CsAirport>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/index/" + CompID + "/" + PgSize + "/" + PgSelectedNum + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsAirport>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else{ list = null; }
                }
            }
            return list;
        }
        //Ends


        //GET:/Airport/
        [HttpGet]
        public async Task<IActionResult> Index(int? PgSelectedNum, int? PgSize, string PageCall, string? Id)
        {         
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            //Display the Dropdown of the Table fields in Search Data Popup
            GetClassMember<CsAirport> getClassMember = new GetClassMember<CsAirport>();
            CsAirport CsAirport = new CsAirport();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsAirport), "Value", "DisplayName");
            //Ends

            try //Pagination and List of data Code
            {
                Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No
                Task<Tuple<int, List<CsAirport>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination

                ViewData["ActionName"] = "Index";
                ViewData["FormID"] = "NoSearchID";
                ViewData["SearchType"] = "NoSearch";
                ViewData["ATId"] = Id;

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
                Tuple<int, List<CsAirport>> list;
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    //Get the List of data
                    using (var response = await client.PostAsJsonAsync("Airport/DateSearch/", cIndex))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsAirport>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        public async Task<IActionResult> TableSearch(CsAirport CsAirport, IFormCollection fc)
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

            Tuple<int, List<CsAirport>> list;

            using (HttpClient client = APIColorStays.Initial())
            {
                CsAirport.CreatedBy = UserID;
                CsAirport.CompId = CompID;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(CsAirport), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Airport/TableSearch/?PageSelectedNum=" + pagedata.Item2 + "&PageSize=" + pagedata.Item1, content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsAirport>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            GetClassMember<CsAirport> getClassMember = new GetClassMember<CsAirport>();
            CsAirport CsAirport = new CsAirport();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsAirport), "Value", "DisplayName");
            //Creating Search Filter List with class member Property Name
            Dictionary<string, string> fields = getClassMember.GetPropertyName(CsAirport);
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

            Tuple<int, List<CsAirport>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
               indexsearchfilter.CurrentUserId = UserID;
                indexsearchfilter.CompId = CompID;
                indexsearchfilter.PageSelectedNum = pagedata.Item2;
                indexsearchfilter.PageSize = pagedata.Item1;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(indexsearchfilter), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Airport/FilterSearch", content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsAirport>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Airport/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsAirport CsAirport = new CsAirport();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/details/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsAirport = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_DetailOrDelete",CsAirport);
                    }
                    else
                    {
                        Tuple<CsAirport, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsAirport, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Airport/CreateOrEdit
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
                var data = new CsAirport();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("Airport/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        
        //GET: /Airport/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
            Task<Tuple<int, List<CsAirport>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
            PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
            ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
            return View();
        } 
        

        //POST: /Airport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CsAirport CsAirport)
        {       
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            CsAirport.CompId = CompID;
            CsAirport.CreatedBy = UserID;
            CsAirport.ModifiedBy = UserID;
            CsAirport.Id = Guid.NewGuid().ToString();
            ViewData["ResponseName"] = "ShowValidation";
            CsAirport data = new CsAirport();
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
				    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(CsAirport), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Airport/create", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", data.Id });
                }
                else { return View("_CreateOrEdit", CsAirport); }
            }
            return View("_CreateorEdit",CsAirport);                
         }


        //GET: /Airport/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
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

            CsAirport CsAirport = new CsAirport();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsAirport>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;                   
                    if (response.IsSuccessStatusCode)
                    {
                        CsAirport = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return View(CsAirport);
        }

                
        //POST: /Airport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CsAirport CsAirport)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            CsAirport.CompId = CompID;
            CsAirport.ModifiedBy = UserID;   
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = APIColorStays.Initial())
                    {
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsAirport>("Airport/edit", CsAirport))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsAirport, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsAirport,Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", CsAirport.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreateorEdit");
                }
            }
            return View("_CreateorEdit",CsAirport);
        }
        
       
        //POST: /Airport/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsAirport CsAirportImages = new CsAirport();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/UploadedImage/" + id, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    CsAirportImages = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAirport>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }

            Task<string> TDeleteImage1= null;
            Task<string> TDeleteImage2 = null;
            Task<string> TDeleteImage3 = null;
            Task<string> TDeleteImage4 = null;
            Task<string> TDeleteImage5 = null;
            if (CsAirportImages.Photo1 != null)
            {
                TDeleteImage1 = ryCsImage.DeleteImage(CsAirportImages.Photo1, TokenKey, "Airport");
            }
            if (CsAirportImages.Photo2 != null)
            {
                TDeleteImage2 = ryCsImage.DeleteImage(CsAirportImages.Photo2, TokenKey, "Airport");
            }
            if (CsAirportImages.Photo3 != null)
            {
                TDeleteImage3 = ryCsImage.DeleteImage(CsAirportImages.Photo3, TokenKey, "Airport");
            }
            if (CsAirportImages.Photo4 != null)
            {
                TDeleteImage4 = ryCsImage.DeleteImage(CsAirportImages.Photo4, TokenKey, "Airport");
            }
            if (CsAirportImages.Photo5 != null)
            {
                TDeleteImage5 = ryCsImage.DeleteImage(CsAirportImages.Photo5, TokenKey, "Airport");
            }
            //Delete the Images from the folder
            Task.WaitAll(TDeleteImage1, TDeleteImage2, TDeleteImage3, TDeleteImage4, TDeleteImage5);

            if (TDeleteImage1.Result == "Error" || TDeleteImage2.Result == "Error" || TDeleteImage3.Result == "Error" || TDeleteImage4.Result == "Error"|| TDeleteImage5.Result == "Error")
            {
                ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
            }


            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { PageCall= "Show" });
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
			CsAirport CsAirport = new CsAirport();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Airport/verifydata/" , content))
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
            starUnstar.ControllerName = "Airport";
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
        public async Task<JsonResult> CheckDuplicationAirport(string Name, string NameAction, string Fk_Place_Name, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            if (Fk_Place_Name == null)
            {
                Fk_Place_Name = "No";
            }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Airport/CheckDuplicationAirport/" + Name + "/" + NameAction + "/" + Fk_Place_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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
    }
}