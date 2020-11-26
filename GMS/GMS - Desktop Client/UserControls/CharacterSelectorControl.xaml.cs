using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Newtonsoft.Json;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for CharacterSelectorControl.xaml
    /// </summary>
    public partial class CharacterSelectorControl : UserControl
    {

        public CharacterSelectorControl()
        {
            InitializeComponent();

            LoadCharactersInComboBox();
        }

        private void LoadCharactersInComboBox()
        {
            
            ArrayList characters = (ArrayList)App.Current.Properties["Characters"];
            foreach (var item in characters)
            {
                characterSelectionBox.Items.Add(item);
            }

        }

        private void characterSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Current.Properties["SelectedCharacter"] = characterSelectionBox.SelectedItem;
        }
    }
}
