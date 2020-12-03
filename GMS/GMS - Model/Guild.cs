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

        public string GuildID { get; set; }
        public string Name { get; set; }
        public ArrayList Members { get; set; }
        public ArrayList Events { get; set; }
    }
}
