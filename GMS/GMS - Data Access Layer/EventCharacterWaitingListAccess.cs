using Dapper;
using NLog;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GMS___Data_Access_Layer
{
    public class EventCharacterWaitingListAccess
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
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
                    log.Info("Deleting character: @characterName from the waiting list for event ID: @eventID", characterName, eventID);
                    int rowsAffected = conn.Execute(@"DELETE FROM EventCharacterWaitingList WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });
                    log.Info("Successfully deleted character: @characterName from the waiting list for event ID: @eventID", characterName, eventID);
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    return false;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while deleting the character from the event waiting list.");
                    log.Error(ex, "Unable to delete the character from the event waiting list.");
                    return false;
                }
            }
        }

        public bool MovePeopleFromWaitingList(int eventID, int noOfPeople)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                int rowsAffected = conn.Execute(@"BEGIN TRANSACTION; SET TRANSACTION ISOLATION LEVEL SERIALIZABLE; " +
                    "INSERT INTO EventCharacter (eventID, characterName, characterRole, signUpDateTime) " +
                    "SELECT TOP(@NoOfPeople) * FROM EventCharacterWaitingList WHERE eventID = @EventID ORDER BY signUpDateTime ASC; " +
                    "DELETE TOP(@NoOfPeople) FROM EventCharacterWaitingList WHERE eventID = @EventID ORDER BY signUpDateTime ASC; COMMIT;", new { EventID = eventID, NoOfPeople = noOfPeople });
                return rowsAffected == noOfPeople * 2;
            }
        }
        public int WaitingListLength(int eventID)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                int waiting = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM EventCharacterWaitingList WHERE eventID = @EventID", new { EventID = eventID });

                return waiting;
            }
        }
    }
}
