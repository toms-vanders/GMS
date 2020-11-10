using System;
using System.Collections.Generic;
using System.Text;
using GMS___Model;

namespace GMS___Data_Access_Layer
{
    interface UserAccessIF
    {
        IEnumerable<User> GetUsersFromDatabase();
        User GetUserFromDatabase(String emailAddress);
        int InsertUser(User user);
        int UpdateUser(User user);
        int DeleteByName(string UserName);

    }
}
