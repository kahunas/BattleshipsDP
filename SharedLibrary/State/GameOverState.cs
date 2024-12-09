using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.State
{
    public class GameOverState : IGameState
    {
        public void EnterState(BattleshipsGame game)
        {
            Console.WriteLine("Entering Game Over State...");
            string winner = game.ATeam.Board.AllShipsDestroyed() ? "Team B" : "Team A";
            Console.WriteLine($"Game Over! {winner} wins!");
        }

        public void ExecuteState(BattleshipsGame game)
        {
            Console.WriteLine("Awaiting restart or exit...");
        }

        public void ExitState(BattleshipsGame game)
        {
            Console.WriteLine("Exiting Game Over State.");
        }
    }
}
