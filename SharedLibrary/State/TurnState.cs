using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.State
{
    public class TurnState : IGameState
    {
        public void EnterState(BattleshipsGame game)
        {
            Console.WriteLine("Entering Turn State...");
            Console.WriteLine($"It's {game.CurrentPlayerId}'s turn.");
        }

        public void ExecuteState(BattleshipsGame game)
        {
            if (game.GameOver)
            {
                Console.WriteLine("Game Over. Transitioning to Game Over State...");
                game.GameContext.SetState(new GameOverState(), game);
            }
            else
            {
                Console.WriteLine($"Shot result: {game.shotResult}");
                game.UpdateTurn(); // Update turn to the next player
            }
        }

        public void ExitState(BattleshipsGame game)
        {
            Console.WriteLine("Exiting Turn State.");
        }
    }
}
