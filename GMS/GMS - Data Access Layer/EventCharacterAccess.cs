using Dapper;
using GMS___Model;
using System.Data;
using System.Data.SqlClient;

namespace GMS___Data_Access_Layer
{
    public class EventCharacterAccess
    {
        IDbConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        public bool InsertEventCharacter(EventCharacter eventParticipant)
        {

            // Either join participant list or the waiting list by checking if participant list reached the maximum amout of sign-ups.
            using (IDbConnection conn = GetConnection())
            {
                if (!isParticipantListFull(eventParticipant.EventID))
                {
                    int rowsAffected = conn.Execute(@"INSERT INTO EventCharacter (eventID, characterName, characterRole, signUpDateTime) " +
                    "VALUES (@EventID, @CharacterName, @CharacterRole, @SignUpDateTime)", eventParticipant);

                    if (rowsAffected > 0)
                    {
                        return true;
                    }

                    return false;
                } else
                {
                    int rowsAffected = conn.Execute(@"INSERT INTO EventCharacterWaitingList (eventID, characterName, characterRole, signUpDateTime) " +
                    "VALUES (@EventID, @CharacterName, @CharacterRole, @SignUpDateTime)", eventParticipant);

                    if (rowsAffected > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }

        public bool ContainsEntry(int eventId, string characterName)
        {
            using (IDbConnection conn = GetConnection())
            {
                int entries = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventId, CharacterName = characterName });
                return entries == 1;
            }
        }

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {
            using (IDbConnection conn = GetConnection())
            {
                int rowsAffected;

                if (isParticipantListFull(eventID))
                {
                    //rowsAffected = conn.Execute(@"DELETE FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });

                    //if (rowsAffected > 0)
                    //{
                    //    return true;
                    //}
                    //return false;

                    return MoveCharacterFromWaitingListToParticipantList(eventID, characterName);

                }
                else
                {
                    rowsAffected = conn.Execute(@"DELETE FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });
                    
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public bool isParticipantListFull(int eventID)
        {
            using (IDbConnection conn = GetConnection())
            {
                int maxAmount = conn.ExecuteScalar<int>("SELECT maxNumberOfCharacters FROM Event WHERE eventID = " + eventID);
                int signedUpCount = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM EventCharacter WHERE eventID = " + eventID);

                return signedUpCount == maxAmount;
            }
        }

        public bool MoveCharacterFromWaitingListToParticipantList(int eventID, string characterName)
        {
            using (IDbConnection conn = GetConnection())
            {

                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Delete character from Participants list
                        conn.Execute(@"DELETE FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName }, trans);

                        //Select character from waiting list that signed up first
                        IEnumerable<EventCharacter> selectedCharacter = conn.Query<EventCharacter>(@"SELECT TOP 1 * FROM EventCharacterWaitingList WHERE eventID = @EventID ORDER BY signUpDateTime ASC",
                            new { EventID = eventID }, trans);

                        //Delete that character from WaitingList
                        conn.Execute(@"DELETE FROM EventCharacterWaitingList WHERE eventID = @EventID AND characterName = @CharacterName",
                            selectedCharacter, trans);

                        //Put that character in Participants List
                        conn.Execute(@"INSERT INTO EventCharacter (eventID, characterName, characterRole, signUpDateTime) " +
                                    "VALUES (@EventID, @CharacterName, @CharacterRole, @SignUpDateTime)", selectedCharacter, trans);

                        trans.Commit();

                        return true;
                       
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();

                        Console.WriteLine(ex.ToString()); // TODO change exception handling

                        return false;
                    }
                }
            }
        }
    }
}
