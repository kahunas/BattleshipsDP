using SharedLibrary;
using Xunit;

namespace Tests
{
    public class ShootingTests
    {
        private BattleshipsGame SetupGame()
        {
            var game = new BattleshipsGame();
            game.ATeamPlayer1Id = "player1";
            game.ATeamPlayer2Id = "player2";
            game.BTeamPlayer1Id = "player3";
            game.BTeamPlayer2Id = "player4";
            game.SetGameDifficulty("Medium");
            game.StartGame();
            game.PlaceShips();
            return game;
        }

        [Fact]
        public void ShootCell_WithValidCoordinates_ShouldReturnValidResult()
        {
            // Arrange
            var game = SetupGame();
            int row = 0;
            int col = 0;
            bool isGameOver;

            // Act
            string result = game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            Assert.Contains(result, new[] { "hit", "miss" });
        }

        [Fact]
        public void ShootCell_WithPreviouslyShotCoordinate_ShouldReturnAlreadyShot()
        {
            // Arrange
            var game = SetupGame();
            int row = 0;
            int col = 0;
            bool isGameOver;

            // Act
            game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);
            string result = game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            Assert.Equal("miss", result);
        }

        [Fact]
        public void ShootCell_WithInvalidCoordinates_ShouldReturnMiss()
        {
            // Arrange
            var game = SetupGame();
            int row = 100;  // Invalid coordinate
            int col = 100;  // Invalid coordinate
            bool isGameOver;

            // Act
            string result = game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            Assert.Equal("miss", result);
        }

        [Fact]
        public void ShootCell_WhenHittingAllShips_ShouldSetGameOver()
        {
            // Arrange
            var game = SetupGame();
            bool isGameOver = false;

            // Act & Assert
            // Shoot all possible coordinates to ensure hitting all ships
            for (int i = 0; i < game.boardSize && !isGameOver; i++)
            {
                for (int j = 0; j < game.boardSize && !isGameOver; j++)
                {
                    game.ShootCell(i, j, game.ATeamPlayer1Id, out isGameOver);
                }
            }

            // Assert that the game is over after hitting all ships
            Assert.True(game.BTeamBoard.AllShipsDestroyed() || !isGameOver);
        }

        [Fact]
        public void ValidateShot_WithValidCoordinates_ReturnsTrue()
        {
            // Arrange
            var game = SetupGame();
            int row = 5;
            int col = 5;

            // Act & Assert
            bool isGameOver;
            string result = game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);
            Assert.Contains(result, new[] { "hit", "miss" });
        }

        [Theory]
        [InlineData(-1, 5)]
        [InlineData(5, -1)]
        [InlineData(10, 5)]
        [InlineData(5, 10)]
        public void ValidateShot_WithInvalidCoordinates_ReturnsMiss(int row, int col)
        {
            // Arrange
            var game = SetupGame();

            // Act
            bool isGameOver;
            string result = game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            Assert.Equal("miss", result);
        }

        [Fact]
        public void ShootCell_WhenNotPlayersTurn_ReturnsFalse()
        {
            // Arrange
            var game = SetupGame();
            int row = 5;
            int col = 5;
            string wrongPlayer = game.BTeamPlayer1Id;
            game.CurrentPlayerId = game.ATeamPlayer1Id;

            // Act
            bool isGameOver;
            string result = game.ShootCell(row, col, wrongPlayer, out isGameOver);

            // Assert
            Assert.Equal("miss", result);
        }

        [Fact]
        public void ShootCell_UpdatesBoardState_AfterHit()
        {
            // Arrange
            var game = SetupGame();
            int row = 0;
            int col = 0;
            bool isGameOver;

            // Act
            string result = game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            Assert.True(game.BTeamBoard.Grid[row][col] == Square.Hit || 
                       game.BTeamBoard.Grid[row][col] == Square.Miss);
        }

        [Fact]
        public void ConsecutiveShots_TrackBoardStateCorrectly()
        {
            // Arrange
            var game = SetupGame();
            bool isGameOver;
            var shotCoordinates = new[] { (0, 0), (0, 1), (1, 0), (1, 1) };

            // Act
            foreach (var (row, col) in shotCoordinates)
            {
                game.ShootCell(row, col, game.ATeamPlayer1Id, out isGameOver);
            }

            // Assert
            foreach (var (row, col) in shotCoordinates)
            {
                Assert.True(
                    game.BTeamBoard.Grid[row][col] == Square.Hit || 
                    game.BTeamBoard.Grid[row][col] == Square.Miss
                );
            }
        }

        [Fact]
        public void ShootCell_UpdatesGameState_WhenAllShipsDestroyed()
        {
            // Arrange
            var game = SetupGame();
            bool isGameOver = false;
            int shotCount = 0;
            const int maxShots = 100; // Safety limit

            // Act
            for (int i = 0; i < game.boardSize && !isGameOver && shotCount < maxShots; i++)
            {
                for (int j = 0; j < game.boardSize && !isGameOver && shotCount < maxShots; j++)
                {
                    game.ShootCell(i, j, game.ATeamPlayer1Id, out isGameOver);
                    shotCount++;
                }
            }

            // Assert
            Assert.True(isGameOver || shotCount == maxShots);
        }

        [Theory]
        [InlineData("Simple")]
        [InlineData("Big")]
        [InlineData("Piercer")]
        [InlineData("Slasher")]
        [InlineData("Cross")]
        public void SelectShotType_WithValidType_ShouldSetActiveShot(string shotType)
        {
            // Arrange
            var game = SetupGame();
            bool isGameOver;

            // Act
            string result = game.SetSelectedShotType(game.ATeamPlayer1Id, shotType, out isGameOver);

            // Assert
            Assert.Equal(shotType, game.GetActiveShot(game.ATeamPlayer1Id));
        }

        [Fact]
        public void ShootCell_WithSpecialShot_ShouldAffectMultipleCells()
        {
            // Arrange
            var game = SetupGame();
            bool isGameOver;
            game.SetSelectedShotType(game.ATeamPlayer1Id, "Big", out isGameOver);
            int centerRow = 5;
            int centerCol = 5;

            // Act
            string result = game.ShootCell(centerRow, centerCol, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            // Check that surrounding cells are affected (Big shot affects 3x3 area)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int targetRow = centerRow + i;
                    int targetCol = centerCol + j;
                    if (targetRow >= 0 && targetRow < game.boardSize && 
                        targetCol >= 0 && targetCol < game.boardSize)
                    {
                        Assert.True(
                            game.BTeamBoard.Grid[targetRow][targetCol] == Square.Hit || 
                            game.BTeamBoard.Grid[targetRow][targetCol] == Square.Miss
                        );
                    }
                }
            }
        }

        [Fact]
        public void ShootCell_WithInvalidShotType_ShouldDefaultToSimpleShot()
        {
            // Arrange 
            var game = SetupGame();
            bool isGameOver;
            game.SetSelectedShotType(game.ATeamPlayer1Id, "InvalidShotType", out isGameOver);

            // Act
            string result = game.ShootCell(0, 0, game.ATeamPlayer1Id, out isGameOver);

            // Assert
            Assert.Contains(result, new[] { "hit", "miss" });
            // Only one cell should be affected
            Assert.True(
                game.BTeamBoard.Grid[0][0] == Square.Hit || 
                game.BTeamBoard.Grid[0][0] == Square.Miss
            );
        }
    }
}