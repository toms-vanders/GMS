using System;
using System.Collections.Generic;
using System.Linq;
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
using GMS___Model;
using Newtonsoft.Json;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for CreateEventUserControl.xaml
    /// </summary>
    public partial class CreateEventUserControl : UserControl
    {
        private readonly HttpClient client;

        public CreateEventUserControl()
        {
            InitializeComponent();

            eventType.ItemsSource = Enum.GetValues(typeof(EventType.EventTypes)).Cast<EventType.EventTypes>();


            client = new HttpClient();

        }

        private async void createEventButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = $"https://localhost:44377/api/Guild";

            Event newEvent = new Event()
            {
                Name = eventName.Text,
                EventType = eventType.Text,
                Location = eventLocation.Text,
                Date = (DateTime)eventDate.Value,
                Description = eventDescription.Text,
                MaxNumberOfCharacters = (int)eventMaxPlayers.Value,
                GuildID = "116E0C0E-0035-44A9-BB22-4AE3E23127E5"
            };
           
            var response = await client.PostAsJsonAsync(uri, newEvent);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Event added");
                ClearCreateEventsForm();

            }
            else
            {
                MessageBox.Show("Error Code" +
                response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }



        private void closeEventFormButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCreateEventsForm();
        }        
        
        private void ClearCreateEventsForm()
        {
            eventName.Text = string.Empty;
            eventType.SelectedIndex = -1;
            eventLocation.Text = string.Empty;
            eventDate.Value = null;
            eventDescription.Text = string.Empty;
            eventMaxPlayers.Value = null;
        }
    }
}
