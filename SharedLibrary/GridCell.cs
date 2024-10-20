using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class GridCell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsHighlighted { get; set; } = false;
        public Square State { get; set; } = Square.Empty; // Track state of the cell

        public string GetCellBackground()
        {
            if (this.State == Square.Ship)
            {
                return "gray";
            }
            else if (this.State == Square.Hit)
            {
                return "red";
            }
            else if (this.State == Square.Miss)
            {
                return "blue";
            }
            else if (this.IsHighlighted)
            {
                return "yellow";
            }
            else
            {
                return "transparent";
            }
        }
    }
}
