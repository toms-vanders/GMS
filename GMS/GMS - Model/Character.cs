using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    class Character
    {
        public Character(string characterName, string characterClass, string email, int level, string guildRank)
        {
            this.characterName = characterName;
            this.characterClass = characterClass;
            this.email = email;
            this.level = level;
            this.guildRank = guildRank;
            items = new ArrayList();
        }
        public string characterName { get; set; }
        public string characterClass { get; set; }
        public string email { get; set; }
        public int level { get; set; }
        public string guildRank { get; set; }
        public Guild guild { get; set; }
        public ArrayList items { get; set; }
    }
}
