using SharedLibrary;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests;

public class BoardTests
{
    private class TestBoard : Board
    {
        public TestBoard(int size = 10) : base(size) { }
    }

    private class TestShip : Ship
    {
        public TestShip(int size) : base("destroyer", 1) { }
    }

    [Fact]
    public void Constructor_InitializesGridAndShips()
    {
        // Arrange
        int size = 10;

        // Act
        var board = new TestBoard(size);

        // Assert
        Assert.NotNull(board.Grid);
        Assert.Equal(size, board.Grid.Count);
        Assert.All(board.Grid, row => Assert.Equal(size, row.Count));
        Assert.NotNull(board.Ships);
        Assert.Empty(board.Ships);
    }

    [Fact]
    public void AllShipsDestroyed_ReturnsFalse_WhenSomeShipsNotDestroyed()
    {
        // Arrange
        var board = new TestBoard();
        var intactShip = new TestShip(1);
        board.Ships.Add(intactShip);

        // Act
        bool result = board.AllShipsDestroyed();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void PlaceShip_AddsShipToGrid_WhenValidCoordinates()
    {
        // Arrange
        var board = new TestBoard();
        var ship = new TestShip(2);
        var coordinates = new List<(int, int)> { (0, 0), (0, 1) };

        // Act
        bool result = board.PlaceShip(ship, coordinates);

        // Assert
        Assert.True(result);
        Assert.Contains(ship, board.Ships);
        Assert.Equal(Square.Ship, board.Grid[0][0]);
        Assert.Equal(Square.Ship, board.Grid[0][1]);
    }

    [Fact]
    public void PlaceShip_ReturnsFalse_WhenCoordinatesOccupied()
    {
        // Arrange
        var board = new TestBoard();
        var ship1 = new TestShip(2);
        var ship2 = new TestShip(1);
        board.PlaceShip(ship1, new List<(int, int)> { (0, 0), (0, 1) });

        // Act
        bool result = board.PlaceShip(ship2, new List<(int, int)> { (0, 0) });

        // Assert
        Assert.False(result);
        Assert.DoesNotContain(ship2, board.Ships);
    }

    [Fact]
    public void RandomlyPlaceShips_PlacesAllShipsOnGrid()
    {
        // Arrange
        var board = new TestBoard();
        var ships = new List<(Ship, Square)>
            {
                (new TestShip(2), Square.Ship),
                (new TestShip(3), Square.Ship)
            };

        // Act
        board.RandomlyPlaceShips(ships);

        // Assert
        Assert.Equal(ships.Count, board.Ships.Count);
    }
}