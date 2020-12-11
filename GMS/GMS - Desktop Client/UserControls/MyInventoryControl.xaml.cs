using GMS___Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
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
    /// Interaction logic for MyInventoryControl.xaml
    /// </summary>
    public partial class MyInventoryControl : UserControl
    {
        private readonly HttpClient client;
        private static List<Item> items = new List<Item>();

        public MyInventoryControl()
        {
            InitializeComponent();
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/")
            };
            client.DefaultRequestHeaders.Add("Authorization", (string)App.Current.Properties["ApiKey"]);
            GetItems();
            // TODO: Save icons in static property
        }

        public async void GetItems()
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

                foreach (var equipmentSlot in equipments.Equipment)
                {
                    // Getting item
                    string itemResponseBody;
                    try
                    {
                        itemResponseBody = await client.GetStringAsync("gw2api/items/" + equipmentSlot.Id);
                    } catch (Exception e)
                    {
                        WpfMessageBox.Show("Item could not be displayed", "Error, while trying to get the item with id: " + equipmentSlot.Id, MessageBoxButton.OK, MessageBoxImage.Warning);
                        continue;
                    }
                    Item item = JsonConvert.DeserializeObject<Item>(itemResponseBody);
                    items.Add(item);
                }
            }
            FillWrapPanel();
        }

        private void FillWrapPanel([Optional] List<Item> itemList)
        {
            List<Item> itemListToTraverse;
            if (!(itemList is null))
            {
                itemListToTraverse = itemList;
            } else
            {
                itemListToTraverse = items;
            }

            foreach (var item in itemListToTraverse)
            {
                // Item image
                Image item1Image = new Image();
                item1Image.Source = new BitmapImage(new Uri(item.Icon));
                item1Image.Width = 80;
                item1Image.HorizontalAlignment = HorizontalAlignment.Center;

                // Item Name
                TextBlock itemName = new TextBlock();
                itemName.Height = 30;
                itemName.Width = 90;
                itemName.TextWrapping = TextWrapping.Wrap;
                itemName.Text = item.Name;
                itemName.HorizontalAlignment = HorizontalAlignment.Center;

                // Stack panel for item
                StackPanel singleItem = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                singleItem.Children.Add(item1Image);
                singleItem.Children.Add(itemName);

                // Wrapping in a button
                Button btn = new Button
                {
                    Name = "btn" + item.Id.ToString(),
                    BorderThickness = new Thickness(0),
                    Background = Brushes.SlateGray,
                    Margin = new Thickness { Right = 25, Bottom = 25 },
                    Content = singleItem
                };

                // Adding item button event handler 
                btn.Click += new RoutedEventHandler(ItemSelectionChanged);

                // Finally appending the item button to the wrap panel
                itemsWrapPanel.Children.Add(btn);
            }

            // Setting the ImageSource of first item to display in the item details
            this.Resources["SelectedItemImage"] = new BitmapImage(new Uri(items[0].Icon));
        }

        private void ItemSelectionChanged(object sender, RoutedEventArgs e)
        {
            // Getting button name (it contains the id of item)
            string buttonName = ((Button)sender).Name;

            // Parsing name to get the ID
            int itemId = Int32.Parse(buttonName.Substring(3));

            // Getting item with matching ID from the static List<Item> items
            Item item = (Item) (from singleItem in items where singleItem.Id == itemId select singleItem).FirstOrDefault();
            
            // Setting item details 
            this.Resources["SelectedItemImage"] = new BitmapImage(new Uri(item.Icon));
            selectedItemName.Text = item.Name;
            selectedItemName.ToolTip = item.Name;
            selectedItemDescription.Text = item.Description;
            selectedItemDescription.ToolTip = item.Description;
            selectedItemType.Text = item.Type;
            selectedItemType.ToolTip = item.Type;
            selectedItemLevel.Text = item.Level.ToString();
            selectedItemLevel.ToolTip = item.Level.ToString();
            selectedItemRarity.Text = item.Rarity;
            selectedItemRarity.ToolTip = item.Rarity;
            selectedItemVendorValue.Text = item.Vendor_value.ToString();
            selectedItemVendorValue.ToolTip = item.Vendor_value.ToString();
        }

        private void searchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(items.Count == 0))
            {
                // Todo extract grid instead, and filter it, instead of filtering items List.
                var filterByName = items.Where(ev => ev.Name.IndexOf(searchField.Text, (StringComparison)CompareOptions.IgnoreCase) >= 0);
                List<Item> itemList = filterByName.ToList();

                // Remove or add items to wrapPanel
                itemsWrapPanel.Children.Clear();
                FillWrapPanel(itemList);
            }
        }
    }
}
