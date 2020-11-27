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
using GMS___Model;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for JoinEventWindow.xaml
    /// </summary>
    public partial class JoinEventWindow : Window
    {

        private readonly HttpClient client;

        public JoinEventWindow(int eventID, string eventName)
        {
            InitializeComponent();

            eventIDBox.Text = eventID.ToString();
            eventNameBox.Text = eventName;

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/");
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

                }
                else
                {
                    MessageBox.Show("Error Code" +
                    response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            else
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
