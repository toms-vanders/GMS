using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers {

    [ApiController]
    public class CharacterController : ControllerBase {

        private IOptions<ClientSettings> _clientSettings;
        private readonly HttpClient client;
        private readonly string apiURL;

        public CharacterController(IOptions<ClientSettings> clientSettings) {
            _clientSettings = clientSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.Value.ApiToken);
            apiURL = _clientSettings.Value.ApiURL + "characters";
        }

        [Route("api/characters")]
        [HttpGet]
        public async Task<string> GetCharacters() {
            HttpResponseMessage response = await client.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}")]
        [HttpGet]
        public async Task<string> GetSpecificCharacter(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/backstory")]
        [HttpGet]
        public async Task<string> GetCharacterBackstory(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/backstory");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/core")]
        [HttpGet]
        public async Task<string> GetCharacterCore(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/core");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/crafting")]
        [HttpGet]
        public async Task<string> GetCharacterCrafting(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/crafting");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/equipment")]
        [HttpGet]
        public async Task<string> GetCharacterEquipment(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/equipment");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/heropoints")]
        [HttpGet]
        public async Task<string> GetCharacterHeroPoints(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/heropoints");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/inventory")]
        [HttpGet]
        public async Task<string> GetCharacterInventory(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/inventory");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/recipes")]
        [HttpGet]
        public async Task<string> GetCharacterRecipes(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/recipes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/sab")]
        [HttpGet]
        public async Task<string> GetCharacterSAB(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/sab");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/skills")]
        [HttpGet]
        public async Task<string> GetCharacterSkills(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/skills");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/specializations")]
        [HttpGet]
        public async Task<string> GetCharacterSpecializations(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/specializations");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Route("api/characters/{characterName}/training")]
        [HttpGet]
        public async Task<string> GetCharacterTraining(string characterName) {
            HttpResponseMessage response = await client.GetAsync(apiURL + "/" + characterName + "/training");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
