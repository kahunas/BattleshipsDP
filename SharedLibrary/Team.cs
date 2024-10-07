using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Team
    {
        private bool hasLost { get; set; }
        private bool turn { get; set; }
        private Board board { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }

        public Team(string name) {
            hasLost = false;
            turn = false;
            board = new Board();
            Players = new List<Player>();
            Name = name;
        }

        public bool HasLost
        { get
            {  
                return hasLost;
            }
        }
        
        public Board Board
        {
            get
            {
                return board;
            }
            set
            {
                this.board = value;
            }
        }
        
        public void Lost()
        {
           this.hasLost = true;
        }

        public bool IsTurn()
        {
            return turn;
        }

        public void Turn(bool switcher)
        {
            turn = switcher;
        }
    }
}
