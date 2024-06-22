using LibCommon.DataTransfer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace WebAppColorStays.Areas.ColorStays.CommonMethods
{
    public class RyCrSsDropDown
    {

        public async Task<List<SelectListItem>> HotelDropDown(string Token, string CompID)
        {
            string URL = "";
            try
            {
				List<SelectListItem> list = new List<SelectListItem>();

				using (HttpClient client = APIColorStays.Initial())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
					using (var dresp = await client.GetAsync("Hotel/DDHotel/" + CompID, HttpCompletionOption.ResponseHeadersRead))
					{
						var dapiResp = await dresp.Content.ReadAsStreamAsync();
						list = await System.Text.Json.JsonSerializer.DeserializeAsync<List<SelectListItem>>(dapiResp, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
					}
				}

				return list;
			}
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<List<SelectListItem>> DDColorStaysAPI(string URL, string TokenKey)
        {
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                using (HttpClient client = APIColorStays.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenKey);
                    using (var dresp = await client.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var dapiResp = await dresp.Content.ReadAsStreamAsync();
                        list = await System.Text.Json.JsonSerializer.DeserializeAsync<List<SelectListItem>>(dapiResp, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    }
                }
                return list;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
