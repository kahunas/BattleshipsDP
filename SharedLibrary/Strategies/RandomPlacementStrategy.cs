namespace SharedLibrary.Strategies
{
    public class RandomPlacementStrategy : IShipPlacementStrategy
    {
        private Random random = new Random();

        public void PlaceShips(Board board, List<(Ship, Square)> ships)
        {
            foreach (var shipType in ships)
            {
                bool placed = false;
                while (!placed)
                {
                    bool isHorizontal = random.Next(0, 2) == 0;
                    int row = random.Next(0, board.Size);
                    int col = random.Next(0, board.Size);
                    placed = TryPlaceShip(board, shipType.Item1, row, col, isHorizontal);
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
