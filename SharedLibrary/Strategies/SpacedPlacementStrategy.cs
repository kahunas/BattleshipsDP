namespace SharedLibrary.Strategies
{
    public class SpacedPlacementStrategy : IShipPlacementStrategy
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
                    // Adjust random range to account for ship size and board boundaries
                    int maxPos = board.Size - shipType.Item1.Size;
                    int row = random.Next(0, board.Size);
                    int col = random.Next(0, board.Size);
                    placed = TryPlaceShipWithSpacing(board, shipType.Item1, row, col, isHorizontal);
                }
            }
        }

        private bool TryPlaceShipWithSpacing(Board board, Ship ship, int startRow, int startCol, bool isHorizontal)
        {
            List<(int, int)> coordinates = new List<(int, int)>();
            List<(int, int)> spacingCheck = new List<(int, int)>();

            // First, check if the ship would fit within the board boundaries
            for (int i = 0; i < ship.Size; i++)
            {
                int row = isHorizontal ? startRow : startRow + i;
                int col = isHorizontal ? startCol + i : startCol;

                if (row >= board.Size || col >= board.Size)
                {
                    return false;
                }

                coordinates.Add((row, col));

                // Check surrounding cells for spacing
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int checkRow = row + dr;
                        int checkCol = col + dc;

                        if (checkRow >= 0 && checkRow < board.Size && 
                            checkCol >= 0 && checkCol < board.Size)
                        {
                            spacingCheck.Add((checkRow, checkCol));
                        }
                    }
                }
            }

            // Check if any of the spacing cells are occupied
            foreach (var pos in spacingCheck)
            {
                if (board.Grid[pos.Item1][pos.Item2] != Square.Empty)
                {
                    return false;
                }
            }

            // All checks passed, place the ship
            return board.PlaceShip(ship, coordinates);
        }
    }
}