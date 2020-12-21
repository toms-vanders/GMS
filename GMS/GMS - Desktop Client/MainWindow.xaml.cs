using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

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

            //icon
            var path = Environment.CurrentDirectory;
            var iconUriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\icon.ico", UriKind.RelativeOrAbsolute);
            this.Resources["MainWindowLogoPath"] = new BitmapImage(iconUriPath);
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
