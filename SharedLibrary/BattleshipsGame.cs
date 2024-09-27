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
            return;
            bool isTeam1Turn = true;

            //while (!Team1.HasLost() && !Team2.HasLost())
            //{
            //    Team currentTeam = isTeam1Turn ? Team1 : Team2;
            //    Team opponentTeam = isTeam1Turn ? Team2 : Team1;

            //    // Here you'd handle the actual player turn logic, such as:
            //    // 1. Let the current player fire at the opponent's board.
            //    // 2. Process ship placements (if in the placement phase).
            //    // 3. Alternate between players within the team for taking turns.

            //    // Example: Team1 Player1 fires at Team2's board
            //    Console.WriteLine($"{currentTeam.Player1.Name}'s turn.");
            //    currentTeam.Player1.FireAtOpponent(opponentTeam.TeamBoard, row: 3, col: 4);

            //    // Switch turns
            //    isTeam1Turn = !isTeam1Turn;
            //}

            //if (Team1.HasLost())
            //{
            //    Console.WriteLine("Team 2 wins!");
            //}
            //else if (Team2.HasLost())
            //{
            //    Console.WriteLine("Team 1 wins!");
            //}
        }

        

    }
}
