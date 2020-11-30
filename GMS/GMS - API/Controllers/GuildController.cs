using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
        private EventCharacterProcessor eventCharacterProcessor;
        private EventCharacterWaitingListProcessor eventCharacterWaitingListProcessor;
        public GuildController(IOptions<ClientSettings> clientSettings)
        {
            this.clientSettings = clientSettings;
            eventProcessor = new EventProcessor();
            eventCharacterProcessor = new EventCharacterProcessor();
            eventCharacterWaitingListProcessor = new EventCharacterWaitingListProcessor();
        }

        [HttpGet("{guildId}")]
        public IEnumerable<Event> GetAll(string guildId) => eventProcessor.GetAllGuildEvents(guildId);

        [HttpGet("events/{eventId}")]
        public IEnumerable<Event> Get(string eventId) => eventProcessor.GetEventByID(Int32.Parse(eventId));

        [HttpGet("{guildId}/type/{type}")]
        public IEnumerable<Event> GetByType(string guildId, string type) => eventProcessor.GetAllGuildEventsByEventType(guildId, type);

        /*[HttpGet("{guildId}/Name/{name}")]
        public IEnumerable<Event> GetByName(string guildId, string name) => eventProcessor.GetAllGuildEventsByName(guildId, name);*/

        //Get all the events a user participates in (takes guildID and characterName)
        [HttpGet("{guildId}/character/{characterName}")]
        public IEnumerable<Event> GetByCharacterName(string guildID, string characterName) => eventProcessor.GetGuildEventsByCharacterName(guildID, characterName);

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

        [HttpPost("events/join")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EventCharacter> Post([FromBody] EventCharacter ec)
        {
            if (eventCharacterProcessor.JoinEvent(ec.EventID, ec.CharacterName, ec.CharacterRole, ec.SignUpDateTime)) { 
                return ec;
            }
            return BadRequest("Invalid data.");
        }

        [HttpDelete("events/withdraw/")]
        public string DeleteEventCharacter([FromHeader(Name = "x-eventid")] int eventID, [FromHeader(Name = "x-charactername")] string characterName)
        {
            // Check if EventCharacter contains entry with given eventID and characterName,
            // if so: delete EventCharacter from EventCharacter
            // if not: delete EventCharacter from EventCharacterWaitingList instead

            if (eventCharacterProcessor.ContainsEntry(eventID, characterName)) {
                if (eventCharacterProcessor.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName))
                {
                    return "Succesfully deleted from participant's list";
                }
                else
                {
                    return "Not succesfully deleted";
                }
            } else
            {
                if (eventCharacterWaitingListProcessor.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName))
                {
                    return "Succesfully deleted from waiting list";
                } else
                {
                    return "Not succesfully deleted from waiting list";
                }
            }
        }

        [HttpDelete("events/remove/")]
        public string DeleteEvent([FromHeader(Name = "x-eventid")] int eventID, [FromHeader(Name = "x-rowid")] byte[] rowId)
        {

            // Get the Event from database, and compare the rowIds
            // If they are equal, then delete the event
            // If not return bad response (currently string)
            if (eventProcessor.HasEventChangedRowVersion(eventID, rowId))
            {
                return "Not succesfully deleted";
            } else
            {
                if (eventProcessor.DeleteEventByID(eventID))
                {
                    return "Succesfully deleted";
                }
                else
                {
                    return "Not succesfully deleted";
                }
            }

            
        }

    }
}
