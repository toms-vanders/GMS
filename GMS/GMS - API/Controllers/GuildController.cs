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
    public class GuildController : ControllerBase {

        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public GuildController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.Value.ApiToken);
            apiURL = _clientSettings.Value.ApiURL + "guild";
        }

        [Route("api/guild/{guildID}")]
        [HttpGet]
        public async Task<string> GetGuild(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/log")]
        [HttpGet]
        public async Task<string> GetGuildLog(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/log");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/members")]
        [HttpGet]
        public async Task<string> GetGuildMembers(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/members");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/ranks")]
        [HttpGet]
        public async Task<string> GetGuildRanks(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/ranks");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/stash")]
        [HttpGet]
        public async Task<string> GetGuildStash(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/stash");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/storage")]
        [HttpGet]
        public async Task<string> GetGuildStorage(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/storage");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/teams")]
        [HttpGet]
        public async Task<string> GetGuildTeams(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/teams");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/treasury")]
        [HttpGet]
        public async Task<string> GetGuildTreasury(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/treasury");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/{guildID}/upgrades")]
        [HttpGet]
        public async Task<string> GetGuildUpgrades(string guildID) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + guildID + "/upgrades");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        //TODO: Complicated one
        [Route("api/guild/permissions")]
        [HttpGet]
        public async Task<string> GetGuildPermissions() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/permissions");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/search/{guildName}")]
        [HttpGet]
        public async Task<string> SearchGuild(string guildName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/search?name=" + guildName);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/guild/upgrades")]
        [HttpGet]
        public async Task<string> GetGuildUpgrades() {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/upgrades");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/emblem")]
        [HttpGet]
        public async Task<string> GetGuildEmblem() {
            HttpResponseMessage response = await client.GetAsync(_clientSettings.Value.ApiURL + "emblem");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/emblem/foregrounds/{emblemID}")]
        [HttpGet]
        public async Task<string> GetGuildEmblemForegrounds(string emblemID) {
            HttpResponseMessage response = await client.GetAsync(_clientSettings.Value.ApiURL + "emblem/foregrounds?ids=" + emblemID);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/emblem/backgrounds/{emblemID}")]
        [HttpGet]
        public async Task<string> GetGuildEmblemBackgrounds(string emblemID) {
            HttpResponseMessage response = await client.GetAsync(_clientSettings.Value.ApiURL + "emblem/backgrounds?ids=" + emblemID);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
