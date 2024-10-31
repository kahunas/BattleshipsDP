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
                    int row = random.Next(1, board.Size - 2);  // Leave space around edges
                    int col = random.Next(1, board.Size - 2);
                    placed = TryPlaceShipWithSpacing(board, shipType.Item1, row, col, isHorizontal);
                }
            }
        }

        private bool TryPlaceShipWithSpacing(Board board, Ship ship, int startRow, int startCol, bool isHorizontal)
        {
            List<(int, int)> coordinates = new List<(int, int)>();
            List<(int, int)> spacingCheck = new List<(int, int)>();

            // Check ship coordinates
            for (int i = 0; i < ship.Size; i++)
            {
                int row = isHorizontal ? startRow : startRow + i;
                int col = isHorizontal ? startCol + i : startCol;

                if (row >= board.Size - 1 || col >= board.Size - 1 || board.Grid[row, col] != Square.Empty)
                {
                    return false;
                }

                coordinates.Add((row, col));
                
                // Add surrounding coordinates to check for spacing
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (row + dr >= 0 && row + dr < board.Size && 
                            col + dc >= 0 && col + dc < board.Size)
                        {
                            spacingCheck.Add((row + dr, col + dc));
                        }
                    }
                }
            }

            // Check if there's enough spacing around the ship
            foreach (var pos in spacingCheck)
            {
                if (board.Grid[pos.Item1, pos.Item2] != Square.Empty)
                {
                    return false;
                }
            }

            return board.PlaceShip(ship, coordinates);
        }
    }
}
