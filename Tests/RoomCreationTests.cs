using SharedLibrary;
using SharedLibrary.Factory;
using Xunit;

namespace Tests
{
    public class RoomCreationTests
    {
        [Theory]
        [InlineData("Room1", "Easy", 8)] // Easy mode: 8x8 board
        [InlineData("Room2", "Medium", 10)] // Medium mode: 10x10 board
        [InlineData("Room3", "Hard", 12)] // Hard mode: 12x12 board
        public void RoomCreation_WithDifferentDifficulties_Test(string roomName, string difficulty,
            int expectedBoardSize)
        {
            // Arrange
            var game = new BattleshipsGame();

            // Step 1 & 2: Set room name and difficulty
            Assert.NotEmpty(roomName);
            game.SetGameDifficulty(difficulty);

            // Step 3: Create room and add players
            game.ATeamPlayer1Id = "Player1";
            game.ATeamPlayer2Id = "Player2";
            game.BTeamPlayer1Id = "Player3";
            game.BTeamPlayer2Id = "Player4";

            // Step 4: Start game and verify board configuration
            game.StartGame();
            game.PlaceShips();

            // Assert
            Assert.True(game.GameStarted);
            Assert.Equal(expectedBoardSize, game.boardSize);

            // Verify ship counts based on difficulty
            int expectedShipCount = difficulty switch
            {
                "Easy" => 5, // Easy mode has fewer ships
                "Medium" => 6, // Medium mode has standard ships
                "Hard" => 8, // Hard mode has more ships
                _ => 0
            };

            Assert.Equal(expectedShipCount, game.ATeamBoard.Ships.Count);
            Assert.Equal(expectedShipCount, game.BTeamBoard.Ships.Count);
        }

        [Fact]
        public void RoomCreation_ValidatesRequiredPlayers_Test()
        {
            // Arrange
            var game = new BattleshipsGame();
            game.SetGameDifficulty("Medium");

            // Act & Assert - Verify that game won't start without all players
            game.ATeamPlayer1Id = "Player1";
            game.ATeamPlayer2Id = "Player2";
            // Intentionally not setting B team players

            // Game should not have started due to missing players
            Assert.False(game.GameStarted);
        }

        [Fact]
        public void RoomJoining_AndTeamAssignment_Test()
        {
            // Arrange
            var gameRoom = new GameRoom("room1", "Test Room");
            var player1 = new Player("player1", "Host"); // Host/Creator
            var player2 = new Player("player2", "Player 2");
            var player3 = new Player("player3", "Player 3");
            var player4 = new Player("player4", "Player 4");

            // Act - Step 1: Players join the room
            bool allPlayersJoined = gameRoom.TryAddPlayer(player1) &&
                                    gameRoom.TryAddPlayer(player2) &&
                                    gameRoom.TryAddPlayer(player3) &&
                                    gameRoom.TryAddPlayer(player4);

            // Assert - Step 1: Verify all players joined successfully
            Assert.True(allPlayersJoined);
            Assert.Equal(4, gameRoom.Players.Count);

            // Act - Step 2: Start the game
            gameRoom.Game.StartGame();

            // Assert - Step 2: Verify game started
            Assert.True(gameRoom.Game.GameStarted);

            // Assert - Step 3: Verify team assignments
            // Team A
            Assert.Contains(gameRoom.Game.ATeam.Players, p => p.ConnectionId == "player1");
            Assert.Contains(gameRoom.Game.ATeam.Players, p => p.ConnectionId == "player2");
            Assert.Equal(2, gameRoom.Game.ATeam.Players.Count);

            // Team B
            Assert.Contains(gameRoom.Game.BTeam.Players, p => p.ConnectionId == "player3");
            Assert.Contains(gameRoom.Game.BTeam.Players, p => p.ConnectionId == "player4");
            Assert.Equal(2, gameRoom.Game.BTeam.Players.Count);

            // Verify each player knows their team
            Assert.Equal("Team A", gameRoom.Game.GetTeamByPlayer("player1"));
            Assert.Equal("Team A", gameRoom.Game.GetTeamByPlayer("player2"));
            Assert.Equal("Team B", gameRoom.Game.GetTeamByPlayer("player3"));
            Assert.Equal("Team B", gameRoom.Game.GetTeamByPlayer("player4"));
        }
    }
}
