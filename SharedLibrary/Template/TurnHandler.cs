using System;
using System.Collections.Generic;

namespace SharedLibrary.Template
{
    public abstract class TurnHandler
    {
        protected List<ITurnObserver> observers = new List<ITurnObserver>();

        // Template method that defines the turn algorithm
        public void ExecuteTurn(string playerId, int row, int col, string shotType)
        {
            if (!ValidatePlayer(playerId))
            {
                NotifyObservers($"Invalid player: {playerId}");
                return;
            }

            if (!ValidateMove(row, col))
            {
                NotifyObservers($"Invalid move: ({row}, {col})");
                return;
            }

            var result = ProcessMove(playerId, row, col, shotType);
            UpdateGameState(result);
            NotifyObservers(result);
            ProcessNextTurn();
        }

        // Abstract methods to be implemented by concrete classes
        protected abstract bool ValidatePlayer(string playerId);
        protected abstract bool ValidateMove(int row, int col);
        protected abstract string ProcessMove(string playerId, int row, int col, string shotType);
        protected abstract void UpdateGameState(string result);
        protected abstract void ProcessNextTurn();

        // Observer pattern methods
        public void RegisterObserver(ITurnObserver observer)
            => observers.Add(observer);

        public void UnregisterObserver(ITurnObserver observer)
            => observers.Remove(observer);

        protected virtual void NotifyObservers(string result)
        {
            foreach (var observer in observers)
            {
                observer.UpdateTurn(result);
            }
        }
    }
}
