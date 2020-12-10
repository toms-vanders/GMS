using GMS___Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for GuildInformationControl.xaml
    /// </summary>
    public partial class GuildInformationControl : UserControl
    {
        private readonly HttpClient client;

        public GuildInformationControl()
        {
            InitializeComponent();
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["ApiKey"]);
            FillGuildInfo();
        }

        public async void FillGuildInfo()
        {
            if (!string.IsNullOrEmpty((string)App.Current.Properties["CharacterGuildID"]))
            {
                HttpResponseMessage responseBody = await client.GetAsync("gw2api/guild/" + App.Current.Properties["CharacterGuildID"]);
                HttpResponseMessage newResponse = responseBody;
                newResponse.Content.Headers.ContentType =  new MediaTypeHeaderValue("application/json");
                newResponse.Content.Headers.ContentEncoding.Add("gzip");
                newResponse.Content.Headers.ContentType.CharSet = "utf-8";
                string jsonResponse = await newResponse.Content.ReadAsStringAsync();

                Guild guild = JsonConvert.DeserializeObject<Guild>(jsonResponse);

                guildLvl.Text = guild.Level.ToString();
                guildInfluence.Text = guild.Influence.ToString();
                guildAetherium.Text = guild.Aetherium.ToString();
                guildResonance.Text = guild.Resonance.ToString();
                guildFavor.Text = guild.Favor.ToString();
                guildMemberCount.Text = guild.MemberCount.ToString();
                guildMemberCapcity.Text = guild.MemberCapacity.ToString();
                guildName.Text = guild.Name.ToString();
                guildTag.Text = guild.Tag.ToString();
            }
        }
    }
}
