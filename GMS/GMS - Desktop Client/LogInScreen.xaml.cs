using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMS___Business_Layer;
using GMS___Model;
using Newtonsoft.Json;

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
        }

        private async void logInButton_Click(object sender, RoutedEventArgs e)
        {
            //string email = userEmailText.Text;
            //string password = passwordText.Password;


            //User user = userProcessor.LogInUser(email, password);

            var uri = $"https://localhost:44377/api/user/login";

            User user = new User() { EmailAddress = userEmailText.Text, Password = passwordText.Password };

            var login = await client.PostAsJsonAsync(uri, user);

            //string json = JsonConvert.SerializeObject(user);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            if (login.StatusCode == HttpStatusCode.OK)
            {                
                var responseContent = await login.Content.ReadAsStringAsync();
                User returnUser = JsonConvert.DeserializeObject<User>(responseContent);
                MessageBox.Show("Login Succesful!\n Welcome " + returnUser.UserName, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window MainWindow = new MainWindow();
                MainWindow.Show();
                Close();
            }
            else
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
    }
}
