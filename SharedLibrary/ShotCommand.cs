using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Builder;

namespace SharedLibrary
{
    public class ShotCommand
    {
        public int Row { get; }
        public int Col { get; }
        public string ShotType { get; }
        public bool IsPlayerShot { get; }

        public ShotCommand(int row, int col, string shotType, bool isPlayerShot)
        {
            Row = row;
            Col = col;
            ShotType = shotType;
            IsPlayerShot = isPlayerShot;
        }

        public void Execute(List<List<GridCell>> board)
        {
            var cell = board[Row][Col];
            cell.IsHighlighted = true;
            cell.State = ShotType == "hit" ? Square.Hit : Square.Miss;  // Assume basic hit/miss outcome for simplicity
        }

        public void Undo(List<List<GridCell>> board)
        {
            var cell = board[Row][Col];
            cell.IsHighlighted = false;
            cell.State = Square.Empty;  // Reset to original state
        }
    }
}
