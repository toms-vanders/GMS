using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GMS___Data_Access_Layer
{
    class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }
    }
}
