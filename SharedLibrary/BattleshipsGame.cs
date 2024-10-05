using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class BattleshipsGame
    {
        //--- TEAM A
        public string ATeamPlayer1Id { get; set; } //Host
        public string ATeamPlayer2Id { get; set; }
        public Board ATeamBoard { get; set; }

        //--- TEAM B
        public string BTeamPlayer1Id { get; set; }
        public string BTeamPlayer2Id { get; set; }
        public Board BTeamBoard { get; set; }


        public string CurrentPlayerId { get; set; }
        public bool GameStarted { get; set; } = false;
        public bool GameOver { get; set; } = false;


        public BattleshipsGame()
        {
            ATeamPlayer1Id = string.Empty;
            ATeamPlayer2Id = string.Empty;
            BTeamPlayer1Id = string.Empty;
            BTeamPlayer2Id = string.Empty;
            CurrentPlayerId = string.Empty;
        }
        public void Start()
        {
            Console.WriteLine("Game started");
        }
    }
}
