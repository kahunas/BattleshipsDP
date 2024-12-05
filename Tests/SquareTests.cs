// File: SharedLibrary.Tests/EnumExtensionsTests.cs
using System;
using Xunit;
using SharedLibrary;
using System.ComponentModel;

namespace Tests
{
    public class SquareTests
    {
        [Theory]
        [InlineData(Square.Empty, " ")]
        [InlineData(Square.Ship, "S")]
        [InlineData(Square.Hit, "X")]
        [InlineData(Square.Miss, "M")]
        public void GetDescription_ReturnsCorrectDescription_ForEnumWithDescription(Square square, string expectedDescription)
        {
            // Act
            var description = square.GetDescription();

            // Assert
            Assert.Equal(expectedDescription, description);
        }
    }
}
