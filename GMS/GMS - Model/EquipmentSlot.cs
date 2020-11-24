using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    public class EquipmentSlot
    {
        public EquipmentSlot(int Id, string Slot, string Bound_to, ArrayList Dyes)
        {
            this.Id = Id;
            this.Slot = Slot;
            this.Bound_to = Bound_to;
            this.Dyes = Dyes;
        }
        public int Id { get; set; }
        public string Slot { get; set; }
        public string Bound_to { get; set; }
        public ArrayList Dyes { get; set; }
    }
}
