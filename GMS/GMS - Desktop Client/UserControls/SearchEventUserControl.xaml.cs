using GMS___Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for SearchEventUserControl.xaml
    /// </summary>
    public partial class SearchEventUserControl : UserControl
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly HttpClient client;
        IEnumerable<Event> eventList;
        IEnumerable<Event> joinedEvents;


        public SearchEventUserControl()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

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
                string responseBody = await client.GetStringAsync("api/Guild/" + App.Current.Properties["CharacterGuildID"]);

                eventList = JsonConvert.DeserializeObject<IEnumerable<Event>>(responseBody);
                eventGrid.ItemsSource = eventList;
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
                }
                else
                {
                    eventGrid.ItemsSource = filterByName;
                }
            }
            else
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
            }
            else if (e.PropertyName == "EventID")
            {
                e.Column.Header = "ID";
            }
            else if (e.PropertyName == "EventType")
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
                MessageBox.Show("Event " + selectedEvent.EventID + " deleted!");
                FillDataGrid();
            }
            else
            {
                MessageBox.Show("Error Code" +
                response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void EditEventButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void JoinEventButton_Click(object sender, RoutedEventArgs e)
        {

            if (!joinedEvents.Contains(SelectedEventID()))
            {
                int eventID = SelectedEventID().EventID;
                string eventName = SelectedEventID().Name;
                byte[] rowID = SelectedEventID().RowId;

                Window joinEventWindow = new JoinEventWindow(eventID, eventName, rowID);
                joinEventWindow.ShowDialog();



                //runs a background worker that loads events
                worker.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show("You are already part of this event, do you wish to cancel your participation?", "Already participating", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    CancelParticipation(SelectedEventID().EventID, (string)App.Current.Properties["SelectedCharacter"]);
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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FillDataGrid();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private async void CancelParticipation(int EventID, string characterName)
        {
            client.DefaultRequestHeaders.Add("x-eventid", "" + EventID);
            client.DefaultRequestHeaders.Add("x-charactername", characterName);
            var response = await client.DeleteAsync("api/guild/events/withdraw");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("You have successfully cancelled your partition in the event", "Cancelled participation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There seems to have been an error cancelling your participation please try again later.", "Error cancelling participation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
