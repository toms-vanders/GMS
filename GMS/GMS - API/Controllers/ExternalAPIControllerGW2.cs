using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers {
    [ApiController]
    public class ExternalAPIControllerGW2 : ControllerBase {

        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public ExternalAPIControllerGW2(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.Value.ApiToken);
            apiURL = _clientSettings.Value.ApiURL;
        }

        [Route("gw2api/{**catchAll}")]
        [HttpGet]
        public async Task<string> GetInformation(string catchAll) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + catchAll);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
