using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    class Item
    {
        public Item(string name, int value, int quantity, string description)
        {
            this.name = name;
            this.value = value;
            this.quantity = quantity;
            this.description = description;
        }
        public string name { get; set; } 
        public int value { get; set; }
        public int quantity { get; set; } 
        public string description { get; set; }

    }
}
