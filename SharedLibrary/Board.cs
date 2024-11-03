using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public abstract class Board
    {
        public int Size { get; set; }
        public Square[,] Grid
        {
            get => _gridAdapter.GetArrayGrid();
            set => _gridAdapter.SetGrid(value);
        }
        public List<Ship> Ships { get; set; }
        private Random random = new Random();
        private IGridAdapter _gridAdapter;

        public Board(int size = 10)
        {
            Size = size;
            _gridAdapter = new GridAdapter(size);
            Grid = new Square[Size, Size];
            Ships = new List<Ship>();

            // Initialize the grid with Square.Empty
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    Grid[row, col] = Square.Empty;
        }

        public List<List<Square>> GetSerializableGrid()
        {
            return _gridAdapter.GetListGrid();
        }


        public bool AllShipsDestroyed()
        {
            return Ships.All(ship => ship.IsDestroyed());
        }


        // Method to place a ship on the board using the appropriate enum
        public bool PlaceShip(Ship ship, List<(int, int)> coordinates)
        {
            foreach (var coord in coordinates)
            {
                if (Grid[coord.Item1, coord.Item2] != Square.Empty)
                    return false; // Cannot place ship on an already occupied space
            }

            ship.Place(coordinates);
            Ships.Add(ship);

            // Mark the board where the ship is placed
            foreach (var coord in coordinates)
                Grid[coord.Item1, coord.Item2] = Square.Ship;

            return true;
        }

        // Randomly place ships on the board (both horizontally and vertically)
        public void RandomlyPlaceShips(List<(Ship, Square)> shipTypes)
        {
            foreach (var shipType in shipTypes)
            {
                bool placed = false;

                while (!placed)
                {
                    // Randomly choose orientation: 0 = horizontal, 1 = vertical
                    bool isHorizontal = random.Next(0, 2) == 0;

                    // Generate random starting coordinates
                    int row = random.Next(0, Size);
                    int col = random.Next(0, Size);

                    // Try placing the ship
                    placed = TryPlaceShip(shipType.Item1, row, col, isHorizontal, shipType.Item2);
                }
            }
        }

        // Helper method to try placing a ship at a given starting position and orientation
        private bool TryPlaceShip(Ship ship, int startRow, int startCol, bool isHorizontal, Square shipType)
        {
            List<(int, int)> coordinates = new List<(int, int)>();

            // Calculate the ship's coordinates
            for (int i = 0; i < ship.Size; i++)
            {
                int row = isHorizontal ? startRow : startRow + i;
                int col = isHorizontal ? startCol + i : startCol;

                // Check if the ship would be out of bounds
                if (row >= Size || col >= Size || Grid[row, col] != Square.Empty)
                {
                    return false; // Invalid position
                }

                coordinates.Add((row, col));
            }

            // Place the ship on the board if it's a valid position
            return PlaceShip(ship, coordinates);
        }

        //public void PrintBoard()
        //{
        //    for (int row = 0; row < Grid.GetLength(0); row++)
        //    {
        //        for (int col = 0; col < Grid.GetLength(1); col++)
        //        {
        //            var square = Grid[row, col];
        //            Console.Write(square == Square.Ship ? "S " : ". ");
        //        }
        //        Console.WriteLine();
        //    }
        //}

    }

    public static class SquareExtensions
    {
        public static string GetDescription(this Square square)
        {
            var field = square.GetType().GetField(square.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? square.ToString() : attribute.Description;
        }
    }
}
