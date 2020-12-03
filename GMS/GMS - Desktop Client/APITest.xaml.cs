using System.Net.Http;
using System.Windows;

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
