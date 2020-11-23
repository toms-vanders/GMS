using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMS___Business_Layer;
using GMS___Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GMS___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        public IOptions<ClientSettings> clientSettings;
        private EventProcessor eventProcessor;
        public GuildController(IOptions<ClientSettings> clientSettings)
        {
            this.clientSettings = clientSettings;
            eventProcessor = new EventProcessor();
        }

        [HttpGet("{guildId}")]
        public IEnumerable<Event> GetAll(string guildId) => eventProcessor.GetAllGuildEvents(guildId);

        [HttpGet("events/{eventId}")]
        public IEnumerable<Event> Get(string eventId) => eventProcessor.GetEventByID(Int32.Parse(eventId));

        [HttpGet("{guildId}/type/{type}")]
        public IEnumerable<Event> GetByType(string guildId, string type) => eventProcessor.GetAllGuildEventsByEventType(guildId, type);

        /*[HttpGet("{guildId}/Name/{name}")]
        public IEnumerable<Event> GetByName(string guildId, string name) => eventProcessor.GetAllGuildEventsByName(guildId, name);*/

        [HttpPost("events/insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Event> Post([FromBody] Event e)
        {
            if (eventProcessor.InsertEvent(e.Name, e.EventType, e.Location, e.Date, e.Description, e.MaxNumberOfCharacters, e.GuildID)) 
            {
                return e;
            }
            return BadRequest("Invalid data.");
        }

        [HttpDelete("events/remove/{eventId}")]
        public string DeleteEvent(string eventId) 
        {
            if(eventProcessor.DeleteEventByID(Int32.Parse(eventId)))
            {
                return "Succesfully deleted";
            } else
            {
                return "Not succesfully deleted";
            }
        }

    }
}
