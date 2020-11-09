using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GMS___Desktop_Client
{
    /// <summary>
    /// Interaction logic for LogInScreen.xaml
    /// </summary>
    public partial class LogInScreen : Window
    {
        UserProcessor userProcessor;

        public LogInScreen()
        {
            InitializeComponent();
            userProcessor = new UserProcessor();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            string email = userEmailText.Text;
            string password = passwordText.Password;


            User user = userProcessor.LogInUser(email, password);


            if (user != null)
            {
                MessageBox.Show("Login Succesful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Username/email or Password is Incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }

        private void Hyperlink_ForgotPassword(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void Hyperlink_CreateAccount(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
