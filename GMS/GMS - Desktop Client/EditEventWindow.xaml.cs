using GMS___Desktop_Client.UserControls;
using GMS___Model;
using MahApps.Metro.Controls;
using System;
using System.Net.Http;
using System.Windows;
using MessageBoxImage = GMS___Desktop_Client.WpfMessageBox.MsgCl.MessageBoxImage;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for EditEventWindow.xaml
    /// </summary>
    public partial class EditEventWindow : MetroWindow
    {
        private readonly HttpClient client;
        private readonly Event localEvent;
        private readonly SearchEventUserControl DataGrid;
        public EditEventWindow(SearchEventUserControl dataGrid, Event selectedEvent)
        {
            InitializeComponent();
            DataGrid = dataGrid;
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["AuthToken"]);

            localEvent = selectedEvent;
            eventID.Text = selectedEvent.EventID.ToString();
            eventName.Text = selectedEvent.Name;
            eventType.Text = selectedEvent.EventType;
            eventDateTime.DisplayDate = selectedEvent.Date;
            eventLocation.Text = selectedEvent.Location;
            eventMaxParticipants.Value = selectedEvent.MaxNumberOfCharacters;
            eventDescription.Text = selectedEvent.Description;
        }

        private async void EditEventButton_Click(object sender, RoutedEventArgs e)
        {
            Event updatedEvent = new Event()
            {
                EventID = Int32.Parse(eventID.Text),
                Name = eventName.Text,
                EventType = eventType.Text,
                Location = eventLocation.Text,
                Date = eventDateTime.DisplayDate,
                Description = eventDescription.Text,
                MaxNumberOfCharacters = (int)eventMaxParticipants.Value,
                GuildID = localEvent.GuildID,
                RowId = localEvent.RowId
            };

            var response = await client.PostAsJsonAsync("api/Guild/events/update", updatedEvent);

            if (response.IsSuccessStatusCode)
            {
                DataGrid.FillDataGrid();
                WpfMessageBox.Show("Event updated", "Successfully updated event information.", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                WpfMessageBox.Show("Error", "Error Code" +
                response.StatusCode + " : Message - " + response.ReasonPhrase, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void CancelEventEditFormButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
