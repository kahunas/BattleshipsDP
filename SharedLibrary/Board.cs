using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SharedLibrary.Iterator;

namespace SharedLibrary
{
    public abstract class Board
    {
        public int Size { get; set; }
        public List<List<Square>> Grid { get; private set; }
        public List<Ship> Ships { get; set; }
        private Random random = new Random();

        public Board(int size = 10)
        {
            Size = size;
            Grid = new List<List<Square>>();
            Ships = new List<Ship>();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int row = 0; row < Size; row++)
            {
                var rowList = new List<Square>();
                for (int col = 0; col < Size; col++)
                {
                    rowList.Add(Square.Empty);
                }
                Grid.Add(rowList);
            }
        }

        // No need for GetSerializableGrid() as Grid is already serializable
        public List<List<Square>> GetSerializableGrid() => Grid;

        public bool AllShipsDestroyed()
        {
            return Ships.All(ship => ship.IsDestroyed());
        }

        public bool PlaceShip(Ship ship, List<(int, int)> coordinates)
        {
            foreach (var coord in coordinates)
            {
                if (Grid[coord.Item1][coord.Item2] != Square.Empty)
                    return false;
            }

            ship.Place(coordinates);
            Ships.Add(ship);

            foreach (var coord in coordinates)
                Grid[coord.Item1][coord.Item2] = Square.Ship;

            return true;
        }

        public void RandomlyPlaceShips(List<(Ship, Square)> shipTypes)
        {
            foreach (var shipType in shipTypes)
            {
                bool placed = false;

                while (!placed)
                {
                    bool isHorizontal = random.Next(0, 2) == 0;
                    int row = random.Next(0, Size);
                    int col = random.Next(0, Size);
                    placed = TryPlaceShip(shipType.Item1, row, col, isHorizontal, shipType.Item2);
                }
            }
        }

        private bool TryPlaceShip(Ship ship, int startRow, int startCol, bool isHorizontal, Square shipType)
        {
            List<(int, int)> coordinates = new List<(int, int)>();

            for (int i = 0; i < ship.Size; i++)
            {
                int row = isHorizontal ? startRow : startRow + i;
                int col = isHorizontal ? startCol + i : startCol;

                if (row >= Size || col >= Size || Grid[row][col] != Square.Empty)
                {
                    return false;
                }

                coordinates.Add((row, col));
            }

            return PlaceShip(ship, coordinates);
        }

        public IGridIterator CreateIterator()
        {
            return new BoardIterator(this);
        }

        public bool CheckForHits()
        {
            var iterator = CreateIterator();
            iterator.SetFilter(cell => cell.State == Square.Ship);

            while (iterator.HasNext())
            {
                var cell = iterator.Next();
                foreach (var ship in Ships)
                {
                    if (ship.Coordinates.Contains((cell.Row, cell.Col)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void ProcessCells(Action<GridCell> action, Func<GridCell, bool> filter = null)
        {
            var iterator = CreateIterator();
            if (filter != null)
            {
                iterator.SetFilter(filter);
            }

            while (iterator.HasNext())
            {
                var cell = iterator.Next();
                action(cell);
            }
        }

        public void DisplayBoardState()
        {
            Console.WriteLine("\nBoard State (using Iterator):");
            Console.WriteLine("  " + string.Join(" ", Enumerable.Range(0, Size).Select(i => i.ToString("D2"))));
            
            for (int row = 0; row < Size; row++)
            {
                Console.Write($"{row:D2} ");
                for (int col = 0; col < Size; col++)
                {
                    Console.Write(Grid[row][col].GetDescription() + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
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