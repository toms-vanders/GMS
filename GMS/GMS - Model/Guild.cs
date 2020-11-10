using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GMS___Model
{
    public class Guild
    {
        public Guild(string name)
        {
            this.name = name;
            this.members = new ArrayList();
            this.events = new ArrayList();
        }
        public string name { get; set; }
        public ArrayList members { get; set; }
        public ArrayList events { get; set; }
    }
}
