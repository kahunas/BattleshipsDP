// File: Tests/GameHubTests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SharedLibrary;
using Xunit;
using BattleshipsDP.Hubs;

namespace Tests
{
    public class GameHubTests
    {
        private readonly Mock<IHubCallerClients> _clientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<IGroupManager> _groupsMock;
        private readonly Mock<GameService> _gameServiceMock;
        private readonly Mock<HubCallerContext> _contextMock;
        private readonly GameHub _hub;

        public GameHubTests()
        {
            // Mock dependencies
            _clientsMock = new Mock<IHubCallerClients>();
            _clientProxyMock = new Mock<IClientProxy>();
            _groupsMock = new Mock<IGroupManager>();
            _contextMock = new Mock<HubCallerContext>();
            _gameServiceMock = new Mock<GameService>();

            // Setup default behaviors
            _clientsMock.Setup(clients => clients.Caller).Returns((ISingleClientProxy)_clientProxyMock.Object);
            _clientsMock.Setup(clients => clients.All).Returns(_clientProxyMock.Object);
            _contextMock.Setup(context => context.ConnectionId).Returns("test-connection-id");

            // Pass the mocked GameService into the GameHub constructor
            _hub = new GameHub(_gameServiceMock.Object)
            {
                Clients = _clientsMock.Object,
                Groups = _groupsMock.Object,
                Context = _contextMock.Object
            };
        }

        [Fact]
        public async Task OnConnectedAsync_SendsRoomsListToCaller()
        {
            // Arrange
            var rooms = new List<GameRoom>
            {
                new GameRoom("room1", "Room One"),
                new GameRoom("room2", "Room Two")
            };
            _gameServiceMock.Setup(service => service.GetAllRooms()).Returns(rooms);

            // Act
            await _hub.OnConnectedAsync();

            // Assert
            _clientsMock.Verify(clients => clients.Caller.SendAsync("Rooms", It.Is<IEnumerable<GameRoom>>(r => r.Count() == rooms.Count()), default), Times.Once);
        }

        [Fact]
        public async Task CreateRoom_CreatesRoomAndAddsPlayer()
        {
            // Arrange
            var playerName = "Player One";
            var roomName = "Test Room";
            var difficulty = "Medium";

            var room = new GameRoom("room-id", roomName, difficulty);
            _gameServiceMock.Setup(service => service.CreateRoom(roomName, difficulty)).Returns(room);
            _gameServiceMock.Setup(service => service.TryAddPlayerToRoom(It.IsAny<string>(), It.IsAny<Player>())).Returns(true);

            // Act
            var result = await _hub.CreateRoom(roomName, playerName, difficulty);

            // Assert
            Assert.Equal(room, result);
            _groupsMock.Verify(groups => groups.AddToGroupAsync("test-connection-id", "room-id", default), Times.Once);
            _clientsMock.Verify(clients => clients.All.SendAsync("Rooms", It.IsAny<IEnumerable<GameRoom>>(), default), Times.Once);
        }

        [Fact]
        public async Task JoinRoom_AddsPlayerToRoom_AndNotifiesClients()
        {
            // Arrange
            var roomId = "room-id";
            var playerName = "Player Two";

            var room = new GameRoom(roomId, "Test Room");
            _gameServiceMock.Setup(service => service.TryAddPlayerToRoom(roomId, It.IsAny<Player>())).Returns(true);
            _gameServiceMock.Setup(service => service.GetRoomById(roomId)).Returns(room);

            // Act
            var result = await _hub.JoinRoom(roomId, playerName);

            // Assert
            Assert.Equal(room, result);
            _groupsMock.Verify(groups => groups.AddToGroupAsync("test-connection-id", roomId, default), Times.Once);
            _clientsMock.Verify(clients => clients.Group(roomId).SendAsync("PlayerJoined", It.IsAny<Player>(), default), Times.Once);
            _clientsMock.Verify(clients => clients.All.SendAsync("Rooms", It.IsAny<IEnumerable<GameRoom>>(), default), Times.Once);
        }

        [Fact]
        public async Task GetBoardSize_ReturnsCorrectSize_BasedOnDifficulty()
        {
            // Arrange
            var playerId = "test-connection-id";
            var room = new GameRoom("room-id", "Test Room", "Easy");
            _gameServiceMock.Setup(service => service.GetRoomByPlayerId(playerId)).Returns(room);

            // Act
            var result = await _hub.GetBoardSize(playerId);

            // Assert
            Assert.Equal(8, result);
        }
        
        [Fact]
        public async Task PlayerReady_SetsPlayerAsTeamLeaderAndReady()
        {
            // Arrange
            var playerId = "test-connection-id";
            var player = new Player(playerId, "Player One") { IsReadyForBattle = false };
            var room = new GameRoom("room-id", "Test Room");
            room.Players.Add(player);
            room.Game.ATeamPlayer1Id = playerId;

            _gameServiceMock.Setup(service => service.GetRoomByPlayerId(playerId)).Returns(room);

            // Act
            await _hub.PlayerReady();

            // Assert
            Assert.True(player.IsReadyForBattle);
            Assert.True(player.IsTeamLeader);
            _clientsMock.Verify(clients => clients.Client(playerId).SendAsync(
                "ReceivePlayerInfo",
                player.Name,
                playerId,
                It.IsAny<string>(),
                true,
                default), Times.Once);
        }
    }
}
