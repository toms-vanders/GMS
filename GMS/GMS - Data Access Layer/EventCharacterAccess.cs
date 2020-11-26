using Dapper;
using GMS___Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
                int rowsAffected = conn.Execute(@"DELETE FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });

                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
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
    }
}
