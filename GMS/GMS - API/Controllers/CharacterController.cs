using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers {
    [Route("api/characters")]
    [ApiController]
    public class CharacterController : ControllerBase {

        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public CharacterController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.Value.ApiToken);
            apiURL = _clientSettings.Value.ApiURL + "characters/";
        }

    [HttpGet]
        public async Task<string> GetCharacters() {
            HttpResponseMessage response = await client.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
