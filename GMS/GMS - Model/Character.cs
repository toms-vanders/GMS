using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    public class Character
    {
        public Character()
        {

        }
        public Character(string name, string race, string gender, string profession, int level, string guild, int age, string created, int deaths, string title)
        {
            this.Name = name;
            this.Race = race;
            this.Gender = profession;
            this.Level = level;
            this.Guild = guild;
            this.Age = age;
            this.Created = created;
            this.Deaths = deaths;
            this.Title = title;
        }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public int Level { get; set; }
        public string Guild { get; set; }
        public int Age { get; set; }
        public string Created { get; set; }
        public int Deaths { get; set; }
        public string Title { get; set; }
    }
}
