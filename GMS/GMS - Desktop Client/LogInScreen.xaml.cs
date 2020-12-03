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
            User user = new User() { UserName = userEmailText.Text, Password = passwordText.Password, EmailAddress = ""};

            var login = client.PostAsJsonAsync("api/user/login", user).Result;

            if (login.StatusCode == HttpStatusCode.OK)
            {
                string authToken = login.Content.ReadAsStringAsync().Result;
                client.DefaultRequestHeaders.Add("Authorization", authToken);
                var userInfo = client.GetAsync("api/user").Result;
                User returnUser = JsonConvert.DeserializeObject<User>(userInfo.Content.ReadAsStringAsync().Result);
                App.Current.Properties["AuthToken"] = authToken;
                //TODO if apikey null

                if (returnUser.ApiKey == "")
                {
                    MessageBox.Show("Account does not have an API Key", "Characters", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Set neeed properties for the scope of the application
                SetAppProperties(returnUser);

                //Get User Characters
                GetCharacters(returnUser);
                GetDefaultCharacter();



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

        private void GetCharacters(User returnUser)
        {
            try
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
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GetDefaultCharacter()
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.Current.Properties["ApiKey"]);
                var json = client.GetStringAsync("gw2api/characters/" + ((ArrayList)App.Current.Properties["Characters"])[0].ToString() + "/core").Result;
                var chara = JsonConvert.DeserializeObject<Character>(json);
                App.Current.Properties["CharacterGuildID"] = chara.Guild;
                App.Current.Properties["SelectedCharacter"] = chara.Name;
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
        }
    }
}
