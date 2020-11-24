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
            using (IDbConnection conn = GetConnection()) 
            {
                int rowsAffected = conn.Execute(@"INSERT INTO EventCharacter (eventID, characterName, role) " +
                    "VALUES (@EventID, @CharacterName, @Role)", eventParticipant);

                if (rowsAffected > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
