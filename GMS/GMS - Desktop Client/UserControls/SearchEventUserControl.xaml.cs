using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
using GMS___Business_Layer;
using GMS___Model;
using Newtonsoft.Json;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for SearchEventUserControl.xaml
    /// </summary>
    public partial class SearchEventUserControl : UserControl
    {
        private readonly HttpClient client;
        IEnumerable<Event> eventList;


        public SearchEventUserControl()
        {
            InitializeComponent();

            filterByEventTypeBox.ItemsSource = Enum.GetValues(typeof(EventType.EventTypes)).Cast<EventType.EventTypes>();

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/");

            FillDataGrid();

        }
        
        public async void FillDataGrid()
        {

            string responseBody = await client.GetStringAsync("api/Guild/" + ConfigurationManager.AppSettings["ApiToken"]);

            eventList = JsonConvert.DeserializeObject<IEnumerable<Event>>(responseBody);

            this.eventGrid.ItemsSource = eventList;
        }

        private void eventSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEvents();
        }

        private void filterByEventTypeBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            FilterEvents();

        }

        private void FilterEvents()
        {
            
            var filterByName = eventList.Where(ev => ev.Name.IndexOf(eventSearchBox.Text, (StringComparison)CompareOptions.IgnoreCase) >= 0);

            var eventTypesSelections = filterByEventTypeBox.SelectedItems;
            
            if(eventTypesSelections.Count > 0)
            {
                List<Event> filterByEventType = new List<Event>();
                foreach (var et in eventTypesSelections)
                {
                    filterByEventType.AddRange(filterByName.Where(ev => ev.EventType == et.ToString()));
                }

                this.eventGrid.ItemsSource = filterByEventType;
            }
            else
            {
                this.eventGrid.ItemsSource = filterByName;
            }

        }

        /// <summary>
        /// Formats dataset columns and adds button columns to the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.PropertyName == "GuildID" || e.PropertyName == "RowId" ||
                e.PropertyName == "Participants" || e.PropertyName == "WaitingList")
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
            else if (e.PropertyName == "MaxNumberOfCharacters")
            {
                e.Column.Header = "Max num. of Players";
            }

            eventGrid.Columns[0].DisplayIndex = eventGrid.Columns.Count - 1;
            eventGrid.Columns[1].DisplayIndex = eventGrid.Columns.Count - 1;
            eventGrid.Columns[2].DisplayIndex = eventGrid.Columns.Count - 1;

        }

        private void deleteEventButton_Click(object sender, RoutedEventArgs e)
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

        private void editEventButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void joinEventButton_Click(object sender, RoutedEventArgs e)
        {
            //EventCharacter newJoin = new EventCharacter()
            //{
            //    EventID = SelectedEventID().EventID,
            //    CharacterName = CharacterSelectorControl
            //};

            //var response = await client.PostAsJsonAsync(uri, newEvent);
        }

        private Event SelectedEventID()
        {
            return (Event)eventGrid.SelectedItem;
        }
    }
}
