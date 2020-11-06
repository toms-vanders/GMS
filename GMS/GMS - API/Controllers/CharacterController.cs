using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers {
    [Route("api/characters")]
    [ApiController]
    public class CharacterController : ControllerBase {

        private IOptions<ClientSettings> _clientSettings;

        public CharacterController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
        }

    [HttpGet]
        public string GetCharacters() {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(_clientSettings.Value.ApiURL+"characters");
            httpRequest.Accept = "application/json";
            httpRequest.Headers["Authorization"] = "Bearer " + _clientSettings.Value.ApiToken;

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            Console.WriteLine(httpResponse.StatusCode);
            return streamReader.ReadToEnd();
        }
    }
}
