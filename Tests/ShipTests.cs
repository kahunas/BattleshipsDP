// File: Tests/ShipTests.cs
using System;
using System.Collections.Generic;
using Xunit;
using SharedLibrary;
using SharedLibrary.Bridge;
using SharedLibrary.Builder;

namespace Tests
{
    public class ShipTests
    {
        private class TestShip : Ship
        {
            public TestShip(string name, int size) : base(name, size) { }
        }

        [Fact]
        public void Constructor_InitializesShipCorrectly()
        {
            // Arrange
            string name = "Test Ship";
            int size = 3;

            // Act
            var ship = new TestShip(name, size);

            // Assert
            Assert.Equal(name, ship.Name);
            Assert.Equal(size, ship.Size);
            Assert.Equal(size, ship.Health);
            Assert.NotNull(ship.Coordinates);
            Assert.Empty(ship.Coordinates);
            Assert.NotNull(ship.HitCoordinates);
            Assert.Empty(ship.HitCoordinates);
            Assert.NotEmpty(ship.SpecialShots);
        }

        [Fact]
        public void Place_AssignsCoordinatesToShip()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            var coordinates = new List<(int, int)> { (1, 1), (1, 2), (1, 3) };

            // Act
            ship.Place(coordinates);

            // Assert
            Assert.Equal(coordinates, ship.Coordinates);
        }

        [Fact]
        public void Hit_DecreasesHealthAndMarksHit_WhenCoordinateIsHit()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            ship.Place(new List<(int, int)> { (1, 1), (1, 2), (1, 3) });

            // Act
            var result = ship.Hit(1, 2);

            // Assert
            Assert.True(result);
            Assert.Equal(2, ship.Health);
            Assert.Contains((1, 2), ship.HitCoordinates);
        }

        [Fact]
        public void Hit_ReturnsFalse_WhenCoordinateIsMissed()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            ship.Place(new List<(int, int)> { (1, 1), (1, 2), (1, 3) });

            // Act
            var result = ship.Hit(2, 2);

            // Assert
            Assert.False(result);
            Assert.Equal(3, ship.Health);
            Assert.DoesNotContain((2, 2), ship.HitCoordinates);
        }

        [Fact]
        public void IsDestroyed_ReturnsTrue_WhenHealthIsZero()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 1);
            ship.Place(new List<(int, int)> { (1, 1) });

            // Act
            ship.Hit(1, 1);
            var result = ship.IsDestroyed();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddBigShot_IncreasesBigShotAmount()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            var initialAmount = ship.SpecialShots.OfType<BigShot>().First().Amount;

            // Act
            ship.AddBigShot(5);

            // Assert
            Assert.Equal(initialAmount + 5, ship.SpecialShots.OfType<BigShot>().First().Amount);
        }

        [Fact]
        public void AddPiercerShot_IncreasesPiercerShotAmount()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            var initialAmount = ship.SpecialShots.OfType<PiercerShot>().First().Amount;

            // Act
            ship.AddPiercerShot(3);

            // Assert
            Assert.Equal(initialAmount + 3, ship.SpecialShots.OfType<PiercerShot>().First().Amount);
        }

        [Fact]
        public void AddSlasherShot_IncreasesSlasherShotAmount()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            var initialAmount = ship.SpecialShots.OfType<SlasherShot>().First().Amount;

            // Act
            ship.AddSlasherShot(2);

            // Assert
            Assert.Equal(initialAmount + 2, ship.SpecialShots.OfType<SlasherShot>().First().Amount);
        }

        [Fact]
        public void AddCrossShot_IncreasesCrossShotAmount()
        {
            // Arrange
            var ship = new TestShip("Test Ship", 3);
            var initialAmount = ship.SpecialShots.OfType<CrossShot>().First().Amount;

            // Act
            ship.AddCrossShot(4);

            // Assert
            Assert.Equal(initialAmount + 4, ship.SpecialShots.OfType<CrossShot>().First().Amount);
        }
    }
}
