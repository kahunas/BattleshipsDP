// File: SharedLibrary.Tests/TeamTests.cs
using System;
using System.Collections.Generic;
using Xunit;
using SharedLibrary;
using SharedLibrary.Factory;
using SharedLibrary.Bridge;

namespace Tests
{
    public class TeamTests
    {
        private class TestShot
        {
            public int Amount { get; set; }
            public TestShot(int amount)
            {
                Amount = amount;
            }
        }

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            string teamName = "Team A";

            // Act
            var team = new Team(teamName);

            // Assert
            Assert.Equal(teamName, team.Name);
            Assert.NotNull(team.Board);
            Assert.NotNull(team.Players);
            Assert.NotEmpty(team.ShotCollection);
        }

        [Fact]
        public void TakeShot_DecreasesShotAmount_WhenShotTypeExistsAndHasShots()
        {
            // Arrange
            var team = new Team("Team A");
            var initialAmount = team.ShotCollection[0].Amount;

            // Act
            var result = team.TakeShot(typeof(SimpleShot));

            // Assert
            Assert.True(result);
            Assert.Equal(initialAmount - 1, team.ShotCollection[0].Amount);
        }

        [Fact]
        public void TakeShot_ReturnsFalse_WhenShotTypeDoesNotExist()
        {
            // Arrange
            var team = new Team("Team A");

            // Act
            var result = team.TakeShot(typeof(TestShot));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TakeShot_ReturnsFalse_WhenShotTypeHasNoShots()
        {
            // Arrange
            var team = new Team("Team A");
            team.ShotCollection[0].Amount = 0;

            // Act
            var result = team.TakeShot(typeof(SimpleShot));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemainingShots_ReturnsCorrectAmount_WhenShotTypeExists()
        {
            // Arrange
            var team = new Team("Team A");
            var expectedAmount = team.ShotCollection[0].Amount;

            // Act
            var result = team.RemainingShots(typeof(SimpleShot));

            // Assert
            Assert.Equal(expectedAmount, result);
        }

        [Fact]
        public void RemainingShots_ReturnsNegativeOne_WhenShotTypeDoesNotExist()
        {
            // Arrange
            var team = new Team("Team A");

            // Act
            var result = team.RemainingShots(typeof(TestShot));

            // Assert
            Assert.Equal(-1, result);
        }
    }
}
