using Newtonsoft.Json;
using System.Collections;

namespace GMS___Model
{
    public class Guild
    {
        public Guild(string id, string name)
        {
            this.GuildID = id;
            this.Name = name;
            this.Members = new ArrayList();
            this.Events = new ArrayList();
        }

        [JsonConstructor]
        public Guild(int level, int influence, int aetherium, int resonance, int favor, int member_count, int member_capacity, string id, string name, string tag)
        {
            Level = level;
            Influence = influence;
            Aetherium = aetherium;
            Resonance = resonance;
            Favor = favor;
            MemberCount = member_count;
            MemberCapacity = member_capacity;
            GuildID = id;
            Name = name;
            Tag = tag;
            Members = new ArrayList();
            Events = new ArrayList();
        }

        public int Level { get; set; }
        public int Influence { get; set; }
        public int Aetherium { get; set; }
        public int Resonance { get; set; }
        public int Favor { get; set; }
        public int MemberCount { get; set; }
        public int MemberCapacity { get; set; }
        public string GuildID { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public ArrayList Members { get; set; }
        public ArrayList Events { get; set; }
    }
}
