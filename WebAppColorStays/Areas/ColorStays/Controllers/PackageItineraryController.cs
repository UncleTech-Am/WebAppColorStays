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
using System.Collections.Generic;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class PackageItineraryController : Controller
    {
        private readonly Paging paging;
        private IWebHostEnvironment Environment;
        private readonly RyCSImage ryCsImage;
        public PackageItineraryController(IWebHostEnvironment Environment)
        {
            paging = new Paging();
            Environment = Environment;
            ryCsImage = new RyCSImage();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "Package Itinerary";
        }
        //Ends

       
        //GET: /PackageItinerary/Add image in Itinerary
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

            CsPackageItinerary CsPackageItinerary = new CsPackageItinerary();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsPackageItinerary>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
                    if (response.IsSuccessStatusCode)
                    {
                        CsPackageItinerary = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return PartialView("_AddImage",CsPackageItinerary);
        }


        [HttpPost]
        public async Task<IActionResult> SaveImage(CsPackageItinerary csPackage)
        {
            var TokenKey = Request.Cookies["JWToken"];

            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = HttpContext.Request.Form.Files;
            //var uploads = Path.Combine(Environment.WebRootPath, "Image\\PackageItinerary");
            csPackage.CompId = CompID;
            csPackage.ModifiedBy = UserID;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                foreach (var file in files)
                {

                    var fileName = "Itinerary-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5) + Path.GetExtension(file.FileName);
                    if ("Photo1" == file.Name)
                    {
                        csPackage.Photo1 = fileName;
                    }
                    if ("Photo2" == file.Name)
                    {
                        csPackage.Photo2 = fileName;
                    }
                    if ("Photo3" == file.Name)
                    {
                        csPackage.Photo3 = fileName;
                    }
                    if ("Photo4" == file.Name)
                    {
                        csPackage.Photo4 = fileName;
                    }
                    if ("Photo5" == file.Name)
                    {
                        csPackage.Photo5 = fileName;
                    }

                    if (file.Length > 0)
                    {
                        Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Itinerary");
                        Task.WaitAll(TImgUpload);
                        if (TImgUpload.Result == "Error")
                        {
                            return View("Error");
                        }
                    }
                }
                using (var response = await client.PostAsJsonAsync<CsPackageItinerary>("PackageItinerary/edit", csPackage))
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
        //POST: /PackageItinerary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImageDetail(CsPackageItinerary CsPackageItinerary)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            CsPackageItinerary.CompId = CompID;
            CsPackageItinerary.ModifiedBy = UserID;
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = APIColorStays.Initial())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsPackageItinerary>("PackageItinerary/edit", CsPackageItinerary))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsPackageItinerary, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPackageItinerary, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    return RedirectToAction("UploadedImage", new { Id = CsPackageItinerary.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreatePhoto");
                }
            }
            return View("_CreatePhoto", CsPackageItinerary);
        }


        //show upload the image
        [HttpGet]
        public async Task<IActionResult> UploadedImage(string Id, string? UpdateDetail, string? ShowBtn)
        {

            var TokenKey = Request.Cookies["JWToken"];
            CsPackageItinerary CsActivityImages = new CsPackageItinerary();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/UploadedImage/" + Id, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    CsActivityImages = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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

            CsPackageItinerary CsActivityImages = new CsPackageItinerary();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/ImageDelete/" + Id + "/" + Field, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }
                    //Delete the Images from the folder
                    Task<string> TDeleteImage = ryCsImage.DeleteImage(ImgName, TokenKey, "Itinerary");
                    Task.WaitAll(TDeleteImage);
                }


            }
            return RedirectToAction("Edit", new { Id });
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
        public async Task<Tuple<int, List<CsPackageItinerary>>> AllDataList(int? PgSize, int? PgSelectedNum)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tuple<int, List<CsPackageItinerary>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/index/" + CompID + "/" + PgSize + "/" + PgSelectedNum + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackageItinerary>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else{ list = null; }
                }
            }
            return list;
        }
        //Ends


        //GET:/PackageItinerary/
        [HttpGet]
        public async Task<IActionResult> Index(string PeId)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            //Display the Dropdown of the Table fields in Search Data Popup
            GetClassMember<CsPackageItinerary> getClassMember = new GetClassMember<CsPackageItinerary>();
            CsPackageItinerary CsPackageItinerary = new CsPackageItinerary();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsPackageItinerary), "Value", "DisplayName");
            //Ends
            Tuple<int, List<CsPackageItinerary>> list;

            try //Pagination and List of data Code
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("PackageItinerary/index/" + CompID + "/" + PeId, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackageItinerary>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        }
                        else { list = null; }
                    }
                }

                ViewData["ActionName"] = "Index";
                ViewData["FormID"] = "NoSearchID";
                ViewData["SearchType"] = "NoSearch";

                return View("_IndexSearch", list.Item2);

            }
            catch (Exception ex)
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
                Tuple<int, List<CsPackageItinerary>> list;
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    //Get the List of data
                    using (var response = await client.PostAsJsonAsync("PackageItinerary/DateSearch/", cIndex))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackageItinerary>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        public async Task<IActionResult> TableSearch(CsPackageItinerary CsPackageItinerary, IFormCollection fc)
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

            Tuple<int, List<CsPackageItinerary>> list;

            using (HttpClient client = APIColorStays.Initial())
            {
                CsPackageItinerary.CreatedBy = UserID;
                CsPackageItinerary.CompId = CompID;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(CsPackageItinerary), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("PackageItinerary/TableSearch/?PageSelectedNum=" + pagedata.Item2 + "&PageSize=" + pagedata.Item1, content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackageItinerary>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            GetClassMember<CsPackageItinerary> getClassMember = new GetClassMember<CsPackageItinerary>();
            CsPackageItinerary CsPackageItinerary = new CsPackageItinerary();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsPackageItinerary), "Value", "DisplayName");
            //Creating Search Filter List with class member Property Name
            Dictionary<string, string> fields = getClassMember.GetPropertyName(CsPackageItinerary);
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

            Tuple<int, List<CsPackageItinerary>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
               indexsearchfilter.CurrentUserId = UserID;
                indexsearchfilter.CompId = CompID;
                indexsearchfilter.PageSelectedNum = pagedata.Item2;
                indexsearchfilter.PageSize = pagedata.Item1;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(indexsearchfilter), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("PackageItinerary/FilterSearch", content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackageItinerary>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /PackageItinerary/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsPackageItinerary CsPackageItinerary = new CsPackageItinerary();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/details/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsPackageItinerary = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_DetailOrDelete",CsPackageItinerary);
                    }
                    else
                    {
                        Tuple<CsPackageItinerary, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPackageItinerary, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /PackageItinerary/CreateOrEdit
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
                var data = new CsPackageItinerary();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("PackageItinerary/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        data.Remarks = data.DayActivity;
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
        
        //GET: /PackageItinerary/Create
        [HttpGet]
        public async Task<IActionResult> Create(string PeId)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            CsPackageItinerary csPackageItinerary = new CsPackageItinerary();
            csPackageItinerary.Fk_Package_Name = PeId;
            return View(csPackageItinerary);
        }


        //POST: /PackageItinerary/Create
        //POST: /PackageItinerary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CsPackageItinerary CsPackageItinerary)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            CsPackageItinerary.CompId = CompID;
            CsPackageItinerary.CreatedBy = UserID;
            CsPackageItinerary.ModifiedBy = UserID;
            CsPackageItinerary.Id = Guid.NewGuid().ToString();
            ViewData["ResponseName"] = "ShowValidation";
            CsPackageItinerary data = new CsPackageItinerary();
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(CsPackageItinerary), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("PackageItinerary/create", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                else { return View("_CreateOrEdit", CsPackageItinerary); }
            }
            return View("_CreateorEdit", CsPackageItinerary);
        }


        //GET: /PackageItinerary/Edit/5
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

            CsPackageItinerary CsPackageItinerary = new CsPackageItinerary();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsPackageItinerary = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return View(CsPackageItinerary);
        }

                
        //POST: /PackageItinerary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CsPackageItinerary CsPackageItinerary)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            CsPackageItinerary.CompId = CompID;
            CsPackageItinerary.ModifiedBy = UserID;
            var files = HttpContext.Request.Form.Files;

            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = APIColorStays.Initial())
                    {
                        foreach (var file in files)
                        {
                            var fileName = "Itinerary-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5) + Path.GetExtension(file.FileName);

                            List<string> image = new List<string>();


                            if ("Photo1" == file.Name)
                            {
                                if (CsPackageItinerary.ImageUrl1 != null)
                                {
                                    image.Add(CsPackageItinerary.Photo1);
                                }
                                CsPackageItinerary.Photo1 = fileName;
                            }
                            if ("Photo2" == file.Name)
                            {
                                if (CsPackageItinerary.ImageUrl2 != null)
                                {
                                    image.Add(CsPackageItinerary.Photo2);
                                }
                                CsPackageItinerary.Photo2 = fileName;
                            }
                            if ("Photo3" == file.Name)
                            {
                                if (CsPackageItinerary.ImageUrl3 != null)
                                {
                                    image.Add(CsPackageItinerary.Photo3);
                                }
                                CsPackageItinerary.Photo3 = fileName;
                            }
                            if ("Photo4" == file.Name)
                            {
                                if (CsPackageItinerary.ImageUrl4 != null)
                                {
                                    image.Add(CsPackageItinerary.Photo4);
                                }
                                CsPackageItinerary.Photo4 = fileName;
                            }
                            if ("Photo5" == file.Name)
                            {
                                if (CsPackageItinerary.ImageUrl5 != null)
                                {
                                    image.Add(CsPackageItinerary.Photo5);
                                }
                                CsPackageItinerary.Photo5 = fileName;
                            }
                            if (image.Count > 0)
                            {
                                Task<string> TDeleteImage = ryCsImage.DeleteMultiImage(image, "Itinerary", TokenKey);
                                Task.WaitAll(TDeleteImage);
                                if (TDeleteImage.Result == "Error")
                                {
                                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                                }
                            }

                            if (file.Length > 0)
                            {
                                Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Itinerary");
                                Task.WaitAll(TImgUpload);
                                if (TImgUpload.Result == "Error")
                                {
                                    return View("Error");
                                }
                            }
                        }


                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsPackageItinerary>("PackageItinerary/edit", CsPackageItinerary))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsPackageItinerary, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPackageItinerary,Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    return RedirectToAction("Index", new { PeId = CsPackageItinerary.Fk_Package_Name });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreateorEdit");
                }
            }
            return View("_CreateorEdit",CsPackageItinerary);
        }
        
       
        //POST: /PackageItinerary/Delete/5
        [HttpGet]
        public async Task<IActionResult> DeleteConfirmed(string id, string PeId)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            CsPackageItinerary csPackageItinerary = new CsPackageItinerary();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    csPackageItinerary = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageItinerary>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            List<string> image = new List<string>();

            if (csPackageItinerary.Photo1 != null)
            {
                image.Add(csPackageItinerary.Photo1);
            }
            
            if (csPackageItinerary.Photo2 != null)
            {
                image.Add(csPackageItinerary.Photo2);
            }
           
            if (csPackageItinerary.Photo3 != null)
            {
                image.Add(csPackageItinerary.Photo3);
            }
            if (csPackageItinerary.Photo4 != null)
            {
                image.Add(csPackageItinerary.Photo4);
            }
            if (csPackageItinerary.Photo5 != null)
            {
                image.Add(csPackageItinerary.Photo5);
            }

            if (image.Count > 0)
            {
                Task<string> TDeleteImage = ryCsImage.DeleteMultiImage(image, "Itinerary", TokenKey);
                Task.WaitAll(TDeleteImage);
                if (TDeleteImage.Result == "Error")
                {
                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                }
            }
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { PeId= PeId });
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
			CsPackageItinerary CsPackageItinerary = new CsPackageItinerary();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("PackageItinerary/verifydata/" , content))
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
            starUnstar.ControllerName = "PackageItinerary";
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
        public async Task<JsonResult> CheckDuplicationPackageItinerary(string DayTitle, string NameAction, string Fk_Package_Name, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            if (Fk_Package_Name == null) { Fk_Package_Name = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItinerary/" + DayTitle + "/" + NameAction + "/" + Fk_Package_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + DayTitle + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }
        public async Task<JsonResult> CheckDuplicationPackageItineraryDayNo(int DayNo, string NameAction, string Fk_Package_Name, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            if (Fk_Package_Name == null) { Fk_Package_Name = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItineraryDayNo/" + DayNo + "/" + NameAction + "/" + Fk_Package_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + DayNo + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }

        public async Task<JsonResult> CheckDuplicationPackageItineraryImg1(string Photo1, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            string fileName = Path.GetFileName(Photo1);
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItineraryImg1/" + fileName + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + fileName + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }

        public async Task<JsonResult> CheckDuplicationPackageItineraryImg2(string Photo2, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            string fileName = Path.GetFileName(Photo2);
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItineraryImg2/" + fileName + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + fileName + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }
        public async Task<JsonResult> CheckDuplicationPackageItineraryImg3(string Photo3, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            string fileName = Path.GetFileName(Photo3);
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItineraryImg3/" + fileName + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + fileName + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }
        public async Task<JsonResult> CheckDuplicationPackageItineraryImg4(string Photo4, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            string fileName = Path.GetFileName(Photo4);
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItineraryImg4/" + fileName + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + fileName + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }

        public async Task<JsonResult> CheckDuplicationPackageItineraryImg5(string Photo5, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            string fileName = Path.GetFileName(Photo5);
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageItinerary/CheckDuplicationPackageItineraryImg5/" + fileName + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + fileName + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }



    }
}