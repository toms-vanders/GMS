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
        private readonly HttpClient client;

        public CharacterSelectorControl()
        {
            InitializeComponent();

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/");

            LoadCharactersInComboBox();
        }

        private void LoadCharactersInComboBox()
        {
            try
            {
                using (client)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ConfigurationManager.AppSettings["ApiToken"]);
                    var json = client.GetStringAsync("gw2api/characters").Result;
                    ArrayList characters = JsonConvert.DeserializeObject<ArrayList>(json);
                    foreach (var item in characters)
                    {
                        characterSelectionBox.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
