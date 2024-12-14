using System;

namespace SharedLibrary
{
    public class GridCell
    {
        private static readonly Dictionary<(int, int, Square), GridCell> _flyweightCache = new();

        public int Row { get; set; }
        public int Col { get; set; }
        public Square State { get; set; }
        public bool IsHighlighted { get; set; } = false;
        public bool IsSelectedShip { get; set; } = false;

        public GridCell()
        {
        }
        public GridCell(int row, int col)
        {
            Row = row;
            Col = col;
        }
        private GridCell(int row, int col, Square state)
        {
            Row = row;
            Col = col;
            State = state;
        }

        public static GridCell Create(int row, int col, Square state)
        {
            if (!_flyweightCache.TryGetValue((row, col, state), out var cell))
            {
                cell = new GridCell(row, col, state);
                _flyweightCache[(row, col, state)] = cell;
            }

            return cell;
        }

        public GridCell Clone()
        {
            // Creates a new GridCell instance with the same state.
            return new GridCell(Row, Col, State) { IsHighlighted = IsHighlighted };
        }

        public string GetCellBackground()
        {
            return State switch
            {
                Square.Ship => "gray",
                Square.Hit => "red",
                Square.Miss => "blue",
                _ when IsHighlighted => "yellow",
                _ => "transparent"
            };
        }
    }
}
