using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GMS___API.Controllers
{
    [ApiController]
    public class ExternalAPIControllerGW2 : ControllerBase
    {

        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public ExternalAPIControllerGW2(IOptions<ClientSettings> clientSettings)
        {
            _clientSettings = clientSettings;
            client = new HttpClient();
            apiURL = _clientSettings.Value.ApiURL;
        }

        [Route("gw2api/{**catchAll}")]
        [HttpGet]
        public async Task<string> GetInformation(string catchAll)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Headers["Authorization"]);
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + catchAll);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
