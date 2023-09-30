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

namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly Paging paging;

        public PhotoController()
        {
            paging = new Paging();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "Photo";
        }
        //Ends

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
        public async Task<Tuple<int, List<CsPhoto>>> AllDataList(int? PgSize, int? PgSelectedNum)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tuple<int, List<CsPhoto>> list;
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/index/" + CompID + "/" + PgSize + "/" + PgSelectedNum + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<int, List<CsPhoto>>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                    else{ list = null; }
                }
            }
            return list;
        }
        //Ends

        //show upload the image
        [HttpGet]
        public async Task<IActionResult> Index(string? CountryId, string? StateId, string? CityId, string? PlaceId, string? UpdateDetail, string? ShowBtn)
        {
            var TokenKey = Request.Cookies["JWToken"];
            List<CsPhoto> photosList = new List<CsPhoto>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/Index/" + CountryId + "/" + StateId + "/" + CityId + "/" + PlaceId))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    photosList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPhoto>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            if (ShowBtn == "false")
            {
                ViewBag.ShowBtn = "false";
            }
            if (UpdateDetail == "Yes") { return PartialView("_AddImageDetail", photosList); }
            return PartialView("UploadedImage", photosList);
        }
        //ends


        //show upload the image
        [HttpGet]
        public async Task<IActionResult> UploadedImage(string? CountryId, string? StateId, string? CityId, string? PlaceId, string ShowBtn)
        {

            var TokenKey = Request.Cookies["JWToken"];
            List<CsPhoto> csPhotos = new List<CsPhoto>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/UploadedImage/" + CountryId + "/" + StateId + "/" + CityId + "/" + PlaceId, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    csPhotos = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPhoto>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            if (ShowBtn == "false")
            {
                ViewBag.ShowBtn = "false";
            }
            return PartialView("UploadedImage", csPhotos);
        }
        //ends

        //Delete the upload image
        public async Task<IActionResult> ImageDelete(string ImageID, string? CountryId, string? StateId, string? CityId, string  PlaceId, string ImgName)
        {
            var TokenKey = Request.Cookies["JWToken"];

            List<CsPhoto> csPhotos = new List<CsPhoto>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/ImageDelete/" + ImageID + "/" + CountryId +"/"+StateId +"/"+ CityId + "/"+ PlaceId, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    csPhotos = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsPhoto>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

                }
                using (HttpClient client1 = APICSImages.Initial())
                {
                    client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);

                    if (CountryId != null)
                    {
                        using (var response = await client1.GetAsync("Images/DeleteWebImage/" + "Country" + "/" + ImgName, HttpCompletionOption.ResponseHeadersRead))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                return View("Error");
                            }
                        }
                    }

                    if (StateId != null)
                    {
                        using (var response = await client1.GetAsync("Images/DeleteWebImage/" + "State" + "/" + ImgName, HttpCompletionOption.ResponseHeadersRead))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                return View("Error");
                            }
                        }
                    }
                    if (CityId != null)
                    {
                        using (var response = await client1.GetAsync("Images/DeleteWebImage/" + "City" + "/" + ImgName, HttpCompletionOption.ResponseHeadersRead))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                return View("Error");
                            }
                        }
                    }
                    if (PlaceId != null)
                    {
                        using (var response = await client1.GetAsync("Images/DeleteWebImage/" + "Place" + "/" + ImgName, HttpCompletionOption.ResponseHeadersRead))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                return View("Error");
                            }
                        }
                    }
                }
                //Delete the Images from the folder
                //Task<string> TDeleteImage = ryImage.DeleteImage(ImgName, TokenKey, "CompUser");
                //Task.WaitAll(TDeleteImage);
            }
            return PartialView("UploadedImage", csPhotos);
        }
        //ends





        //GET: /Photo/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            CsPhoto CsPhoto = new CsPhoto();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/details/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsPhoto = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPhoto>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_DetailOrDelete",CsPhoto);
                    }
                    else
                    {
                        Tuple<CsPhoto, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPhoto, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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


        //GET: /Photo/CreateOrEdit
        [HttpGet]
        [ResponseCache(Duration = 0)]
        public async Task<IActionResult> CreateOrEdit(string Id)
        {
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ResponseName"] = "ShowValidation";
            if (Id != null)
            {
                bool Success = false;
                var data = new CsPhoto();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.GetAsync("Photo/edit/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPhoto>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
        
        //GET: /Photo/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
            Task<Tuple<int, List<CsPhoto>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
            PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
            ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;
            return View();
        } 
        

        //POST: /Photo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CsPhoto CsPhoto)
        {       
            Title();
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            CsPhoto.CompId = CompID;
            CsPhoto.CreatedBy = UserID;
            CsPhoto.ModifiedBy = UserID;
            CsPhoto.Id = Guid.NewGuid().ToString();
            ViewData["ResponseName"] = "ShowValidation";
            CsPhoto data = new CsPhoto();
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
				    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(CsPhoto), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Photo/create", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPhoto>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
                { return RedirectToAction("Index", new { PageCall = "Show" }); }
                else { return View("_CreateOrEdit", CsPhoto); }
            }
            return View("_CreateorEdit",CsPhoto);                
         }


        //GET: /Photo/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            Title();
            ViewData["AnName"] = "Edit";
            ViewData["Id"] = id;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                ViewBag.Message = "Not Founded";
                return View();
            }

            ViewData["ActionName"] = "Index";
            ViewData["FormID"] = "NoSearchID";
            ViewData["SearchType"] = "NoSearch";

            CsPhoto CsPhoto = new CsPhoto();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/edit/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    Tuple<int, int> pagedata = await paging.PaginationData(null, null);//Give the Page Size and Page No
                    Task<Tuple<int, List<CsPhoto>>> ReturnDataList = AllDataList(pagedata.Item1, pagedata.Item2);//Give the List of data
                    PaginationViewData(pagedata.Item2, ReturnDataList.Result.Item1, pagedata.Item1);//Give the ViewData value for Pagination
                    ViewData["EnteredDetails"] = ReturnDataList.Result.Item2;                   
                    if (response.IsSuccessStatusCode)
                    {
                        CsPhoto = await System.Text.Json.JsonSerializer.DeserializeAsync<CsPhoto>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return View(CsPhoto);
        }


        //POST: /Photo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CsPhoto csPhoto)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            csPhoto.CompId = CompID;
            csPhoto.ModifiedBy = UserID;
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.PostAsJsonAsync<CsPhoto>("Photo/edit", csPhoto))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            Tuple<CsPhoto, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsPhoto, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                            if (data.Item2 != null)
                            {
                                if (data.Item2.Message == "Duplicate")
                                {   //Here Replace the ID With The Key Name That has to Be checked for the duplication.
                                    ModelState.AddModelError("ImageName", "Duplicate Value, Already Exists !");
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
            }
            if (csPhoto.Fk_Country_Name != null)
            {

                return RedirectToAction("Index", new { CountryId = Base64UrlEncoder.Encode(Process.Encrypt(csPhoto.Fk_Country_Name)), StateId ="null", CityId = "null", PlaceId = "null" });

            }
            if (csPhoto.Fk_State_Name != null)
            {
                return RedirectToAction("Index", new {CountryId = "null", StateId = Base64UrlEncoder.Encode(Process.Encrypt(csPhoto.Fk_State_Name)), CityId = "null", PlaceId = "null" });
            }
            if (csPhoto.Fk_City_Name != null)
            {
                return RedirectToAction("Index", new { CountryId = "null", StateId = "null", CityId = Base64UrlEncoder.Encode(Process.Encrypt(csPhoto.Fk_City_Name)), PlaceId = "null" });
            }
            if (csPhoto.Fk_Place_Name != null)
            {
                return RedirectToAction("Index", new { CountryId = "null", StateId = "null", CityId = "null", PlaceId = Base64UrlEncoder.Encode(Process.Encrypt(csPhoto.Fk_Place_Name)) });
            }
            return NotFound();
        }


        //POST: /Photo/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
			var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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
			var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
			VerNActViewModel model = new VerNActViewModel();
            model.Id = Id;
            model.ActionName = AnName;
            model.CompId = CompID;
            if (model.ActionName == "Verify" || model.ActionName == "UnVerify") { model.VerifiedBy = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); }
            if (model.ActionName == "Activate" || model.ActionName == "Inactivate") { model.ActivatedBy = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); }
			CsPhoto CsPhoto = new CsPhoto();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Photo/verifydata/" , content))
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
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            StarUnstar starUnstar = new StarUnstar();
            starUnstar.Id = Id;
            starUnstar.Host = Request.Scheme + "://" + Request.Host;
            starUnstar.AreaName = "ColorStays";
            starUnstar.ControllerName = "Photo";
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
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
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
        public async Task<JsonResult> CheckDuplicationPhoto(string Name, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Photo/CheckDuplicationPhoto/" + Name + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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