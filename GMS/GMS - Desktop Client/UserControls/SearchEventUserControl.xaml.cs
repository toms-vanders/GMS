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

        private void searchEventsbutton_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
