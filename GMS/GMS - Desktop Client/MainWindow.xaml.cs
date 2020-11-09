using GMS___Business_Layer;
using GMS___Data_Access_Layer;
using GMS___Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
        }

        private async void RequestAccount_Click(object sender, RoutedEventArgs e) {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(requestURL.Text);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            requestResponse.Text = responseBody;
        }
    }
}
