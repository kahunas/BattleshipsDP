namespace BattleshipsDP.Client.State
{
    public interface IGameState
    {
        string GetMessage(); // Message to show in the UI
        bool CanPerformAction(string action); // Check if a specific action is allowed
    }

    public class PreparationState : IGameState
    {
        public string GetMessage() => "Prepare your strategy!";

        public bool CanPerformAction(string action)
        {
            return action switch
            {
                "ConfirmStrategy" => true,
                "ReadyForBattle" => true,
                "Shoot" => false,
                "Wait" => false,
                "GameOver" => false,
                _ => false
            };
        }
    }

    public class YourTurnState : IGameState
    {
        public string GetMessage() => "Your turn! Make your move.";

        public bool CanPerformAction(string action)
        {
            return action switch
            {
                "ConfirmStrategy" => false,
                "ReadyForBattle" => false,
                "Shoot" => true,
                "Wait" => false,
                "GameOver" => false,
                _ => false
            };
        }
    }

    public class WaitingState : IGameState
    {
        public string GetMessage() => "Wait for your turn...";

        public bool CanPerformAction(string action)
        {
            return action switch
            {
                "ConfirmStrategy" => false,
                "ReadyForBattle" => false,
                "Shoot" => false,
                "Wait" => true,
                "GameOver" => false,
                _ => false
            };
        }
    }

    public class GameOverState : IGameState
    {
        private readonly string _result;

        public GameOverState(string result) => _result = result;

        public string GetMessage() => $"Game Over! {_result}";

        public bool CanPerformAction(string action)
        {
            return action switch
            {
                "ConfirmStrategy" => false,
                "ReadyForBattle" => false,
                "Shoot" => false,
                "Wait" => false,
                "GameOver" => true,
                _ => false
            };
        }
    }
}
