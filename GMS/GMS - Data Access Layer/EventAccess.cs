using Dapper;
using GMS___Model;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GMS___Data_Access_Layer
{
    public class EventAccess : IEventAccess
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public Event GetEventByID(int eventID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving event with ID: @EventID", eventID);
                    
                    if(conn.QueryFirst<Event>("SELECT * FROM Event WHERE eventID = @EventID", new { EventID = eventID }) is Event retrievedEvent)
                    {
                        log.Info("Successfully retrieved event.");
                        return retrievedEvent;

                    } else
                    {
                        log.Error(new Exception(), "Unable to retrieve event from database.");
                        return null;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the event from the database.");
                    log.Error(ex, "Unable to retrieve event from database.");
                    return null;
                }
            }
        }

        public IEnumerable<Event> GetAllGuildEvents(string guildID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving events from databse for guild ID: @GuildID", guildID);
                    IEnumerable<Event> events = conn.Query<Event>("SELECT eventID, guildID, name, description, eventType, location, date, maxNumberOfCharacters, rowId FROM Event WHERE guildID = @GuildID", new { GuildID = guildID }).ToList();

                    if (events.Any())
                    {
                        log.Info("Successfully retrieved @eventCount event(s) for guild ID: ", events.Count(), guildID);
                        return events;
                    } else
                    {
                        log.Error(new Exception(), "Unable to retrieve events from database.");
                        return null;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the events from the database.");
                    log.Error(ex, "Unable to retrieve events from database.");
                    return null;
                }
            }
        }

        public IEnumerable<Event> GetAllGuildEventsByEventType(string guildID, string eventType)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving events from database for guild ID: @GuildID of type :", guildID, eventType);
                    IEnumerable<Event> events = conn.Query<Event>("SELECT * FROM Event WHERE guildID = @GuildID AND eventType = @EventType", new { GuildID = guildID, EventType = eventType }).ToList();
                    if (events.Any())
                    {
                        log.Info("Successfully retrieved @eventCount event(s) of type @eventType for guild ID : ", events.Count(), eventType, guildID);
                        return events;
                    } else
                    {
                        log.Error(new Exception(), "Unable to retrieve events from database");
                        return null;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the events from the database.");
                    log.Error(ex, "Unable to retrieve events from database.");
                    return null;
                }
            }
        }

        public IEnumerable<Event> GetGuildEventsByCharacterName(string guildID, string characterName)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            // Gets events the user signed up for and is either on Participants List OR Waiting List.
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving events for guild ID: @guildID and character: @characterName", guildID, characterName);
                    IEnumerable<Event> foundEvents = conn.Query<Event>("SELECT e.eventID, e.guildID, e.name, e.description, e.eventType, e.location, e.date, e.maxNumberOfCharacters" +
                        " FROM Event e RIGHT JOIN EventCharacter ec on e.eventID = ec.eventID" +
                        " WHERE ec.characterName = @CharacterName and e.guildID = @GuildID", new { GuildID = guildID, CharacterName = characterName }).ToList();

                    log.Info("Retrieving events on waiting list for guild ID: @guildID and character: @characterName", guildID, characterName);
                    IEnumerable<Event> totalEvents = foundEvents.Concat(conn.Query<Event>("SELECT e.eventID, e.guildID, e.name, e.description, e.eventType, e.location, e.date, e.maxNumberOfCharacters" +
                        " FROM Event e RIGHT JOIN EventCharacterWaitingList ecwl on e.eventID = ecwl.eventID" +
                        " WHERE ecwl.characterName = @CharacterName and e.guildID = @GuildID", new { GuildID = guildID, CharacterName = characterName }).ToList()).ToList();

                    if (totalEvents.Any())
                    {
                        log.Info("Successfully retrieved @eventCount event(s) for guild ID: @guildID and character: @characterName", foundEvents, guildID, characterName);
                        log.Info("Successfully retrieved @eventCount event(s) (including waiting list) for guild ID: @guildID and character: @characterName", totalEvents, guildID, characterName);
                        return totalEvents;
                    } else
                    {
                        log.Error(new Exception(), "Unable to retrieve events from database");
                        return null;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the events from the database.");
                    log.Error(ex, "Unable to retrieve events from database.");
                    return null;
                }
            }
        }

        public bool HasEventChangedRowVersion(int eventID, byte[] currentRowId)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving event row version for event: @eventID", eventID);
                    byte[] fetchedRowId = conn.ExecuteScalar<byte[]>("SELECT rowId FROM Event WHERE eventID = @EventID", new { EventID = eventID });

                    if (fetchedRowId.Any())
                    {
                        log.Info("Successfully retrieved row version for event: @eventID", eventID);
                        if (Convert.ToBase64String(fetchedRowId) != Convert.ToBase64String(currentRowId))
                        {
                            return true;
                        } else
                        {
                            return false;
                        }
                    } else
                    {
                        log.Info("Unable to retrieve row version for event: @eventID", eventID);
                        return false;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving event row versioning from the database.");
                    log.Error(ex, "Unable to retrieve event row versioning from database.");
                    return false;
                }
            }
        }

        public bool InsertEvent(Event guildEvent)
        {
            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Inserting event into database for guild ID: @guildID with name: @eventName", guildEvent.GuildID, guildEvent.Name);
                    int rowsAffected = conn.Execute(@"INSERT INTO Event (name, eventType, location, date, description, maxNumberOfCharacters, guildID) " +
                    "VALUES (@Name, @EventType, @Location, @Date, @Description, @maxNumberOfCharacters, @GuildID)", guildEvent);

                    if (rowsAffected > 0)
                    {
                        log.Info("Successfully inserted event: @eventName into the database.", guildEvent.Name);
                        return true;
                    }
                    log.Error(new Exception(), "Unable to insert event into the database.");
                    return false;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while inserting event into the database.");
                    log.Error(ex, "Unable to insert the event into the database.");
                    return false;
                }
            }
        }

        public bool UpdateEvent(Event guildEvent)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Updating event into database for guild ID: @guildID with name: @eventName", guildEvent.GuildID, guildEvent.Name);
                    int rowsAffected = conn.Execute("UPDATE Event SET name = @Name, eventType = @EventType, location = @Location," +
                    " date = @Date, description = @Description, maxNumberOfCharacters = @MaxNumberOfCharacters, guildID = @GuildID WHERE eventID = @EventID" +
                    " AND rowId = @RowId", guildEvent);

                    if (rowsAffected > 0)
                    {
                        log.Info("Successfully updated event: @eventName into the database.", guildEvent.Name);
                        return true;
                    }
                    log.Error(new Exception(), "Unable to update event into the database.");
                    return false;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while updating the event to the database.");
                    log.Error(ex, "Unable to update the event to the database.");
                    return false;
                }
            }
        }

        public bool DeleteEventByID(int eventID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Deleting event: @eventID from database.", eventID);
                    int rowsAffected = conn.Execute(@"DELETE FROM Event WHERE eventID = @EventID", new { EventID = eventID });

                    if (rowsAffected > 0)
                    {
                        log.Info("Successfully deleted event: @eventID from database.");
                        return true;
                    }
                    log.Error(new Exception(), "Unable to delete event from database.");
                    return false;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while deleting the event from the database.");
                    log.Error(ex, "Unable to delete the event from the database.");
                    return false;
                }
            }
        }

        //This method is used by the Tests
        public int GetIdOfEvent(string eventName)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return -1;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving event id from database with event name: @eventName", eventName);
                    List<int> ids = (List<int>)conn.Query<int>(@"Select eventID FROM Event WHERE name = @Name", new { Name = eventName });
                    if (ids.Count() == 0)
                    {
                        log.Error(new Exception(),"Unable to retrieve event from database");
                        return 0;
                    } else
                    {
                        log.Info("Successfully retrieved event ID with event name: @eventName", eventName);
                        return ids[0];
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the event from the database.");
                    log.Error(ex, "Unable to retrieve the event from the database.");
                    return 0;
                }
            }
        }

    }
}
