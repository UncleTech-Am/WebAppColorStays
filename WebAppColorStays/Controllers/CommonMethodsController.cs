using LibCommon.Service;
using LibCompanyService.Models.ViewCompany;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using LibCommon.DataTransfer;
using UncleTech.Encryption;
using System.Security.Claims;
using NuGet.Common;

namespace WebAppColorStays.Controllers
{
    public class CommonMethodsController : Controller
    {
        //Show the User Companies Dropdown in the Layout
        [HttpGet]
        public async Task<IActionResult> UserCompanies()
        {
            var TokenKey = Request.Cookies["JWToken"];
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<CsCompany> complist = new List<CsCompany>();
            using (HttpClient client1 = APIComp.Initial())
            {
                client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var ddresponse = await client1.GetAsync("MyBusiness/MappedCompanyList/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var ddapiResponse = await ddresponse.Content.ReadAsStreamAsync();
                    complist = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CsCompany>>(ddapiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                }
            }
            return Json(complist);
        }
        //Ends

        //Show the Clipped data Count of Current User
        [HttpGet]
        public async Task<IActionResult> ClippedDataCount()
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int Count = 0;
            using (HttpClient client = APIComp.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("UserClip/ClippedDataCount/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        Count = await System.Text.Json.JsonSerializer.DeserializeAsync<int>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return Json(Count);
                    }
                }
            }
            return Json(Count);
        }
        //Ends

        //Show the Star Marked/Clipped data of the Current User
        [HttpGet]
        public async Task<IActionResult> UserAllClippedData()
        {

            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ResponseName"] = "ShowValidation";
            ViewBag.ctrName = "Company";
            ViewBag.Title = "ClippedData";
            List<UserClippedData> userclippeddata = new List<UserClippedData>();
            using (HttpClient client = APIComp.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("UserClip/UserAllClippedData/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        userclippeddata = await System.Text.Json.JsonSerializer.DeserializeAsync<List<UserClippedData>>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_UserClippedData", userclippeddata);
                    }
                    else
                    {
                        return View("_ErrorGeneric");
                    }
                }
            }
        }
        //Ends

        //Get Methods of EditClippedData
        [HttpGet]
        public async Task<IActionResult> EditClippedData(string Id)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ResponseName"] = "ShowValidation";
            ViewBag.ctrName = "Company";
            ViewBag.Title = "ClippedData";
            UserClippedData data = new UserClippedData();
            using (HttpClient client = APIComp.Initial())
            {
                using (var response = await client.GetAsync("UserClip/EditClippedData/" + Id, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        data = await System.Text.Json.JsonSerializer.DeserializeAsync<UserClippedData>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        return View("_EditClippedData", data);
                    }
                }
            }
            return View("Error");
        }

        //POST: Methods of EditClippedData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClippedData(UserClippedData userclippeddata)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ResponseName"] = "ShowValidation";
            ViewBag.ctrName = "Company";
            ViewBag.Title = "ClippedData";
            if (ModelState.IsValid)
            {
                try
                {
                    CsCompany data = new CsCompany();
                    using (HttpClient client = APIComp.Initial())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                        using (var response = await client.PostAsJsonAsync<UserClippedData>("UserClip/EditClippedData", userclippeddata))
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                ViewData["Message"] = "Data Edited!";
                                return View("_SuccessWithMsg");
                            }
                            else
                            {
                                return View("_ErrorGeneric");
                            }
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Message = "Not Found";
                    return View("_CreateorEdit");
                }
            }
            return View("_CreateorEdit", userclippeddata);
        }

        //POST: Delete the Particular Clipped Data
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClippedData(string id)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            ViewBag.ctrName = "Company";
            ViewBag.Title = "Company";
            ViewBag.Action = "Delete";
            using (HttpClient client = APIComp.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("UserClip/DeleteClippedData/" + id, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ViewData["Message"] = "Data Edited!";
                        return View("_SuccessWithMsg");
                    }
                    else
                    {
                        var responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                        if (responsemsg.Message == "DeleteStop") { return View("DeleteStop"); }
                        else { return View("_ErrorGeneric"); }
                    }
                }
            }
        }

    }
}
