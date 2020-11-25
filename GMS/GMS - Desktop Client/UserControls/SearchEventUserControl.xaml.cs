using System;
using System.Collections.Generic;
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

            FillDataGrid();

        }
        
        private async void FillDataGrid()
        {

            var uri = $"https://localhost:44377/api/Guild/116E0C0E-0035-44A9-BB22-4AE3E23127E5";

            string responseBody = await client.GetStringAsync(uri);

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
            var uri = $"https://localhost:44377/api/Guild/events/remove/";

            Event selectedEvent = (Event)eventGrid.SelectedItem;

            var response = client.DeleteAsync(uri + selectedEvent.EventID).Result;
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

        }
    }
}
