using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace GMS___Data_Access_Layer
{
    public class EventCharacterWaitingListAccess
    {
        IDbConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {
            using (IDbConnection conn = GetConnection())
            {
                int rowsAffected = conn.Execute(@"DELETE FROM EventCharacterWaitingList WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });

                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
