using LibAuthService.ModelView;
using LibCommon.DataTransfer;
using LibCommon.Service;
using LibCompanyService.Models.ViewCompany;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UncleTech.Emailer;
using UncleTech.Encryption;
using static LibAuthService.ModelView.Account;

namespace WebAppColorStays.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        public ActionResult Login()
        {
            return Redirect(ApiSingleSignOn.AccountLoginColorStays());
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            Response.Cookies.Delete("JWToken");
            Response.Cookies.Delete("CompanyID");
            Response.Cookies.Delete("UserType");
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page 
            return RedirectToAction(nameof(Login));
        }

        //Show the Employee Autocomplete
        [SessionCheck]
        [Authorize]
        public async Task<JsonResult> EmployeeAoCe(string term)
        {
            var TokenKey = Request.Cookies["JWToken"];
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            List<User> list = new List<User>();
            using (HttpClient client = APIAuthor.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                HttpResponseMessage res = await client.GetAsync("Account/EmployeeAoCe/" + term + "/" + CompID, HttpCompletionOption.ResponseHeadersRead);
                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStreamAsync().Result;
                    list = await System.Text.Json.JsonSerializer.DeserializeAsync<List<User>>(results, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    var data = list.Select(x => new
                    {
                        id = Base64UrlEncoder.Encode(Process.Encrypt(x.Id)),
                        value = x.FullName,
                        label = x.FullName + "|" + x.PhoneNumber + "|" + x.Email

                    }).ToList();
                    return Json(data);
                }
            }
            return null;
        }
        //Ends

        [SessionCheck]
        [Authorize]
        public async Task<ActionResult> AppJump(string Url)
        {
            var TokenKey = Request.Cookies["JWToken"];
            string email = User.FindFirstValue(ClaimTypes.Email);
            CsAppLoginToken apptoken = new CsAppLoginToken();
            apptoken.ReturnUrl = Url;
            apptoken.Email = email;
            CsAppLoginToken token = new CsAppLoginToken();
            using (HttpClient client = APIAuthor.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                StringContent content = new StringContent(JsonSerializer.Serialize(apptoken), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("Account/SingleSignOnTokenPost", content))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        token = await System.Text.Json.JsonSerializer.DeserializeAsync<CsAppLoginToken>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
            }
            string returntoken = Process.Decrypt(token.Id);
            Url = Url + "?Tok=" + returntoken;
            return Redirect(Url);
        }

        public async Task<ActionResult> Confirm(string Tok)
        {
            using (HttpClient client = APIAuthor.Initial())
            {
                HttpResponseMessage GeResponse = await client.GetAsync("Account/AppLogin/" + Tok, HttpCompletionOption.ResponseHeadersRead);
                if (GeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = GeResponse.Content.ReadAsStreamAsync().Result;

                    CsLoginCookies logincookies = await System.Text.Json.JsonSerializer.DeserializeAsync<CsLoginCookies>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    string token = Convert.ToString(logincookies.Token);
                    string userid = Convert.ToString(logincookies.UserId);
                    string username = Convert.ToString(logincookies.UserName);
                    string useremail = Convert.ToString(logincookies.UserEmail);
                    string compid = Convert.ToString(logincookies.CompanyId);
                    string usertype = Convert.ToString(logincookies.UserType);
                    string roles = Convert.ToString(logincookies.Roles);
                    string timezone = Convert.ToString(logincookies.Timezone);
                    string? logourl = Convert.ToString(logincookies.CompLogo);

                    var claims = new List<Claim>()
                            {
                                new Claim(ClaimTypes.NameIdentifier, userid),
                                new Claim(ClaimTypes.Name,username),
                                new Claim(ClaimTypes.Email,useremail),
                                new Claim(ClaimTypes.Country, timezone)
                            };

                    foreach (var userRole in logincookies.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal
                                , new AuthenticationProperties());

                    HttpContext.Session.SetString("Roles", roles);
                    Response.Cookies.Append("JWToken", token, new CookieOptions { HttpOnly = true });
                    var company = Process.Encrypt(compid);
                    Response.Cookies.Append("CompanyID", company, new CookieOptions { HttpOnly = true });
                    Response.Cookies.Append("UserType", usertype, new CookieOptions { HttpOnly = true });
                    if (logourl == null) { logourl = "NoLogo"; }
                    Response.Cookies.Append("CompLogo", logourl, new CookieOptions { HttpOnly = true });
                    //Prompts User To Change the Password
                    if (logincookies.VerifiedStatus == false)
                    {
                        ViewData["Message"] = "ChangePassword";
                        ViewData["UserEmailId"] = logincookies.UserEmail;
                        return RedirectToAction("Login");
                    }
                    //Ends
                    return RedirectToAction("Index", "AppsView");
                }


            }
            return View("Error");
        }

        // GET: /Account/ChangePassword
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string UserEmailId)
        {

            ViewData["UserEmailId"] = UserEmailId;
            return View("_ChangePassword");
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                ChangePasswordViewModel data = new ChangePasswordViewModel();

                using (HttpClient client = APIAuthor.Initial())
                {
                    StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("account/ChangePassword", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ViewData["Message"] = "Password Changed Successfully!";
                            return View("_ResetSuccessPopUp");
                        }
                        else
                        {
                            var apiResponse = await response.Content.ReadAsStreamAsync();
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<ChangePasswordViewModel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                            if (data.NewPassword == "PasswordMismatch")
                            {
                                ModelState.AddModelError("OldPassword", "Password Incorrect!");
                                ViewData["ResponseName"] = "ShowValidation";
                            }
                            return View("_ChangePassword", data);
                        }
                    }
                }
            }
            return View(model);
        }

        // GET: /Account/PasswordVerify
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordVerify(string UserEmailId)
        {

            ViewData["UserEmailId"] = UserEmailId;
            return View("PasswordVerify");
        }

        //POST: /Account/PasswordVerify
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PasswordVerify(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                LoginViewModel data = new LoginViewModel();

                using (HttpClient client = APIAuthor.Initial())
                {
                    StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("account/PasswordVerify", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            data = await System.Text.Json.JsonSerializer.DeserializeAsync<LoginViewModel>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                            ViewData["ResponseName"] = "SuccessPop";
                            return View("_PasswordVerify", data);
                        }
                        else
                        {
                            ViewData["ResponseName"] = "ShowValidation";
                            Response responsemsg = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                            if (responsemsg.Message == "Password Incorrect")
                            {
                                ModelState.AddModelError("Password", " Password Incorrect");
                            }
                            return View("_PasswordVerify", model);
                        }
                    }
                }
            }
            return View(model);
        }

        //GET: /Account/ChangeEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeEmail(string UserId)
        {

            ViewData["UserId"] = UserId;
            return View("_ChangeEmail");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ChangeEmail(User model)
        {

            var TokenKey = Request.Cookies["JWToken"];
            var CompID = Process.Decrypt(Request.Cookies["CompanyID"]);
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            SeSendEmail seSendEmail = new SeSendEmail();
            model.CompId = CompID;
            model.ModifiedBy = UserID;
            ViewBag.ShowFields = "No";
            ViewData["ResponseName"] = "ShowValidation";
            if (ModelState.IsValid)
            {
                using (HttpClient client = APIAuthor.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("account/ChangeEmail/", content))
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            CsSendEmail csSendEmail = await System.Text.Json.JsonSerializer.DeserializeAsync<CsSendEmail>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

                            string accountstatus = csSendEmail.AccountStatus;

                            switch (accountstatus)
                            {
                                case "EmailChange":

                                    //Send Email For Verification
                                    //Gets the Verification URL And Adds To Body
                                    csSendEmail = ConfirmUrl(csSendEmail);
                                    csSendEmail.EmailTo = model.Email;
                                    await seSendEmail.SendEmailAsync(csSendEmail);
                                    ViewData["Message"] = "Verify the New EmailId then you can use it for LogIn!";
                                    return View("_SuccessWithMsg");

                            }
                        }
                        else
                        {
                            return View("_ErrorGeneric");
                        }
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public CsSendEmail ConfirmUrl(CsSendEmail csSendEmail)
        {
            var callbackUrl = Url.Action("ConfirmUserEmail", "Account", new { UserEmail = csSendEmail.UserEmail, UId = csSendEmail.UserId, Code = csSendEmail.ConfirmCode }, protocol: Request.Scheme);

            var confirmurl = "Please Confirm your Email-Id by clicking <a href=\"" + callbackUrl + "\">Here</a>";

            csSendEmail.EmailBody =
               "<b> Dear " + csSendEmail.UserName + ", </b> <br/> <br/>" +
                csSendEmail.EmailBodyP1 + "<br/>" + confirmurl
                + csSendEmail.EmailBodyP2 + csSendEmail.EmailBodyP3;

            return csSendEmail;
        }
    }
}
