namespace BattleshipsDP.Client.State
{
    public class GameStateContext
    {
        private IGameState _currentState;

        public GameStateContext(IGameState initialState)
        {
            _currentState = initialState;
        }

        public void SetState(IGameState newState)
        {
            _currentState = newState;
        }

        public string GetMessage() => _currentState.GetMessage();

        public bool CanPerformAction(string action) => _currentState.CanPerformAction(action);
    }
}
