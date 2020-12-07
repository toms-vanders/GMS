using GMS___Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
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

        private void CharacterSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSelectedCharacterInfo();
            
            

            //GetJoinedEventsAsync();
        }

        private async void GetSelectedCharacterInfo()
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.Current.Properties["ApiKey"]);
                var response = await client.GetStringAsync("gw2api/characters/" + characterSelectionBox.SelectedItem + "/core");
                Character returnedCharacter = JsonConvert.DeserializeObject<Character>(response);
                App.Current.Properties["CharacterGuildID"] = returnedCharacter.Guild;
                App.Current.Properties["CharacterName"] = returnedCharacter.Name;

                SetProfessionIcon(returnedCharacter.Profession);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetProfessionIcon(string charProfession)
        {
            string professionIconPath = "";

            switch(charProfession.ToLower())
            {
                case "guardian":
                    professionIconPath = @"Guardian_icon.png";
                    break;
                case "dragonhunter":
                    professionIconPath = @"Dragonhunter.png";
                    break;
                case "firebrand":
                    professionIconPath = @"Firebrand.png";
                    break;
                case "revenant":
                    professionIconPath = @"Revenant_icon.png";
                    break;
                case "herald":
                    professionIconPath = @"Herald.png";
                    break;
                case "renegade":
                    professionIconPath = @"Renegade.png";
                    break;
                case "warrior":
                    professionIconPath = @"Warrior_icon.png";
                    break;
                case "breserker":
                    professionIconPath = @"Berserker.png";
                    break;
                case "spellbreaker":
                    professionIconPath = @"Spellbreaker.png";
                    break;
                case "engineer":
                    professionIconPath = @"Engineer_icon.png";
                    break;
                case "scrapper":
                    professionIconPath = @"Scrapper.png";
                    break;
                case "holosmith":
                    professionIconPath = @"Holosmith.png";
                    break;
                case "ranger":
                    professionIconPath = @"Ranger_icon.png";
                    break;
                case "druid":
                    professionIconPath = @"Druid.png";
                    break;
                case "soulbeast":
                    professionIconPath = @"Soulbeast.png";
                    break;
                case "thief":
                    professionIconPath = @"Thief_icon.png";
                    break;
                case "daredevil":
                    professionIconPath = @"Daredevil.png";
                    break;
                case "deadeye":
                    professionIconPath = @"Deadeye.png";
                    break;
                case "elementalist":
                    professionIconPath = @"Elementalist_icon.png";
                    break;
                case "tempest":
                    professionIconPath = @"Tempest.png";
                    break;
                case "weaver":
                    professionIconPath = @"Weaver.png";
                    break;
                case "mesmer":
                    professionIconPath = @"Mesmer_icon.png";
                    break;
                case "chronomancer":
                    professionIconPath = @"Chronomancer.png";
                    break;
                case "mirage":
                    professionIconPath = @"Mirage.png";
                    break;
                case "necromancer":
                    professionIconPath = @"Necromancer_icon.png";
                    break;
                case "reaper":
                    professionIconPath = @"Reaper.png";
                    break;
                case "scourge":
                    professionIconPath = @"Scourge.png";
                    break;
            }

            //path URI
            var pathURI = new BitmapImage(new Uri(@"C:\Users\dymus\Developer\githubDamiana\gms\GMS\GMS - Desktop Client\Images\professions\" + professionIconPath, UriKind.Absolute));
            this.Resources["ProfessionIconPath"] = pathURI;
        }
    }
}
