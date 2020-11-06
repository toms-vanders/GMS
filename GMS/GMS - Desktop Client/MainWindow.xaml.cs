using GMS___Data_Access_Layer;
using GMS___Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        private void requestAccount_Click(object sender, RoutedEventArgs e) {

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestURL.Text);
            httpRequest.Accept = "application/json";
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            string response = streamReader.ReadToEnd();
            requestResponse.Text = response;
        }
    }
}
