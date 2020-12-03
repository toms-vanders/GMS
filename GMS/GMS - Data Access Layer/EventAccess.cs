using Dapper;
using GMS___Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GMS___Data_Access_Layer
{
    public class EventAccess : IEventAccess
    {
        IDbConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        public IEnumerable<Event> GetEventByID(int eventID)
        {
            using (IDbConnection conn = GetConnection())
            {
                IEnumerable<Event> events = conn.Query<Event>("SELECT * FROM Event WHERE eventID = @EventID", new { EventID = eventID }).ToList();
                return events;
            }
        }

        public IEnumerable<Event> GetAllGuildEvents(string guildID)
        {
            using (IDbConnection conn = GetConnection())
            {
                IEnumerable<Event> events = conn.Query<Event>("SELECT eventID, guildID, name, description, eventType, location, date, maxNumberOfCharacters, rowId FROM Event WHERE guildID = @GuildID", new { GuildID = guildID }).ToList();
                return events;
            }
        }

        public IEnumerable<Event> GetAllGuildEventsByEventType(string guildID, string eventType)
        {
            using (IDbConnection conn = GetConnection())
            {
                IEnumerable<Event> events = conn.Query<Event>("SELECT * FROM Event WHERE guildID = @GuildID AND eventType = @EventType",
                    new { GuildID = guildID, EventType = eventType }).ToList();
                return events;
            }
        }

        public IEnumerable<Event> GetGuildEventsByCharacterName(string guildID, string characterName)
        {
            // Gets events the user signed up for and is either on Participants List OR Waiting List.
            using (IDbConnection conn = GetConnection())
            {
                IEnumerable<Event> foundEvents = conn.Query<Event>("select e.eventID, e.guildID, e.name, e.description, e.eventType, e.location, e.date, e.maxNumberOfCharacters from Event e right join EventCharacter ec on e.eventID = ec.eventID where ec.characterName = @CharacterName and e.guildID = @GuildID", new { GuildID = guildID, CharacterName = characterName }).ToList();
                return foundEvents.Concat(conn.Query<Event>("select e.eventID, e.guildID, e.name, e.description, e.eventType, e.location, e.date, e.maxNumberOfCharacters from Event e right join EventCharacterWaitingList ecwl on e.eventID = ecwl.eventID where ecwl.characterName = @CharacterName and e.guildID = @GuildID", new { GuildID = guildID, CharacterName = characterName }).ToList()).ToList();   
            }
        }

        public bool HasEventChangedRowVersion(int eventId, byte[] currentRowId)
        {
            using (IDbConnection conn = GetConnection())
            {
                byte[] fetchedRowId = conn.ExecuteScalar<byte[]>("SELECT rowId FROM Event WHERE eventID = @EventID", new { EventID = eventId });
                if (Convert.ToBase64String(fetchedRowId) != Convert.ToBase64String(currentRowId))
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public bool InsertEvent(Event guildEvent)
        {
            using (IDbConnection conn = GetConnection())
            {
                int rowsAffected = conn.Execute(@"INSERT INTO Event (name, eventType, location, date, description, maxNumberOfCharacters, guildID) " +
                    "VALUES (@Name, @EventType, @Location, @Date, @Description, @maxNumberOfCharacters, @GuildID)", guildEvent);

                if (rowsAffected > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool UpdateEvent(Event guildEvent)
        {
            using (IDbConnection conn = GetConnection())
            {
                int rowsAffected = conn.Execute("UPDATE Event SET name = @Name, eventType = @EventType, location = @Location," +
                    " date = @Date, description = @Description, maxNumberOfCharacters = @MaxNumberOfCharacters, guildID = @GuildID WHERE eventID = @EventID" +
                    " AND rowId = @RowId", guildEvent);

                if (rowsAffected > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool DeleteEventByID(int eventID)
        {
            using (IDbConnection conn = GetConnection())
            {
                int rowsAffected = conn.Execute(@"DELETE FROM Event WHERE eventID = @EventID", new { EventID = eventID });

                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //This method is used by the Tests
        public int getIdOfEvent(string name)
        {
            using (IDbConnection conn = GetConnection())
            {
                List<int> ids = (List<int>)conn.Query<int>(@"Select eventID FROM Event WHERE name = @Name", new { Name = name });
                if (ids.Count() == 0)
                {
                    return 0;
                } else
                {
                    return ids[0];
                }
            }
        }

    }
}
