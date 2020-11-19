using System;
using System.Collections.Generic;
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

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for SearchEventUserControl.xaml
    /// </summary>
    public partial class SearchEventUserControl : UserControl
    {
        public SearchEventUserControl()
        {
            InitializeComponent();

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            EventProcessor ep = new EventProcessor();
            this.eventGrid.ItemsSource = ep.GetAllGuildEvents("116E0C0E-0035-44A9-BB22-4AE3E23127E5");
        }
    }
}
