using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SharedLibrary;


namespace SharedLibrary.State
{
    public class SetupState : IGameState
    {
        public void EnterState(BattleshipsGame game)
        {
            Console.WriteLine("Entering Setup State...");
            game.GameStarted = true;
            game.GameOver = false;

            // Initialize boards
            game.ATeamBoard = game.GetLevelFactory().GetBoard();
            game.BTeamBoard = game.GetLevelFactory().GetBoard();
        }

        public void ExecuteState(BattleshipsGame game)
        {

                Console.WriteLine("All players ready. Transitioning to Turn State...");
                game.PlaceShips();
                game.CountShots();
                game.GameContext.SetState(new TurnState(), game);

        }

        public void ExitState(BattleshipsGame game)
        {
            Console.WriteLine("Exiting Setup State.");
        }
    }
}
