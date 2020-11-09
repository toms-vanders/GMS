using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers {
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase {
        private IOptions<ClientSettings> _clientSettings;

        public AccountController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
        }

        [HttpGet]
        public async Task<string> GetAllAccountInformation() {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",_clientSettings.Value.ApiToken);
            HttpResponseMessage response = await client.GetAsync(_clientSettings.Value.ApiURL + "account");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

    }
}
