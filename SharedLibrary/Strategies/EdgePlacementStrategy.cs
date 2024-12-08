namespace SharedLibrary.Strategies
{
    public class EdgePlacementStrategy : IShipPlacementStrategy
    {
        private Random random = new Random();

        public void PlaceShips(Board board, List<(Ship, Square)> ships)
        {
            foreach (var shipType in ships)
            {
                bool placed = false;
                while (!placed)
                {
                    // Try to place along edges
                    int edge = random.Next(4); // 0: top, 1: right, 2: bottom, 3: left
                    placed = edge switch
                    {
                        0 => TryPlaceShip(board, shipType.Item1, 0, random.Next(board.Size), true),
                        1 => TryPlaceShip(board, shipType.Item1, random.Next(board.Size), board.Size - 1, false),
                        2 => TryPlaceShip(board, shipType.Item1, board.Size - 1, random.Next(board.Size), true),
                        3 => TryPlaceShip(board, shipType.Item1, random.Next(board.Size), 0, false),
                        _ => false
                    };
                }
            }
        }

        private bool TryPlaceShip(Board board, Ship ship, int startRow, int startCol, bool isHorizontal)
        {
            List<(int, int)> coordinates = new List<(int, int)>();

            for (int i = 0; i < ship.Size; i++)
            {
                int row = isHorizontal ? startRow : startRow + i;
                int col = isHorizontal ? startCol + i : startCol;

                if (row >= board.Size || col >= board.Size || board.Grid[row, col] != Square.Empty)
                {
                    return false;
                }

                coordinates.Add((row, col));
            }

            return board.PlaceShip(ship, coordinates);
        }
    }
}
