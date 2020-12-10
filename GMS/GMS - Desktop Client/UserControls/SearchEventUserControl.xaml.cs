using GMS___Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MessageBoxImage = GMS___Desktop_Client.WpfMessageBox.MsgCl.MessageBoxImage;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for SearchEventUserControl.xaml
    /// </summary>
    public partial class SearchEventUserControl : UserControl
    {
        private readonly HttpClient client;
        private readonly MetroWindow parentWindow = (MetroWindow)App.Current.MainWindow;
        IEnumerable<Event> eventList;
        IEnumerable<Event> joinedEvents;


        public SearchEventUserControl()
        {
            InitializeComponent();

            filterByEventTypeBox.ItemsSource = Enum.GetValues(typeof(EventType.EventTypes)).Cast<EventType.EventTypes>();

            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["AuthToken"]);

            eventSearchBox.IsReadOnly = true;
            filterByEventTypeBox.IsEnabled = false;
            filterByRoleBox.IsEnabled = false;
            eventGrid.Visibility = Visibility.Hidden;
            dataGridMessage.Visibility = Visibility.Visible;
            
            FillDataGrid();

        }

        public async void FillDataGrid()
        {
            if (!string.IsNullOrEmpty((string)App.Current.Properties["CharacterGuildID"]))
            {
                try
                {
                    string responseBody = await client.GetStringAsync("api/Guild/" + App.Current.Properties["CharacterGuildID"]);

                    eventList = JsonConvert.DeserializeObject<IEnumerable<Event>>(responseBody);
                } catch (TimeoutException)
                {
                    await parentWindow.ShowMessageAsync("Service unavailable", "An error occurred while contacting the server please try again later.", MessageDialogStyle.Affirmative);

                } catch (WebException)
                {
                    await parentWindow.ShowMessageAsync("Web service error", "An error occured contacting the web service please try again later.", MessageDialogStyle.Affirmative);
                }

                eventGrid.ItemsSource = eventList;

                eventGrid.Items.Refresh();
                GetJoinedEventsAsync();

                eventSearchBox.IsReadOnly = false;
                filterByEventTypeBox.IsEnabled = true;
                filterByRoleBox.IsEnabled = true;
                eventGrid.Visibility = Visibility.Visible;
                dataGridMessage.Visibility = Visibility.Hidden;
            }
        }

        private void EventSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEvents();
        }

        private void FilterByEventTypeBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            FilterEvents();

        }

        private void FilterEvents()
        {
            if (eventList != null)
            {
                var filterByName = eventList.Where(ev => ev.Name.IndexOf(eventSearchBox.Text, (StringComparison)CompareOptions.IgnoreCase) >= 0);

                var eventTypesSelections = filterByEventTypeBox.SelectedItems;

                if (eventTypesSelections.Count > 0)
                {
                    List<Event> filterByEventType = new List<Event>();
                    foreach (var et in eventTypesSelections)
                    {
                        filterByEventType.AddRange(filterByName.Where(ev => ev.EventType == et.ToString()));
                    }

                    eventGrid.ItemsSource = filterByEventType;
                } else
                {
                    eventGrid.ItemsSource = filterByName;
                }
            } else
            {
                eventGrid.Visibility = Visibility.Hidden;
            }


        }

        /// <summary>
        /// Formats dataset columns and adds button columns to the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.PropertyName == "GuildID" || e.PropertyName == "RowId" || e.PropertyName == "Description" ||
                e.PropertyName == "Participants" || e.PropertyName == "WaitingList" || e.PropertyName == "MaxNumberOfCharacters")
            {
                e.Cancel = true;
            } else if (e.PropertyName == "EventID")
            {
                e.Column.Header = "ID";
            } else if (e.PropertyName == "EventType")
            {
                e.Column.Header = "Event Type";
            }

            eventGrid.Columns[0].DisplayIndex = eventGrid.Columns.Count - 1;
            eventGrid.Columns[1].DisplayIndex = eventGrid.Columns.Count - 1;
            eventGrid.Columns[2].DisplayIndex = eventGrid.Columns.Count - 1;
        }

        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {

            Event selectedEvent = (Event)eventGrid.SelectedItem;

            var response = client.DeleteAsync("api/Guild/events/remove/" + selectedEvent.EventID).Result;
            if (response.IsSuccessStatusCode)
            {
                WpfMessageBox.Show("Successfully deleted", "Event " + selectedEvent.EventID + " deleted!", MessageBoxButton.OK, MessageBoxImage.Information);
                FillDataGrid();
            } else
            {
                WpfMessageBox.Show("Error", "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditEventButton_Click(object sender, RoutedEventArgs e)
        {
            Window editEventWindow = new EditEventWindow(this, SelectedEventID());
            editEventWindow.ShowDialog();
        }

        private void JoinEventButton_Click(object sender, RoutedEventArgs e)
        {

            if (!joinedEvents.Contains(SelectedEventID()))
            {
                int eventID = SelectedEventID().EventID;
                string eventName = SelectedEventID().Name;
                byte[] rowID = SelectedEventID().RowId;

                Window joinEventWindow = new JoinEventWindow(this, eventID, eventName, rowID);
                joinEventWindow.ShowDialog();
            } else
            {
                if (WpfMessageBox.Show("Already participating", "You are already part of this event, do you wish to cancel your participation?",  MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CancelParticipation(SelectedEventID().EventID, (string)App.Current.Properties["SelectedCharacter"]);
                    FillDataGrid();
                }
            }
        }

        private Event SelectedEventID()
        {
            return (Event)eventGrid.SelectedItem;
        }

        private async void GetJoinedEventsAsync()
        {
            var response = await client.GetStringAsync("api/guild/" + App.Current.Properties["CharacterGuildID"] + "/character/" + App.Current.Properties["SelectedCharacter"]);
            joinedEvents = JsonConvert.DeserializeObject<IEnumerable<Event>>(response);
        }

        private async void CancelParticipation(int EventID, string characterName)
        {
            client.DefaultRequestHeaders.Add("x-eventid", "" + EventID);
            client.DefaultRequestHeaders.Add("x-charactername", characterName);
            var response = await client.DeleteAsync("api/guild/events/withdraw");
            if (response.IsSuccessStatusCode)
            {
                WpfMessageBox.Show("Cancelled participation", "You have successfully cancelled your partition in the event", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                WpfMessageBox.Show("Error cancelling participation", "There seems to have been an error cancelling your participation please try again later.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
