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
using WebAppColorStays.Model.ViewModel;
using LibCommon.APICommonMethods;



namespace WebAppColorStays.Areas.ColorStays.Controllers
{   
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]
    public class BgImageController : Controller
    {
        private readonly Paging paging;
        private readonly RyCSImage ryCsImage;


        public BgImageController()
        {
            paging = new Paging();
            ryCsImage = new RyCSImage();
        }

        //Show the Title in View
        private void Title()
        {
            ViewBag.Title = "BgImage";
        }
        //Ends

        //POST: /BgImage/Create
        [HttpPost]
        public async Task<IActionResult> Create(string BlogId, string Title)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            CsBlogImage bgimage = new CsBlogImage();
            var files = HttpContext.Request.Form.Files;
            try
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5) + file.FileName;

                            using (var response = await client.GetAsync("BgImage/Create/" + BlogId + "/" + CompID + "/" + UserID + "/" + fileName, HttpCompletionOption.ResponseHeadersRead))
                            {
                                var apiResponse = await response.Content.ReadAsStreamAsync();
                                if (!response.IsSuccessStatusCode)
                                {
                                    return View("Error");
                                }
                            }

                            //Save the Images in the Folder
                            Task<string> TImgUpload = ryCsImage.UploadWebImages(file, fileName, TokenKey, "Blog");
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
            catch (Exception e)
            {

                throw;
            }

        }

        //show upload the image
        [HttpGet]
        public async Task<IActionResult> Index(string BlogId, string UpdateDetail, string ShowBtn)
        {
            var TokenKey = Request.Cookies["JWToken"];
            List<CsBlogImage> blogimglist = new List<CsBlogImage>();

            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("BgImage/Index/" + BlogId))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    blogimglist = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsBlogImage>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            if (ShowBtn == "false")
            {
                ViewBag.ShowBtn = "false";
            }
            if (UpdateDetail == "Yes") { return PartialView("_AddImageDetail", blogimglist); }
            return PartialView("UploadedImage", blogimglist);
        }
        //ends

        //POST: /BgImage/Delete/5
        [HttpGet]
        public async Task<IActionResult> DeleteConfirmed(string id, string BlogId, string ImgName)
        {
            Title();
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("BgImage/deleteconfirmed/" + id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        //Delete the Images from the folder
                        Task<string> TDeleteImage = ryCsImage.DeleteImage(ImgName, TokenKey, "Blog");
                        Task.WaitAll(TDeleteImage);

                        return RedirectToAction("Index", new { BlogId = BlogId });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CsBlogImage tblblogimage)
        {
            Title();
            ViewData["AnName"] = "Edit";
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            tblblogimage.CompId = CompID;
            tblblogimage.ModifiedBy = UserID;
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var response = await client.PostAsJsonAsync<CsBlogImage>("BgImage/edit", tblblogimage))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            Tuple<CsBlogImage, Response> data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<CsBlogImage, Response>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            if (tblblogimage.CoverPic == true)
            {
                CsBlog csblog = new CsBlog();
                csblog.Id = tblblogimage.Fk_Blog_Id;
                csblog.ImageUrl = tblblogimage.ImageName;
                csblog.Remarks = "CreateEdit";
                csblog.CompId = CompID;
                csblog.ModifiedBy = UserID;
                var tags = csblog.Tags;
                if (csblog.Tags == null)
                {
                    string[] notags = { "No" };
                    tags = notags;
                }
                Tuple<CsBlog, string[]?> model = Tuple.Create(csblog, tags);
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Bg/edit", content))
                    { }
                }
            }
            return RedirectToAction("Index", new { BlogId = tblblogimage.Fk_Blog_Id });
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
			CsBlogImage CsBlogImage = new CsBlogImage();
            using (HttpClient client = APIColorStays.Initial())
            {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
				StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("BgImage/verifydata/" , content))
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
            starUnstar.ControllerName = "BgImage";
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
        public async Task<JsonResult> CheckDuplicationBgImage(string Name, string NameAction, string Id)
        {
            bool Success = false;
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            if (Id == null) { Id = "No"; }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("BgImage/CheckDuplicationBgImage/" + Name + "/" + NameAction + "/" + Id + "/" + CompID, HttpCompletionOption.ResponseHeadersRead))
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