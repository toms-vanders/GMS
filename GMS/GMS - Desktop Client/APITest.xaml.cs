using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for APITest.xaml
    /// </summary>
    public partial class APITest : Window
    {
        public APITest()
        {
            InitializeComponent();
        }

        private async void RequestAccount_Click(object sender, RoutedEventArgs e)
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(requestURL.Text);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            requestResponse.Text = responseBody;
        }
    }
}
