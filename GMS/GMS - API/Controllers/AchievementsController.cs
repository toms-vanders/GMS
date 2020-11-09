using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers {
    
    [ApiController]
    public class AchievementsController : ControllerBase {

        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public AchievementsController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.Value.ApiToken);
            apiURL = _clientSettings.Value.ApiURL + "achievements";
        }

        [Route("api/achievements")]
        [HttpGet]
        public async Task<string> GetAllAchievements() {
            HttpResponseMessage response = await client.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        //TODO: GetAchievements from list of ids

        [Route("api/achievements/{achievementID}")]
        [HttpGet]
        public async Task<string> GetAchievement(string achievementID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + achievementID);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/achievements/daily")]
        [HttpGet]
        public async Task<string> GetDailyAchievements() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/daily");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/achievements/daily/tomorrow")]
        [HttpGet]
        public async Task<string> GetTomorrowDailyAchievements() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/daily/tomorrow");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/achievements/groups")]
        [HttpGet]
        public async Task<string> GetAchievementGroups() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/groups");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/achievements/categories")]
        [HttpGet]
        public async Task<string> GetAchievementCategories() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/categories");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
