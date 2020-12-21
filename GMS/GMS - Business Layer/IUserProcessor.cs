using GMS___Model;

namespace GMS___Business_Layer
{
    interface IUserProcessor
    {
        User GetUserByEmail(string email);
        User InsertNewUser(string userName, string email, string password);
        User GetUserByUsername(string name);
        User LogInUser(string username, string password);
        bool InsertApiKey(string emailAddress, string apiKey);
        bool ChangeUsername(string oldUsername, string newUsername, string password);
        bool ChangeEmail(string username, string newEmailAddress, string password);
        bool ChangePassword(string username, string currentPassword, string newPassword);
        string GetHashedPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
