// Add this to a new file TurnHandler.cs
using SharedLibrary;

public abstract class TurnHandler
{
    protected readonly BattleshipsGame _game;
    protected readonly List<ITurnObserver> _observers;

    protected TurnHandler(BattleshipsGame game)
    {
        _game = game;
        _observers = new List<ITurnObserver>();
    }

    // Template method that defines the algorithm's skeleton
    public void ExecuteTurn(string playerId, int row, int col, string shotType)
    {
        if (!ValidatePlayerTurn(playerId))
            return;

        if (!ValidateShot(row, col, shotType))
            return;

        ProcessShot(row, col, shotType, playerId);

        bool isGameOver = CheckGameOver();
        
        if (!isGameOver)
        {
            UpdateNextPlayer();
            NotifyObservers();
        }
    }

    // These are the steps that can vary between implementations
    // These methods are abstract and must be implemented by the concrete classes | subclasses
    protected abstract bool ValidatePlayerTurn(string playerId);
    protected abstract bool ValidateShot(int row, int col, string shotType);
    protected abstract void ProcessShot(int row, int col, string shotType, string playerId);
    protected abstract bool CheckGameOver();
    protected abstract void UpdateNextPlayer();

    // Common functionality
    protected void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateTurn(_game.CurrentPlayerId);
        }
    }

    public void RegisterObserver(ITurnObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void UnregisterObserver(ITurnObserver observer)
    {
        _observers.Remove(observer);
    }
}