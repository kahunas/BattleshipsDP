// File: Tests/GameRoomTests.cs
using System;
using System.Collections.Generic;
using Xunit;
using SharedLibrary;

namespace Tests
{
    public class GameRoomTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly_WithParameters()
        {
            // Arrange
            string roomId = "123";
            string roomName = "Battle Room";
            string difficulty = "Hard";

            // Act
            var gameRoom = new GameRoom(roomId, roomName, difficulty);

            // Assert
            Assert.Equal(roomId, gameRoom.RoomId);
            Assert.Equal(roomName, gameRoom.RoomName);
            Assert.Equal(difficulty, gameRoom.Difficulty);
            Assert.NotNull(gameRoom.Players);
            Assert.Empty(gameRoom.Players);
            Assert.NotNull(gameRoom.Game);
        }

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly_WithoutParameters()
        {
            // Act
            var gameRoom = new GameRoom();

            // Assert
            Assert.Equal(string.Empty, gameRoom.RoomId);
            Assert.Equal(string.Empty, gameRoom.RoomName);
            Assert.NotNull(gameRoom.Players);
            Assert.Empty(gameRoom.Players);
            Assert.NotNull(gameRoom.Game);
        }

        [Fact]
        public void TryAddPlayer_AddsPlayer_WhenRoomHasSpace()
        {
            // Arrange
            var gameRoom = new GameRoom();
            var player = new Player("player1", "Player One");

            // Act
            var result = gameRoom.TryAddPlayer(player);

            // Assert
            Assert.True(result);
            Assert.Single(gameRoom.Players);
            Assert.Contains(player, gameRoom.Players);
        }

        [Fact]
        public void TryAddPlayer_DoesNotAddPlayer_WhenRoomIsFull()
        {
            // Arrange
            var gameRoom = new GameRoom();
            gameRoom.Players.AddRange(new List<Player>
            {
                new Player("player1", "Player One"),
                new Player("player2", "Player Two"),
                new Player("player3", "Player Three"),
                new Player("player4", "Player Four")
            });
            var newPlayer = new Player("player5", "Player Five");

            // Act
            var result = gameRoom.TryAddPlayer(newPlayer);

            // Assert
            Assert.False(result);
            Assert.DoesNotContain(newPlayer, gameRoom.Players);
        }

        [Fact]
        public void TryAddPlayer_DoesNotAddDuplicatePlayer()
        {
            // Arrange
            var gameRoom = new GameRoom();
            var player = new Player("player1", "Player One");
            gameRoom.TryAddPlayer(player);

            // Act
            var result = gameRoom.TryAddPlayer(player);

            // Assert
            Assert.False(result);
            Assert.Single(gameRoom.Players);
        }

        [Fact]
        public void TryAddPlayer_AssignsPlayerToCorrectTeam()
        {
            // Arrange
            var gameRoom = new GameRoom();
            var players = new List<Player>
            {
                new Player("player1", "Player One"),
                new Player("player2", "Player Two"),
                new Player("player3", "Player Three"),
                new Player("player4", "Player Four")
            };

            // Act
            foreach (var player in players)
            {
                gameRoom.TryAddPlayer(player);
            }

            // Assert
            Assert.Equal("player1", gameRoom.Game.ATeamPlayer1Id);
            Assert.Equal("player2", gameRoom.Game.ATeamPlayer2Id);
            Assert.Equal("player3", gameRoom.Game.BTeamPlayer1Id);
            Assert.Equal("player4", gameRoom.Game.BTeamPlayer2Id);
        }
    }
}
