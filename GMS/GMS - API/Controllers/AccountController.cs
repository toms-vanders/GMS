using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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

            var request = WebRequest.Create(_clientSettings.Value.ApiURL + "account") as HttpWebRequest;
            request.Accept = "application/json";
            request.Headers["Authorization"] = "Bearer " + _clientSettings.Value.ApiToken;
            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            var responseStream = responseObject.GetResponseStream();
            using var streamReader = new StreamReader(responseObject.GetResponseStream());

            return streamReader.ReadToEnd();
        }

    }
}
