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
using WebAppColorStays.Areas.ColorStays.CommonMethods;
using LibCommon.UserAgent;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class PackageController : Controller
    {
        private readonly Paging paging;
        private readonly RyCSImage ryCsImage;
        public PackageController()
        {
            paging = new Paging();
            ryCsImage = new RyCSImage();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "Package";
        }
        //End


        //Drop Down
        public async void DropDown(string CompId, string Token)
        {
            RyCrSsDropDown ry = new RyCrSsDropDown();
            string URLContinent = "PackageType/DropDown/" + CompId;
            string URLCancellation = "CancellationPolicyType/DropDown/" + CompId;
            string URLTerms = "TermsAndCondition/DropDown/" + CompId;
            try
            {
                Task<List<SelectListItem>> PackageType = ry.DDColorStaysAPI(URLContinent, Token);
                Task<List<SelectListItem>> PolicyType = ry.DDColorStaysAPI(URLCancellation, Token);
                Task<List<SelectListItem>> Terms = ry.DDColorStaysAPI(URLTerms, Token);

                Task.WaitAll(PackageType, PolicyType, Terms);
                ViewBag.PackageType = PackageType;
                ViewBag.PolicyType = PolicyType;
                ViewBag.Terms = Terms;
            }
            catch (Exception ex) { }

        }

        [HttpGet]
        public async Task<IActionResult> PackageCity(string PeId)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            CsPackageCityMap cityMap = new CsPackageCityMap();
            cityMap.Fk_Package_Name = PeId;
            return View("_CreateOrEditCity", cityMap);
        }

        //check dupchek
        public async Task<JsonResult> CheckDuplicationPackageCityMap(int SerialNo, string NameAction, string Id, string Fk_Package_Name)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageCityMap/CheckDuplicationPackageCityMap/" + SerialNo + "/" + NameAction + "/" + Fk_Package_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + SerialNo + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPackageCity(CsPackageCityMap csPackageCityMap)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            csPackageCityMap.CompId = CompID;
            csPackageCityMap.CreatedBy = UserID;
            csPackageCityMap.ModifiedBy = UserID;

            ViewData["ResponseName"] = "ShowValidation";
            CsPackageCityMap data = new CsPackageCityMap();

            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        csPackageCityMap.Id = Guid.NewGuid().ToString();
                        StringContent content = new StringContent(JsonSerializer.Serialize(csPackageCityMap), Encoding.UTF8, "application/json");

                        using (var response = await client.PostAsync("PackageCityMap/create", content))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackageCityMap>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                                Success = true;

                            }
                            else
                            {
                                Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                                if (responsemsg != null && responsemsg.Message == "DuplicateCity")
                                {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                    ModelState.AddModelError("Location", "Duplicate Value, Already Exists !");
                                }
                                if (responsemsg != null && responsemsg.Message == "DuplicateSerial")
                                {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                    ModelState.AddModelError("SerialNo", "Duplicate Value, Already Exists !");
                                }

                                Success = false;
                            }
                        }

                    }

                if (Success == true)
                {
                    return RedirectToAction("PackageCityList", new { PeId = csPackageCityMap.Fk_Package_Name });
                }
                else { return View("_CreateOrEditCity", csPackageCityMap); }
            }
            return View("_CreateOrEditCity", csPackageCityMap);
        }

        //here we get the list of cities of package
        [HttpGet]
        public async Task<IActionResult> PackageCityList(string PeId)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            //Display the Dropdown of the Table fields in Search Data Popup
            GetClassMember<CsPackageCityMap> getClassMember = new GetClassMember<CsPackageCityMap>();
            CsPackageCityMap CsPackageCityMap = new CsPackageCityMap();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsPackageCityMap), "Value", "DisplayName");
            //Ends

            try //Pagination and List of data Code
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("PackageCityMap/index/" + CompID + "/" + PeId, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPackageCityMap>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

                            ViewData["ActionName"] = "Index";
                            ViewData["FormID"] = "NoSearchID";
                            ViewData["SearchType"] = "NoSearch";
                            return View(data);
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
                return View("Error");
            }


            Title();
            ViewBag.Action = "RolesAssign";
           

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageCityMap/index/" + CompID + "/" + PeId, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPackageCityMap>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View(data);
                    }
                }
            }
            return View("Error");
        }

        //Delete the package city
        [HttpGet]
        public async Task<IActionResult> DeletePackageCity(string Id, string PeId)
        {
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageCityMap/DeleteConfirmed/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("PackageCityList", new { PeId = PeId });
                    }
                }
            }
            return View("Error");
        }


        //AutoComplete
        [HttpGet]
        public async Task<JsonResult> SuggestPackage(string term)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var UserID = Request.Cookies["UserID"];
            List<CsAutocomplete> list = new List<CsAutocomplete>();
            var CompId = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                HttpResponseMessage res = await client.GetAsync("Package/SuggestPackage/" + term + "/" + CompId, HttpCompletionOption.ResponseHeadersRead);
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

        [HttpPost]
        public async Task<IActionResult> SaveImage(string PId)
        {
            var TokenKey = Request.Cookies["JWToken"];

            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = HttpContext.Request.Form.Files;
            //var uploads = Path.Combine(Environment.WebRootPath, "Image\\Country");

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                foreach (var file in files)
                {
                    var fileName = file.FileName;
                    //StringContent content = new StringContent(JsonSerializer.Serialize(file), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Package/SaveImage/?PId=" + PId + "&CompId=" + CompID + "&UserId=" + UserID + "&FileName=" + fileName, null))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            return View("Error");
                        }
                    }

                    if (file.Length > 0)
                    {
                        Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Package");
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
        public async Task<Tuple<int, List<CsPackage>>> AllDataList(int? PgSize, int? PgSelectedNum)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tuple<int, List<CsPackage>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/index/" + CompID + "/" + PgSize + "/" + PgSelectedNum + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackage>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else{ list = null; }
                }
            }
            return list;
        }
        //Ends


        //GET:/Package/
        [HttpGet]
        public async Task<IActionResult> Index(int? PgSelectedNum, int? PgSize, string PageCall, string? Id)
        {         
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Title();

            //Display the Dropdown of the Table fields in Search Data Popup
            GetClassMember<CsPackage> getClassMember = new GetClassMember<CsPackage>();
            CsPackage CsPackage = new CsPackage();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsPackage), "Value", "DisplayName");
            //Ends

            try //Pagination and List of data Code
            {
                Tuple<int, int> pagedata = await paging.PaginationData(PgSize, PgSelectedNum);//Give the Page Size and Page No
                Task<Tuple<int, List<CsPackage>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination

                ViewData["ActionName"] = "Index";
                ViewData["FormID"] = "NoSearchID";
                ViewData["SearchType"] = "NoSearch";
                ViewData["PId"] = Id;
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
                Tuple<int, List<CsPackage>> list;
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    //Get the List of data
                    using (var response = await client.PostAsJsonAsync("Package/DateSearch/", cIndex))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackage>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        public async Task<IActionResult> TableSearch(CsPackage CsPackage, IFormCollection fc)
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

            Tuple<int, List<CsPackage>> list;

            using (HttpClient client = APIColorStays.Initial())
            {
                CsPackage.CreatedBy = UserID;
                CsPackage.CompId = CompID;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(CsPackage), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Package/TableSearch/?PageSelectedNum=" + pagedata.Item2 + "&PageSize=" + pagedata.Item1, content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackage>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            GetClassMember<CsPackage> getClassMember = new GetClassMember<CsPackage>();
            CsPackage CsPackage = new CsPackage();
            ViewBag.List = new SelectList(getClassMember.GetPropertyDisplayName(CsPackage), "Value", "DisplayName");
            //Creating Search Filter List with class member Property Name
            Dictionary<string, string> fields = getClassMember.GetPropertyName(CsPackage);
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

            Tuple<int, List<CsPackage>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
               indexsearchfilter.CurrentUserId = UserID;
                indexsearchfilter.CompId = CompID;
                indexsearchfilter.PageSelectedNum = pagedata.Item2;
                indexsearchfilter.PageSize = pagedata.Item1;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(indexsearchfilter), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Package/FilterSearch", content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPackage>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Package/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            DropDown(CompID, TokenKey);
            CsPackage CsPackage = new CsPackage();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/details/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsPackage = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackage>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_DetailOrDelete",CsPackage);
                    }
                    else
                    {
                        Tuple<CsPackage, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPackage, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Package/CreateOrEdit
        [HttpGet]
        [ResponseCache(Duration = 0)]
        public async Task<IActionResult> CreateOrEdit(string Id)
        {
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            DropDown(CompID, TokenKey);
            ViewData["ResponseName"] = "ShowValidation";
            if (Id != null)
            {
                bool Success = false;
                var data = new CsPackage();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("Package/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackage>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        
        //GET: /Package/Create
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
            Task<Tuple<int, List<CsPackage>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
            PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
            ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
            return View();
        } 
        

        //POST: /Package/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CsPackage CsPackage)
        {       
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            DropDown(CompID, TokenKey);
            CsPackage.CompId = CompID;
            CsPackage.CreatedBy = UserID;
            CsPackage.ModifiedBy = UserID;
            CsPackage.Id = Guid.NewGuid().ToString();
            ViewData["ResponseName"] = "ShowValidation";
            CsPackage data = new CsPackage();
            var files = HttpContext.Request.Form.Files;
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    foreach (var file in files)
                    {
                        var fileName = file.FileName;

                        if ("CoverImage" == file.Name)
                        {
                            CsPackage.CoverImage = fileName;
                        }
                        if ("VideoImage" == file.Name)
                        {
                            CsPackage.VideoImage = fileName;
                        }

                        if (file.Length > 0)
                        {
                            Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "PackageBanner");
                            Task.WaitAll(TImgUpload);
                            if (TImgUpload.Result == "Error")
                            {
                                return View("Error");
                            }
                        }
                    }
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(CsPackage), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Package/create", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackage>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                else { return View("_CreateOrEdit", CsPackage); }
            }
            return View("_CreateorEdit",CsPackage);                
         }


        //GET: /Package/Edit/5
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

            CsPackage CsPackage = new CsPackage();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsPackage>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;                   
                    if (response.IsSuccessStatusCode)
                    {
                        CsPackage = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackage>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return View("Edit",CsPackage);
        }

                
        //POST: /Package/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPgDetail(CsPackage CsPackage)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            DropDown(CompID, TokenKey);
            ViewData["ResponseName"] = "ShowValidation";
            CsPackage.CompId = CompID;
            CsPackage.ModifiedBy = UserID;
            var files = HttpContext.Request.Form.Files;
            if (CsPackage.CoverImageName != null)
            {
                CsPackage.CoverImage = CsPackage.CoverImageName;
            }
            if (CsPackage.VideoImageName != null)
            {
                CsPackage.VideoImageName = CsPackage.VideoImageName;
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

                            if ("CoverImage" == file.Name)
                            {
                                CsPackage.CoverImage = fileName;
                                //Delete the Images from the folder
                                Task<string> TDeleteImage = ryCsImage.DeleteImage(CsPackage.CoverImageName, TokenKey, "PackageBanner");
                                Task.WaitAll(TDeleteImage);
                            }
                            if ("VideoImage" == file.Name)
                            {
                                CsPackage.VideoImage = fileName;
                                //Delete the Images from the folder
                                Task<string> TDeleteImage1 = ryCsImage.DeleteImage(CsPackage.VideoImageName, TokenKey, "PackageBanner");
                                Task.WaitAll(TDeleteImage1);
                            }

                            if (file.Length > 0)
                            {
                                Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "PackageBanner");
                                Task.WaitAll(TImgUpload);
                                if (TImgUpload.Result == "Error")
                                {
                                    return View("Error");
                                }
                            }
                        }
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<CsPackage>("Package/edit", CsPackage))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsPackage, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPackage,Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                    return RedirectToAction("Index", new { PageCall = "ShowIxSh", CsPackage.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreateorEdit");
                }
            }
            return View("_CreateorEdit",CsPackage);
        }
        
       
        //POST: /Package/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));

            CsPackage csPackage = new CsPackage();
            List<CsPackageImage> photosList = new List<CsPackageImage>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    csPackage = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPackage>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageImage/Index/" + id))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    photosList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPackageImage>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }

            //Delete the Images from the folder
            List<string> image = new List<string>();
            foreach (var item in photosList)
            {
                string img;
                img = item.Title;
                image.Add(img);
            }

            if (image.Count > 0)
            {
                Task<string> TDeleteImage = ryCsImage.DeleteMultiImage(image, "Package", TokenKey);
                Task.WaitAll(TDeleteImage);
                if (TDeleteImage.Result == "Error")
                {
                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                }
            }
            if (csPackage.VideoImageName != null)
            {
                Task<string> TDeleteImage3 = ryCsImage.DeleteImage(csPackage.VideoImageName, TokenKey, "PackageBanner");
                Task.WaitAll(TDeleteImage3);
                if (TDeleteImage3.Result == "Error")
                {
                    ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
                }
            }
            Task<string> TDeleteImage1 = ryCsImage.DeleteImage(csPackage.CoverImageName, TokenKey, "PackageBanner");
            
            Task.WaitAll(TDeleteImage1);

            if (TDeleteImage1.Result == "Error")
            {
                ViewData["ErrorMessage"] = "Try Again!"; return View("_ErrorGeneric");
            }


            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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
			CsPackage CsPackage = new CsPackage();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Package/verifydata/" , content))
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
            starUnstar.ControllerName = "Package";
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
        public async Task<JsonResult> CheckDuplicationPackage(string Name, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/CheckDuplicationPackage/" + Name + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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


        //Package Exclusion
        [HttpGet]
        public async Task<IActionResult> PackageExclusionList(string PeId)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            PackageExclusionCheckbox facilityList = new PackageExclusionCheckbox();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageExclusionMap/PackageExclusionList/" + PeId + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    facilityList = await System.Text.Json.JsonSerializer.DeserializeAsync<PackageExclusionCheckbox>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            return View("_CreateOrEditPackageExclusion", facilityList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPackageExclusion(PackageExclusionCheckbox model)
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
                    using (var response = await client.PostAsync("PackageExclusionMap/AddPackageExclusion", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            return RedirectToAction("GetPackageExclusion", new { id = model.Fk_Package_Name });
                        }
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> GetPackageExclusion(string PeId)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            List<CsPackageInclusionMap> data = new List<CsPackageInclusionMap>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageExclusionMap/GetPackageExclusion/" + PeId + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPackageInclusionMap>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        ViewBag.PackageExclusion = data;
                        return View();
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> DeletePackageExclusion(string PeId, string Id)
        {
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageExclusionMap/DeleteConfirmed/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetPackageExclusion", new { id = PeId });
                    }
                }
            }
            return View("Error");
        }


        //Package Inclusion
        [HttpGet]
        public async Task<IActionResult> PackageInclusionList(string PeId)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";
            PackageInExCheckbox facilityList = new PackageInExCheckbox();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageInclusionMap/PackageInclusionList/" + PeId + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    facilityList = await System.Text.Json.JsonSerializer.DeserializeAsync<PackageInExCheckbox>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            return View("_CreateOrEditPackageInclusion", facilityList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPackageInclusion(PackageInExCheckbox model)
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
                    using (var response = await client.PostAsync("PackageInclusionMap/AddPackageInclusion", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            return RedirectToAction("GetPackageInclusion", new { id = model.Fk_Package_Name });
                        }
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> GetPackageInclusion(string PeId)
        {
            Title();
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            List<CsPackageInclusionMap> data = new List<CsPackageInclusionMap>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageInclusionMap/GetPackageInclusion/" + PeId + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPackageInclusionMap>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        ViewBag.PackageInclusion = data;
                        return View();
                    }
                }
            }
            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> DeletePackageInclusion(string PeId, string Id)
        {
            ViewBag.Action = "RolesAssign";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("PackageInclusionMap/DeleteConfirmed/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetPackageInclusion", new { id = PeId });
                    }
                }
            }
            return View("Error");
        }





        //GET: /PackageItinerary/Create
        [HttpGet]
        public async Task<IActionResult> CreateItinerary(string PeId)
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
            return View("_CreateorEditPackageItinerary", csPackageItinerary);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItinerary(CsPackageItinerary CsPackageItinerary, IFormCollection fc)
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            CsPackageItinerary.CompId = CompID;
            CsPackageItinerary.ModifiedBy = UserID;
            if (CsPackageItinerary.Remarks != null)
            {
                CsPackageItinerary.Remarks = "null";
            }
            CsPackageItinerary.DayActivity = Convert.ToString(fc["DayActivity"]);
            ViewData["ResponseName"] = "ShowValidation";
            CsPackageItinerary data = new CsPackageItinerary();
            var files = HttpContext.Request.Form.Files;

            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    if (CsPackageItinerary.Id == null)
                    {
                        foreach (var file in files)
                        {

                            var fileName = file.FileName;
                            if ("Photo1" == file.Name)
                            {
                                CsPackageItinerary.Photo1 = fileName;
                            }
                            if ("Photo2" == file.Name)
                            {
                                CsPackageItinerary.Photo2 = fileName;
                            }
                            if ("Photo3" == file.Name)
                            {
                                CsPackageItinerary.Photo3 = fileName;
                            }
                            if ("Photo4" == file.Name)
                            {
                                CsPackageItinerary.Photo4 = fileName;
                            }
                            if ("Photo5" == file.Name)
                            {
                                CsPackageItinerary.Photo5 = fileName;
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
                        CsPackageItinerary.Id = Guid.NewGuid().ToString();
                        CsPackageItinerary.CreatedBy = UserID;

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
                                    ModelState.AddModelError("DayName", "Duplicate Value, Already Exists !");
                                }
                                if (responsemsg != null && responsemsg.Message == "Duplicate1")
                                {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                    ModelState.AddModelError("DayNo", "Duplicate Value, Already Exists !");
                                }
                                Success = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (var file in files)
                        {
                            var fileName = file.FileName;

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
                        using (var response = await client.PostAsJsonAsync<CsPackageItinerary>("PackageItinerary/edit", CsPackageItinerary))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            Success = true;

                            if (!response.IsSuccessStatusCode)
                            {
                                Tuple<CsPackageItinerary, Response> data1 = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPackageItinerary, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

                                if (data1.Item2 != null)
                                {
                                    if (data1.Item2.Message == "Duplicate")
                                    {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                        ModelState.AddModelError("DayName", "Duplicate Value, Already Exists !");
                                    }
                                    if (data1.Item2.Message == "Duplicate1")
                                    {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                        ModelState.AddModelError("DayNo", "Duplicate Value, Already Exists !");
                                    }
                                    if (data1.Item2.Message == "GlobalItem")
                                    {
                                        ViewBag.Message = "Sytem Entry, Can't Change !";
                                    }
                                    if (data1.Item2.Message == "Verified")
                                    {
                                        ViewBag.Message = "You can not Edit Verified Entry !";
                                    }
                                }
                                Success = false;
                            }
                        }
                    }
                }
                if (Success == true)
                {
                    return RedirectToAction("Index", "PackageItinerary", new { PeId = CsPackageItinerary.Fk_Package_Name });
                }
                else { return View("_CreateOrEditPackageItinerary", CsPackageItinerary); }
            }
            return View("_CreateOrEditPackageItinerary", CsPackageItinerary);
        }


        //GET: /PackageItinerary/Edit/5
        [HttpGet]
        public async Task<ActionResult> EditItinerary(string id)
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
                        CsPackageItinerary.Remarks = CsPackageItinerary.DayActivity;
                        CsPackageItinerary.Video = CsPackageItinerary.DayActivity1;

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
            return View("_CreateorEditPackageItinerary", CsPackageItinerary);
        }


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
            return RedirectToAction("EditItinerary", new { Id });
        }
        //ends
        //Get the list of itinerary
        [HttpGet]
        public async Task<IActionResult> ItineraryList(string PeId)
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

                return View(list.Item2);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        //This method is to check duplicate values for specific columns......
        public async Task<JsonResult> CheckDuplicationPackageRank(int Rank, string NameAction, string Fk_Country_Name, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            if (Fk_Country_Name == null) { Fk_Country_Name = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Package/CheckDuplicationPackageRank/" + Rank + "/" + NameAction + "/" + Fk_Country_Name + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Success = true;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json("Sorry, this " + Rank + " already exists");
                    }
                }
            }
            if (Success == true) { return Json(true); }
            else { return Json("Some Error!"); }
        }

    }
}