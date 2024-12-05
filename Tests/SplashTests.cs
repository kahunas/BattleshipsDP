// File: SharedLibrary.Tests/SplashTests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SharedLibrary;
using SharedLibrary.Builder;

namespace Tests
{
    public class SplashTests
    {
        private class MockShot : IShot
        {
            public string Name => "Mock Shot";
            public List<(int, int)> Spread => new List<(int, int)> { (0, 0) };

            public List<(int, int)> ShotCoordinates(int row, int col)
            {
                return new List<(int, int)>
                {
                    (row, col)
                };
            }
        }

        [Fact]
        public void Constructor_InitializesWithBaseShotAndRadius()
        {
            // Arrange
            var baseShot = new MockShot();
            int splashRadius = 2;

            // Act
            var splashShot = new SplashDamageDecorator(baseShot, splashRadius);

            // Assert
            Assert.Equal($"{baseShot.Name} with Splash Damage", splashShot.Name);
        }

        [Fact]
        public void ShotCoordinates_ReturnsCorrectCoordinates_WithSplashRadius()
        {
            // Arrange
            var baseShot = new MockShot();
            int splashRadius = 1;
            var splashShot = new SplashDamageDecorator(baseShot, splashRadius);
            int row = 5, col = 5;

            // Act
            var coordinates = splashShot.ShotCoordinates(row, col);

            // Assert
            var expectedCoordinates = new HashSet<(int, int)>
            {
                (4, 4), (4, 5), (4, 6),
                (5, 4), (5, 5), (5, 6),
                (6, 4), (6, 5), (6, 6)
            };

            Assert.Equal(expectedCoordinates, coordinates.ToHashSet());
        }

        [Fact]
        public void ShotCoordinates_ReturnsSinglePoint_WhenSplashRadiusIsZero()
        {
            // Arrange
            var baseShot = new MockShot();
            int splashRadius = 0;
            var splashShot = new SplashDamageDecorator(baseShot, splashRadius);
            int row = 5, col = 5;

            // Act
            var coordinates = splashShot.ShotCoordinates(row, col);

            // Assert
            var expectedCoordinates = new List<(int, int)> { (5, 5) };
            Assert.Equal(expectedCoordinates, coordinates);
        }

        [Fact]
        public void ShotCoordinates_IncludesBaseShotCoordinates()
        {
            // Arrange
            var baseShot = new MockShot();
            int splashRadius = 1;
            var splashShot = new SplashDamageDecorator(baseShot, splashRadius);
            int row = 5, col = 5;

            // Act
            var coordinates = splashShot.ShotCoordinates(row, col);

            // Assert
            Assert.Contains((5, 5), coordinates);
        }

        [Fact]
        public void Name_IncludesSplashDamage()
        {
            // Arrange
            var baseShot = new MockShot();
            var splashShot = new SplashDamageDecorator(baseShot, 1);

            // Act
            var name = splashShot.Name;

            // Assert
            Assert.Equal("Mock Shot with Splash Damage", name);
        }
    }
}
