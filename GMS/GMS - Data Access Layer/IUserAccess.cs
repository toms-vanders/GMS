using GMS___Model;
using System;
using System.Collections.Generic;

namespace GMS___Data_Access_Layer
{
    interface IUserAccess
    {
        IEnumerable<User> GetUsersFromDatabase();
        User GetUserFromDatabase(String emailAddress);
        User GetUserFromDatabaseWithUsername(string username);
        int InsertUser(User user);
        int UpdateUser(User user);
        int DeleteByName(string UserName);

    }
}
