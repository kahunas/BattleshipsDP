using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class GameState
    {
        public List<List<GridCell>> TeamCells { get; set; }
        public List<List<GridCell>> EnemyCells { get; set; }
        public bool IsPlayerTurn { get; set; }
        public string CurrentPlayerId { get; set; }

        public GameState(List<List<GridCell>> teamCells, List<List<GridCell>> enemyCells, bool isPlayerTurn, string currentPlayerId)
        {
            TeamCells = teamCells.Select(row => row.Select(cell => cell.Clone()).ToList()).ToList();
            EnemyCells = enemyCells.Select(row => row.Select(cell => cell.Clone()).ToList()).ToList();
            IsPlayerTurn = isPlayerTurn;
            CurrentPlayerId = currentPlayerId;
        }

        public GameStateMemento SaveState()
        {
            return new GameStateMemento(this);
        }

        public void RestoreState(GameStateMemento memento)
        {
            TeamCells = memento.TeamCells.Select(row => row.Select(cell => cell.Clone()).ToList()).ToList();
            EnemyCells = memento.EnemyCells.Select(row => row.Select(cell => cell.Clone()).ToList()).ToList();
            IsPlayerTurn = memento.IsPlayerTurn;
            CurrentPlayerId = memento.CurrentPlayerId;
        }
    }

    public class GameStateMemento
    {
        public List<List<GridCell>> TeamCells { get; private set; }
        public List<List<GridCell>> EnemyCells { get; private set; }
        public bool IsPlayerTurn { get; private set; }
        public string CurrentPlayerId { get; private set; }

        public GameStateMemento(GameState state)
        {
            TeamCells = state.TeamCells.Select(row => row.Select(cell => cell.Clone()).ToList()).ToList();
            EnemyCells = state.EnemyCells.Select(row => row.Select(cell => cell.Clone()).ToList()).ToList();
            IsPlayerTurn = state.IsPlayerTurn;
            CurrentPlayerId = state.CurrentPlayerId;
        }
    }


}
