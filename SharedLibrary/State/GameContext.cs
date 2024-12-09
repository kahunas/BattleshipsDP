using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.State
{
    public class GameContext
    {
        private IGameState _currentState;
        public GameContext() { }
        public void SetState(IGameState newState, BattleshipsGame game)
        {
            _currentState?.ExitState(game);  // Exit current state
            _currentState = newState;       // Transition to the new state
            _currentState.EnterState(game); // Enter the new state
        }

        public void Update(BattleshipsGame game)
        {
            _currentState?.ExecuteState(game); // Execute the current state's logic
        }
    }
}
