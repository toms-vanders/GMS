using GMS___Data_Access_Layer;
using GMS___Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace GMS___Desktop_Client {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserAccess dataAccessLayer = new UserAccess();
            List<User> users = dataAccessLayer.GetUsersFromDatabase().ToList();
            userName.Content = users[0].UserName;

        }

        private async void requestAccount_Click(object sender, RoutedEventArgs e) {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(requestURL.Text);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            requestResponse.Text = responseBody;
        }
    }
}
