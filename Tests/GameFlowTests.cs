
using SharedLibrary;
using SharedLibrary.Factory;
using Xunit;

public class GameFlowTests
{
    private BattleshipsGame SetupTestGame()
    {
        var game = new BattleshipsGame();
        game.ATeamPlayer1Id = "A1";
        game.ATeamPlayer2Id = "A2";
        game.BTeamPlayer1Id = "B1";
        game.BTeamPlayer2Id = "B2";
        game.SetGameDifficulty("Easy");
        return game;
    }

    [Fact]
    public void CompleteGameFlow_Test()
    {
        // Arrange
        var game = SetupTestGame();

        // Act - Step 1: Start the game
        game.StartGame();

        // Assert - Game should be started
        Assert.True(game.GameStarted);
        Assert.False(game.GameOver);
        Assert.Equal("A1", game.CurrentPlayerId);

        // Act - Step 2: Place ships and simulate shots
        game.PlaceShips();

        // Simulate shooting all ships on Team B's board
        var isGameOver = false;
        for (int row = 0; row < game.boardSize; row++)
        {
            for (int col = 0; col < game.boardSize; col++)
            {
                string result = game.ShootCell(row, col, "A1", out isGameOver);
                
                // Assert - Shot results should be valid
                Assert.True(result == "hit" || result == "miss" || result == "already_shot");
                
                if (isGameOver)
                    break;
            }
            if (isGameOver)
                break;
        }

        // Assert - Step 3: Verify game end condition
        Assert.True(isGameOver);
        
        // Verify winning condition by checking if all Team B ships are destroyed
        Assert.True(game.BTeamBoard.AllShipsDestroyed());
    }
}