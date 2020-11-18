using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using GMS___Model;

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
                IEnumerable<Event> events = conn.Query<Event>("SELECT eventID, guildID, name, description, eventType, location, date, maxNumberOfCharacters FROM Event WHERE guildID = @GuildID", new { GuildID = guildID }).ToList();
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
                    " date = @Date, maxNumberOfCharacters = @MaxNumberOfCharacters, guildID = @GuildID WHERE eventID = @EventID", guildEvent);

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

    }
}
