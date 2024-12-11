using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class GridCell
    {
        private int _row;
        private int _col;
        
        public int Row 
        { 
            get => _row;
            set
            {
                _row = value;
                Console.WriteLine($"Setting Row to {value}");
            }
        }
        
        public int Col 
        { 
            get => _col;
            set
            {
                _col = value;
                Console.WriteLine($"Setting Col to {value}");
            }
        }
        
        public bool IsHighlighted { get; set; } = false;
        public Square State { get; set; } = Square.Empty; // Track state of the cell
        public bool IsSelectedShip { get; set; } = false;

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
