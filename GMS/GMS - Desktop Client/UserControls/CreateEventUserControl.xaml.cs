﻿using GMS___Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

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
            Event newEvent = new Event()
            {
                Name = eventName.Text,
                EventType = eventType.Text,
                Location = eventLocation.Text,
                Date = eventDate.DisplayDate,
                Description = eventDescription.Text,
                MaxNumberOfCharacters = (int)eventMaxPlayers.Value,
                GuildID = (string)App.Current.Properties["CharacterGuildID"]
            };

            var response = await client.PostAsJsonAsync("api/Guild/events/insert", newEvent);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Event added");
                ClearCreateEventsForm();

            } else
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
            eventDate.DisplayDate = new DateTime();
            eventDescription.Text = string.Empty;
            eventMaxPlayers.Value = null;
        }

    }
}
