using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Team
    {
        public string Name { get; set; }
        public Board Board { get; set; }
        public List<Player> Players { get; set; }

        public Team(string name)
        {
            Name = name;
            Board = new Board();
            Players = new List<Player>();
        }
    }
}
