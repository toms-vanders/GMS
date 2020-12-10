using GMS___Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MessageBoxImage = GMS___Desktop_Client.WpfMessageBox.MsgCl.MessageBoxImage;

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

            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["AuthToken"]);

        }

        private async void createEventButton_Click(object sender, RoutedEventArgs e)
        {
            var eName = eventName.Text;
            if (eName.Equals(""))
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            var eEventType = eventType.Text;
            if (eEventType.Equals(""))
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            var eLocation = eventLocation.Text;
            if (eLocation.Equals(""))
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            if (!eventDate.SelectedDateTime.HasValue)
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            var dateTimeNow = DateTime.Now;
            var eDate = (DateTime) eventDate.SelectedDateTime;
            if (eDate == eventDate.DisplayDate 
                || eDate.DayOfYear < dateTimeNow.DayOfYear 
                || (eDate.DayOfYear <= dateTimeNow.DayOfYear && eDate.Hour < dateTimeNow.Hour) 
                || (eDate.DayOfYear <= dateTimeNow.DayOfYear && eDate.Hour <= dateTimeNow.Hour && eDate.Minute < dateTimeNow.Minute) 
                || (eDate.DayOfYear <= dateTimeNow.DayOfYear && eDate.Hour <= dateTimeNow.Hour && eDate.Minute <= dateTimeNow.Minute && eDate.Second < dateTimeNow.Second))
            {
                WpfMessageBox.Show("Error", "Please correct the event date. Event date cannot be from the past", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            var eDescription = eventDescription.Text;
            if (eDescription.Equals(""))
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            if (!eventMaxPlayers.Value.HasValue)
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            var eMaxNumberOfCharacters = (int)eventMaxPlayers.Value;
            if (eMaxNumberOfCharacters <= 0)
            {
                WpfMessageBox.Show("Error", "Max number of character must be higher than 0", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
            var eGuildID = (string)App.Current.Properties["CharacterGuildID"];
            if (eGuildID is null)
            {
                WpfMessageBox.Show("Error", "Please fill all the necessary fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            Event newEvent = new Event()
            {
                Name = eName,
                EventType = eEventType,
                Location = eLocation,
                Date = eDate,
                Description = eDescription,
                MaxNumberOfCharacters = eMaxNumberOfCharacters,
                GuildID = eGuildID
            };

            var response = await client.PostAsJsonAsync("api/Guild/events/insert", newEvent);

            if (response.IsSuccessStatusCode)
            {
                WpfMessageBox.Show("Event added", "Successfully added new event!", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearCreateEventsForm();

            } else
            {
                WpfMessageBox.Show("Error", "Error Code" +
                response.StatusCode + " : Message - " + response.ReasonPhrase, MessageBoxButton.OK, MessageBoxImage.Error);
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
            eventDate.DisplayDate = new DateTime();
            eventDescription.Text = string.Empty;
            eventMaxPlayers.Value = null;
        }

    }
}
