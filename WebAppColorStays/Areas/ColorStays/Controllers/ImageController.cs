using LibAuthService.ModelView;
using LibCommon.DataTransfer;
using LibCommon.Service;
using LibCommon.UserAgent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebAppColorStays.Models.ViewModel;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{
    [Area("ColorStays")]
    [SessionCheck]
    [Authorize]

    public class ImageController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            ViewData["AnName"] = "Create";
            var TokenKey = Request.Cookies["JWToken"];
            var UserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool Success = false;
            ViewData["ResponseName"] = "ShowValidation";
            var files = HttpContext.Request.Form.Files;
            int i = 0;
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                i = i + 1;

                        byte[] data;
                        using (var br = new BinaryReader(file.OpenReadStream()))
                            data = br.ReadBytes((int)file.OpenReadStream().Length);

                        ByteArrayContent bytes = new ByteArrayContent(data);
                        multiContent.Add(bytes, "file", file.FileName);

                        
            }
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);

                using (var response = await client.PostAsync("Image/Upload", multiContent))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
                return null;
        }


        [HttpGet]
        public async Task<ActionResult> GetImage(string id)
        {
            var TokenKey = Request.Cookies["JWToken"];
            if (id == null)
            {
                ViewBag.Message = "Not Founded";
                return View();
            }

            CsImage CsImage = new CsImage();
            using (HttpClient client = APIColorStays.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                using (var response = await client.GetAsync("Image/GetImage/" + id, HttpCompletionOption.ResponseHeadersRead))
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CsImage = await System.Text.Json.JsonSerializer.DeserializeAsync<CsImage>(apiResponse, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
            return View(CsImage);
        }

    }
}
