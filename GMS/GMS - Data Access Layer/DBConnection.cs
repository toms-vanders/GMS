using System.Data;
using System.Data.SqlClient;

namespace GMS___Data_Access_Layer
{
    class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        public static bool IsConnectionAvailable()
        {
            using (IDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    conn.Close();
                } catch (SqlException ex)
                {
                    return false;
                    throw ex;
                }
            }
            return true;
        }

    }
}
