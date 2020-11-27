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
using GMS___Model;
using Newtonsoft.Json;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for CharacterSelectorControl.xaml
    /// </summary>
    public partial class CharacterSelectorControl : UserControl
    {
        private readonly HttpClient client;

        public CharacterSelectorControl()
        {
            InitializeComponent();

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/");

            LoadCharactersInComboBox();
            GetSelectedCharacterInfo();

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


            GetSelectedCharacterInfo();
        }

        private async void GetSelectedCharacterInfo()
        {
            try
            {
                using (client)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.Current.Properties["ApiKey"]);
                    var response = await client.GetStringAsync("gw2api/characters/" + characterSelectionBox.SelectedItem + "/core");
                    Character returnedCharacter = JsonConvert.DeserializeObject<Character>(response);
                    App.Current.Properties["CharacterGuildID"] = returnedCharacter.Guild;
                    App.Current.Properties["CharacterName"] = returnedCharacter.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
