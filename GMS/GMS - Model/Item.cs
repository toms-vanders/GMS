using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    public class Item
    {
        public Item()
        {

        }
        public Item(int Id, string Chat_link, string Name, string Icon, string Description, string Type, string Rarity, int Level, int Vendor_value, ArrayList Flags, ArrayList Game_types, ArrayList Restrictions)
        {
            this.Id = Id;
            this.Chat_link = Chat_link;
            this.Name = Name;
            this.Icon = Icon;
            this.Description = Description;
            this.Type = Type;
            this.Rarity = Rarity;
            this.Level = Level;
            this.Vendor_value = Vendor_value;
            this.Flags = Flags;
            this.Game_types = Game_types;
            this.Restrictions = Restrictions;
        }
        public int Id { get; set; } 
        public string Chat_link { get; set; }
        public string Name { get; set; } 
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public int Level { get; set; }
        public int Vendor_value { get; set; }
        public ArrayList Flags { get; set; }
        public ArrayList Game_types { get; set; }
        public ArrayList Restrictions { get; set; }

    }
}
