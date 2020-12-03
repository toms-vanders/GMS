using GMS___Model;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for JoinEventWindow.xaml
    /// </summary>
    public partial class JoinEventWindow : Window
    {

        private readonly HttpClient client;

        public JoinEventWindow(int eventID, string eventName, byte[] rowID)
        {
            InitializeComponent();

            eventIDBox.Text = eventID.ToString();
            eventNameBox.Text = eventName;
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization",(string)App.Current.Properties["AuthToken"]);
            client.DefaultRequestHeaders.Add("x-rowid",JsonSerializer.Serialize(rowID));
        }

        private void joinEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(characterRoleBox.Text))
            {
                EventCharacter newEventCharacter = new EventCharacter()
                {
                    EventID = Int32.Parse(eventIDBox.Text),
                    CharacterName = (string)App.Current.Properties["SelectedCharacter"],
                    CharacterRole = characterRoleBox.Text,
                    SignUpDateTime = DateTime.Now,

                };

                var response = client.PostAsJsonAsync("api/Guild/events/join", newEventCharacter).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("You joined the event");
                    Close();

                } else
                {
                    MessageBox.Show("Error Code" +
                    response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            } else
            {
                MessageBox.Show("Please input your role for the event");
            }


        }

        private void closeJoinEventButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
