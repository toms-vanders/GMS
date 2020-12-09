using AuthenticationService.Managers;
using GMS___Business_Layer;
using GMS___Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public ActionResult<List<Event>> GetAll(string guildId)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    return eventProcessor.GetAllGuildEvents(guildId).ToList();
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpGet("events/{eventId}")]
        public ActionResult<List<Event>> Get(string eventId)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    return eventProcessor.GetEventByID(Int32.Parse(eventId)).ToList();
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpGet("events/{eventID}/participants")]
        public ActionResult<int> GetNumberOfSignedParticipants(string eventId)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    return eventCharacterProcessor.ParticipantsInEvent(Int32.Parse(eventId));
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpGet("{guildId}/type/{type}")]
        public ActionResult<List<Event>> GetByType(string guildId, string type)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    return eventProcessor.GetAllGuildEventsByEventType(guildId, type).ToList();
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

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
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    if (eventProcessor.InsertEvent(e.Name, e.EventType, e.Location, e.Date, e.Description, e.MaxNumberOfCharacters, e.GuildID))
                    {
                        return e;
                    }
                    return BadRequest("Invalid data");
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpPost("events/update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Event> UpdateEvent([FromBody] Event e)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    if (eventProcessor.UpdateEvent(e.EventID, e.Name, e.EventType, e.Location, e.Date, e.Description, e.MaxNumberOfCharacters, e.GuildID, e.RowId))
                    {
                        return e;
                    }
                    return BadRequest("Invalid data");
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpPost("events/join")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EventCharacter> Post([FromBody] EventCharacter ec, [FromHeader(Name = "x-rowid")] byte[] rowId)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    if (eventProcessor.HasEventChangedRowVersion(ec.EventID, rowId))
                    {
                        // TODO change response type
                        return BadRequest("The information about event you tried to join has changed. Joining event was unsuccessful");
                    }

                    if (eventCharacterProcessor.JoinEvent(ec.EventID, ec.CharacterName, ec.CharacterRole, ec.SignUpDateTime))
                    {
                        return ec;
                    }
                    return BadRequest("Invalid data.");
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpDelete("events/withdraw/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> DeleteEventCharacter([FromHeader(Name = "x-eventid")] int eventID, [FromHeader(Name = "x-charactername")] string characterName)
        {
            // Check if EventCharacter contains entry with given eventID and characterName,
            // if so: delete EventCharacter from EventCharacter
            // if not: delete EventCharacter from EventCharacterWaitingList instead

            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    if (eventCharacterProcessor.ContainsEntry(eventID, characterName))
                    {
                        if (eventCharacterProcessor.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName))
                        {
                            return "Succesfully deleted from participant's list";
                        } else
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
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpDelete("events/remove/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> DeleteEvent([FromHeader(Name = "x-eventid")] int eventID, [FromHeader(Name = "x-rowid")] byte[] rowId)
        {
            // Get the Event from database, and compare the rowIds
            // If they are equal, then delete the event
            // If not return bad response (currently string)

            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    if (eventProcessor.HasEventChangedRowVersion(eventID, rowId))
                    {
                        return "Not succesfully deleted";
                    } else
                    {
                        if (eventProcessor.DeleteEventByID(eventID))
                        {
                            return "Succesfully deleted";
                        } else
                        {
                            return "Not succesfully deleted";
                        }
                    }
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }
    }
}
