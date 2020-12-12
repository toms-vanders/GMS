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
using MessageBoxImage = GMS___Desktop_Client.WpfMessageBox.MsgCl.MessageBoxImage;

namespace GMS___Desktop_Client.UserControls
{
    /// <summary>
    /// Interaction logic for HeroEquipmentControl.xaml
    /// </summary>
    public partial class HeroEquipmentControl : UserControl
    {
        static string path = Environment.CurrentDirectory;
        private readonly HttpClient client;
        public HeroEquipmentControl()
        {
            InitializeComponent();
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["ApiKey"]);

            if (App.Current.Properties["SelectedCharacter"] is null)
            {
                equipmentTextBlock.Text = "Equipment worn by character";
            }
            else
            {
                equipmentTextBlock.Text = "Equipment worn by character " + App.Current.Properties["SelectedCharacter"];
            }

            FillDefaultEquipmentSlots();
            SetEquipmentSlots();
        }

        //private void InitializeControl()
        //{
            
        //}

        public void FillDefaultEquipmentSlots()
        {
            // Armor stack panel
            var uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Head_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["HeadSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Shoulder_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["ShoulderSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Chest_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["ChestSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Hand_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["HandSlot"] = new BitmapImage(uriPath); 
            
            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Leg_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["LegSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Feet_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["FeetSlot"] = new BitmapImage(uriPath);

            // Weapons stack panel

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Sword_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["SwordSlotA"] = new BitmapImage(uriPath);
            this.Resources["SwordSlotB"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Shield_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["ShieldSlotA"] = new BitmapImage(uriPath);
            this.Resources["ShieldSlotB"] = new BitmapImage(uriPath);

            // Back item and trinket stack panel

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Back_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["BackSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Bear_trinket_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["BearTrinketSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Cube_trinket_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["CubeTrinketSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Amulet_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["AmuletSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Right_ring_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["RightRingSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Left_ring_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["LefttRingSlot"] = new BitmapImage(uriPath);

            // Tools stack panel
            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Foraging_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["ForagingSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Logging_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["LoggingSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Mining_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["MiningSlot"] = new BitmapImage(uriPath);

            // Aquatic stack panel
            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Breathing_apparatus_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["BreathingApparatusSlot"] = new BitmapImage(uriPath);

            uriPath = new Uri(path.Substring(0, path.LastIndexOf("bin")) + @"\Images\equipment_slots\Aquatic_weapon_slot.png", UriKind.RelativeOrAbsolute);
            this.Resources["AquaticWeaponSlotA"] = new BitmapImage(uriPath);
            this.Resources["AquaticWeaponSlotB"] = new BitmapImage(uriPath);
        }

        private async void SetEquipmentSlots()
        {
            if (!string.IsNullOrEmpty((string)App.Current.Properties["SelectedCharacter"]))
            {
                HttpResponseMessage equipmentResponseMessage = await client.GetAsync("gw2api/characters/" + App.Current.Properties["SelectedCharacter"] + "/equipment");
                HttpResponseMessage newResponse = equipmentResponseMessage;
                newResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                newResponse.Content.Headers.ContentEncoding.Add("gzip");
                newResponse.Content.Headers.ContentType.CharSet = "utf-8";
                string jsonEquipmentResponse = await newResponse.Content.ReadAsStringAsync();
                Equipments equipments = JsonConvert.DeserializeObject<Equipments>(jsonEquipmentResponse);
                FillEquipmentSlotsWithItems(equipments);
            }
        }

        private async void FillEquipmentSlotsWithItems(Equipments equipments)
        {
            foreach (var equipmentSlot in equipments.Equipment)
            {
                // Getting item
                string itemResponseBody;
                try
                {
                    itemResponseBody = await client.GetStringAsync("gw2api/items/" + equipmentSlot.Id);
                }
                catch (Exception e)
                {
                    WpfMessageBox.Show("Item could not be displayed", "Error, while trying to get the item with id: " + equipmentSlot.Id, MessageBoxButton.OK, MessageBoxImage.Warning);
                    continue;
                }
                Item item = JsonConvert.DeserializeObject<Item>(itemResponseBody);
                SetSlot(equipmentSlot.Slot, item);
            }
        }
         
        private void SetSlot(string slotName, Item item)
        {
            // Setting the slot
            switch (slotName)
            {
                // Armor
                case "Helm":
                    this.Resources["HeadSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Shoulders": 
                    this.Resources["ShoulderSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Coat":
                    this.Resources["ChestSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Gloves":
                    this.Resources["HandsSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Leggings":
                    this.Resources["LegSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Boots":
                    this.Resources["FeetSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "HelmAquatic":
                    this.Resources["BreathingApparatusSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;

                // Aquatic
                case "WeaponAquaticA":
                    this.Resources["AquaticWeaponSlotA"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "WeaponAquaticB":
                    this.Resources["AquaticWeaponSlotB"] = new BitmapImage(new Uri(item.Icon));
                    break;

                // Tools
                case "Sickle":
                    this.Resources["ForagingSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Axe":
                    this.Resources["LoggingSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Pick":
                    this.Resources["MiningSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;

                // Weapons
                case "WeaponA1":
                    this.Resources["SwordSlotA"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "WeaponA2":
                    this.Resources["SwordSlotB"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "ShieldA1":
                    this.Resources["ShieldSlotA"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "ShieldA2":
                    this.Resources["ShieldSlotB"] = new BitmapImage(new Uri(item.Icon));
                    break;

                // Back item and trinkets
                case "Backpack":
                    this.Resources["BackSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Accessory1":
                    this.Resources["BearTrinketSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Accessory2":
                    this.Resources["CubeTrinketSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Amulet":
                    this.Resources["AmuletSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Ring1":
                    this.Resources["RightRingSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
                case "Ring2":
                    this.Resources["LeftRingSlot"] = new BitmapImage(new Uri(item.Icon));
                    break;
            }
        }
    }
}
