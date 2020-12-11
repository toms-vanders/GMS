using GMS___Desktop_Client.UserControls;
using GMS___Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using MessageBoxImage = GMS___Desktop_Client.WpfMessageBox.MsgCl.MessageBoxImage;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for JoinEventWindow.xaml
    /// </summary>
    public partial class JoinEventWindow : MetroWindow
    {

        private readonly HttpClient client;
        private readonly SearchEventUserControl DataGrid;

        public JoinEventWindow(SearchEventUserControl dataGrid, int eventID, string eventName, byte[] rowID)
        {
            InitializeComponent();

            DataGrid = dataGrid;
            eventIDBox.Text = eventID.ToString();
            eventNameBox.Text = eventName;
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["AuthToken"]);
            client.DefaultRequestHeaders.Add("x-rowid", JsonSerializer.Serialize(rowID));
        }

        private void JoinEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(characterRoleBox.Text))
            {
                EventCharacter newEventCharacter = new EventCharacter()
                {
                    EventID = int.Parse(eventIDBox.Text),
                    CharacterName = (string)App.Current.Properties["SelectedCharacter"],
                    CharacterRole = characterRoleBox.Text,
                    SignUpDateTime = DateTime.Now,

                };

                var response = client.PostAsJsonAsync("api/Guild/events/join", newEventCharacter).Result;

                if (response.IsSuccessStatusCode)
                {
                    WpfMessageBox.Show("Successfully joined event", "You have successfully joined this event!", MessageBoxButton.OK, MessageBoxImage.Information);

                } else
                {
                    WpfMessageBox.Show("Something went wrong", "An error has occured while joining the event\n Error code : " + response.ReasonPhrase, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else
            {
                WpfMessageBox.Show("No role specified", "Please fill in your role for this event!", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            DataGrid.FillDataGrid();
            Close();
        }

        private void CloseJoinEventButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
