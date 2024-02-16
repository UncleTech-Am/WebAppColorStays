using LibCommon.DataTransfer;
using LibCommon.Service;
using LibCompanyService.Models.ViewCompany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using UncleTech.Encryption;

namespace WebAppColorStays.Controllers
{
    public class AppsViewController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index(string CompId, string CompNe)
        {
            //Check User Company Is Created if true show apps else show Starting Wizard.
            var TokenKey = Request.Cookies["JWToken"];
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //this is used when Current User Change the Company from any where
            //then only change the Cookie value of Company and go to the Apps View
            var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            var CompName = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyNe"]));
            if (CompId != null)
            {
                Response.Cookies.Delete("CompanyID");
                Response.Cookies.Delete("CompanyNe");
                Response.Cookies.Append("CompanyID", CompId, new CookieOptions { HttpOnly = true });
                var cne = Base64UrlEncoder.Encode(Process.Encrypt(CompNe));
                Response.Cookies.Append("CompanyNe", cne, new CookieOptions { HttpOnly = true });
                CompID = Process.Decrypt(Base64UrlEncoder.Decode(CompId));
                CompName = CompNe;
            }
            //Ends

            //this Works when user is Login and check usertype and accroding to that Company
            var UserType = Request.Cookies["UserType"];
            bool Success = false;
            Tuple<List<CsProjectApp>, bool?, string> apps;
            if (CompID == "NoCompany" && UserType == "CompanyUser")
            {
                return RedirectToAction("Login", "Account", new { Area = "", UserNoComp = "UserNoComp" });
            }
            var ProjectId = "5";
            if (CompID == "NoCompany" && UserType == "Owner")
            {
                return RedirectToAction("Create", "MyBusiness", new { area = "Business" });
            }
            else
            {
                using (HttpClient client = APIComp.Initial())
                {
                    //Find the Company Apps
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var caresponse = await client.GetAsync("BusinessApps/CompanyMappedProjectApps/" + CompID + "/" + ProjectId + "/" + UserID, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (caresponse.IsSuccessStatusCode)
                        {
                            var caapiResponse = await caresponse.Content.ReadAsStreamAsync();
                            apps = await System.Text.Json.JsonSerializer.DeserializeAsync<Tuple<List<CsProjectApp>, bool?, string>>(caapiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                            Success = true;
                            if (apps.Item2 == true) { ViewData["ShowApps"] = "true"; }
                            if (apps.Item3 == null) { Response.Cookies.Append("CompLogo", "NoLogo", new CookieOptions { HttpOnly = true }); }
                            else { Response.Cookies.Append("CompLogo", apps.Item3, new CookieOptions { HttpOnly = true }); }
                            return View(apps.Item1);
                        }
                    }
                    //Ends
                }
            }
            //Ends
            return View("_ErrorGeneric");
        }

    }
}