using System;
using System.Collections.Generic;
using System.Text;
using GMS___Model;

namespace GMS___Business_Layer
{
    interface UserProcessorIF
    {
        Boolean InsertNewUser(string userName, string email, string password);
        User LogInUser(string emailAddress, string password);
        bool InsertApiKey(string emailAddress, string apiKey);
    }
}
