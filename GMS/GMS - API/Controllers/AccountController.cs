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
    [ApiController]
    public class AccountController : ControllerBase {
        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public AccountController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.Value.ApiToken);
            apiURL = _clientSettings.Value.ApiURL + "account";
        }

        [Route("api/account")]
        [HttpGet]
        public async Task<string> GetAllAccountInformation() {
            HttpResponseMessage response = await client.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        [Route("api/account/achievements")]
        [HttpGet]
        public async Task<string> GetAchievements() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/achievements");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/bank")]
        [HttpGet]
        public async Task<string> GetBankInformation() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/bank");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/dailycrafting")]
        [HttpGet]
        public async Task<string> GetDailyCrafting() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/dailycrafting");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/dungeons")]
        [HttpGet]
        public async Task<string> GetDailyClearedDungeons() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/dungeons");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/dyes")]
        [HttpGet]
        public async Task<string> GetUnlockedDyes() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/dyes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/finishers")]
        [HttpGet]
        public async Task<string> GetUnlockedFinishers() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/finishers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/gliders")]
        [HttpGet]
        public async Task<string> GetUnlockedGliders() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/gliders");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/home/cats")]
        [HttpGet]
        public async Task<string> GetUnlockedHomeCats() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/home/cats");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/home/nodes")]
        [HttpGet]
        public async Task<string> GetUnlockedHomeNodes() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/home/nodes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/inventory")]
        [HttpGet]
        public async Task<string> GetSharedInventory() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/inventory");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/luck")]
        [HttpGet]
        public async Task<string> GetLuckInformation() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/luck");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/mailcarriers")]
        [HttpGet]
        public async Task<string> GetMailCarriers() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/mailcarriers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/mapchests")]
        [HttpGet]
        public async Task<string> GetMapChests() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/mapchests");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/masteries")]
        [HttpGet]
        public async Task<string> GetMasteries() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/masteries");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/mastery/points")]
        [HttpGet]
        public async Task<string> GetMasteryPoints() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/mastery/points");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/materials")]
        [HttpGet]
        public async Task<string> GetMaterialsStorage() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/materials");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/minis")]
        [HttpGet]
        public async Task<string> GetUnlockedMiniatures() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/minis");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/skins")]
        [HttpGet]
        public async Task<string> GetUnlockedMountSkins() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/mounts/skins");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/mounts/types")]
        [HttpGet]
        public async Task<string> GetUnlockedMountTypes() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/mounts/types");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/novelties")]
        [HttpGet]
        public async Task<string> GetUnlockedNovelties() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/novelties");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/outfits")]
        [HttpGet]
        public async Task<string> GetUnlockedOutfits() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/outfits");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/pvp/heroes")]
        [HttpGet]
        public async Task<string> GetUnlockedPVPHeroes() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/pvp/heroes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/raids")]
        [HttpGet]
        public async Task<string> GetCompletedWeeklyRaids() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/raids");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/recipes")]
        [HttpGet]
        public async Task<string> GetUnlockedRecipes() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/recipes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/skins")]
        [HttpGet]
        public async Task<string> GetUnlockedSkins() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/skins");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/titles")]
        [HttpGet]
        public async Task<string> GetUnlockedTitles() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/titles");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/wallet")]
        [HttpGet]
        public async Task<string> GetWalletInformation() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/wallet");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/account/worldbosses")]
        [HttpGet]
        public async Task<string> GetWorldBossClears() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/worldbosses");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
