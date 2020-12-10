using GMS___Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Windows.Navigation;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for LogInScreen.xaml
    /// </summary>
    public partial class LogInScreen : MetroWindow
    {
        private readonly HttpClient client;

        public LogInScreen()
        {
            InitializeComponent();
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            var path = Environment.CurrentDirectory;
            var uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\bg.png", UriKind.RelativeOrAbsolute);
            this.Resources["BackgroundPath"] = new BitmapImage(uriPath);

            //icon
            var iconUriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\icon.ico", UriKind.RelativeOrAbsolute);
            this.Resources["LogoPath"] = new BitmapImage(iconUriPath);
        }

        private async void logInButton_Click(object sender, RoutedEventArgs e)
        {
            var controller = await this.ShowProgressAsync("Please wait...", "Logging in...", false);
            var userName = userEmailText.Text;
            var password = passwordText.Password;
            var emailAddress = "";

            var loggingResult = false;

            await Task.Run(() =>
            {
                controller.SetProgress(0.0);
                controller.SetTitle("Logging in");

                User user = new User() { UserName = userName, Password = password, EmailAddress = emailAddress };

                controller.SetMessage("Sending login information.");
                var login = client.PostAsJsonAsync("api/user/login", user).Result;
                controller.SetProgress(0.2);

                if (login.StatusCode == HttpStatusCode.OK)
                {
                    controller.SetMessage("Retrieving authorization token.");
                    string authToken = login.Content.ReadAsStringAsync().Result;
                    controller.SetProgress(0.3);
                    client.DefaultRequestHeaders.Add("Authorization", authToken);
                    controller.SetMessage("Retrieving user information.");
                    var userInfo = client.GetAsync("api/user").Result;
                    controller.SetProgress(0.4);
                    controller.SetMessage("Creating user object.");
                    User returnUser = JsonConvert.DeserializeObject<User>(userInfo.Content.ReadAsStringAsync().Result);
                    controller.SetProgress(0.5);
                    controller.SetMessage("Saving authentication token.");
                    App.Current.Properties["AuthToken"] = authToken;
                    controller.SetProgress(0.6);
                    //TODO if apikey null

                    if (returnUser.ApiKey == "")
                    {
                        MessageBox.Show("Account does not have an API Key", "Characters", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    controller.SetMessage("Saving application properties.");
                    // Set neeed properties for the scope of the application
                    SetAppProperties(returnUser);
                    controller.SetProgress(0.7);

                    //Get User Characters
                    controller.SetMessage("Retrieving GW2 characters info.");
                    GetCharacters(returnUser);
                    GetDefaultCharacter();
                    controller.SetProgress(1.0);
                    loggingResult = true;
                }
            });

            await controller.CloseAsync();

            if (loggingResult == true)
            {
                var windowLocation = this.PointToScreen(new Point(0, 0));
                Window MainWindow = new MainWindow
                {
                    Left = windowLocation.X,
                    Top = windowLocation.Y
                };
                MainWindow.Show();
                Close();
            }
            else
            {
                await this.ShowMessageAsync("Logging in", "Logging in failed.");
                IncorrectCredentialsTextBlock.Text = "Wrong username or password. Try again.";
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
                App.Current.Properties["SelectedCharacterObject"] = chara;
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
