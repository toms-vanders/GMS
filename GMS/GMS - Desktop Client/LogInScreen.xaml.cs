using GMS___Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Navigation;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for LogInScreen.xaml
    /// </summary>
    public partial class LogInScreen : Window
    {
        private readonly HttpClient client;

        public LogInScreen()
        {
            InitializeComponent();
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/");
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {

            User user = new User() { EmailAddress = userEmailText.Text, Password = passwordText.Password };

            var login = client.PostAsJsonAsync("api/user/login", user).Result;

            if (login.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = login.Content.ReadAsStringAsync().Result;
                User returnUser = JsonConvert.DeserializeObject<User>(responseContent);
                //TODO if apikey null

                if (returnUser.ApiKey == "")
                {
                    MessageBox.Show("Account does not have an API Key", "Characters", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Set neeed properties for the scope of the application
                SetAppProperties(returnUser);

                //Get User Characters
                GetCharacters(returnUser);



                MessageBox.Show("Login Succesful!\n Welcome " + returnUser.UserName, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window MainWindow = new MainWindow();
                MainWindow.Show();
                Close();
            } else
            {
                MessageBox.Show("Username/email or Password is Incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }

        private void Hyperlink_ForgotPassword(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void Hyperlink_CreateAccount(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        //Might be redundant now
        //static void UpdateAppSettings(string key, string value) 
        //{
        //    try 
        //    {
        //        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //        var settings = configFile.AppSettings.Settings;
        //        if (settings[key] == null) 
        //        {
        //            settings.Add(key, value);
        //        }
        //        else 
        //        {
        //            settings[key].Value = value;
        //        }
        //        configFile.Save(ConfigurationSaveMode.Modified);
        //        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        //    } 
        //    catch (ConfigurationErrorsException) 
        //    {
        //        Console.WriteLine("Error writing app settings");
        //    }
        //}
        private void GetCharacters(User returnUser)
        {
            try
            {
                using (client)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.Current.Properties["ApiKey"]);
                    var json = client.GetStringAsync("gw2api/characters").Result;
                    ArrayList characters = JsonConvert.DeserializeObject<ArrayList>(json);
                    string chars = "";
                    foreach (var item in characters)
                    {
                        chars = chars + " " + item;
                    }
                    returnUser.Characters = characters;
                    App.Current.Properties["Characters"] = characters;
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetAppProperties(User returnedUser)
        {
            App.Current.Properties["ApiKey"] = returnedUser.ApiKey;
            App.Current.Properties["UserName"] = returnedUser.UserName;
            App.Current.Properties["Characters"] = returnedUser.Characters;
            //App.Current.Properties["GuildID"] = "821239B1-FE78-3742-83A4-75152E1ED7A96C18AA79-9093-490B-8B9A-F2AA6C8DAB8E";
            App.Current.Properties["SelectedCharacter"] = "";


        }
    }
}
