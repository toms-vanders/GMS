using System.Windows;
using MahApps.Metro.Controls;

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Clearing app properties

            App.Current.Properties["ApiKey"] = null;
            App.Current.Properties["UserName"] = null;
            App.Current.Properties["Characters"] = null;
            App.Current.Properties["SelectedCharacter"] = null;

            var windowLocation = this.PointToScreen(new Point(0, 0));
            Window loginWindow = new LogInScreen
            {
                Top = windowLocation.X,
                Left = windowLocation.Y
            };
            loginWindow.Show();
            Close();
        }
    }
}
