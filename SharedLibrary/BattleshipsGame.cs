using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class BattleshipsGame
    {
        //--- TEAM A
        public string ATeamPlayer1Id { get; set; } //Host
        public string ATeamPlayer2Id { get; set; }
        public Board ATeamBoard { get; set; }

        //--- TEAM B
        public string BTeamPlayer1Id { get; set; }
        public string BTeamPlayer2Id { get; set; }
        public Board BTeamBoard { get; set; }

        public Team ATeam { get; set; }
        public Team BTeam { get; set; }

        public string CurrentPlayerId { get; set; }
        public bool GameStarted { get; set; } = false;
        public bool GameOver { get; set; } = false;


        public BattleshipsGame()
        {
            ATeamPlayer1Id = string.Empty;
            ATeamPlayer2Id = string.Empty;
            BTeamPlayer1Id = string.Empty;
            BTeamPlayer2Id = string.Empty;
            CurrentPlayerId = string.Empty;


        }
        public void Start()
        {
            GameStarted = true;
            GameOver = false;

            DividePlayersIntoTeams(new List<Player>
            {
                new Player(ATeamPlayer1Id, "Player 1"),
                new Player(ATeamPlayer2Id, "Player 2"),
                new Player(BTeamPlayer1Id, "Player 3"),
                new Player(BTeamPlayer2Id, "Player 4")
            });

            ATeamBoard = ATeam.Board;
            BTeamBoard = BTeam.Board;

            PrintTeams();

            // Define ships to be placed
            var ships = new List<(Ship, Square)>
            {
                (new Ship("Destroyer", 2), Square.Ship),
                (new Ship("Submarine", 3), Square.Ship),
                (new Ship("Cruiser", 3), Square.Ship),
                (new Ship("Battleship", 4), Square.Ship),
                (new Ship("Carrier", 5), Square.Ship)
            };

            // Randomly place ships on Team A's board
            ATeamBoard.InitializeGrid();
            ATeamBoard.RandomlyPlaceShips(ships);
            Console.WriteLine("Team A's Board:");
            ATeamBoard.Display();

            // Randomly place ships on Team B's board
            BTeamBoard.InitializeGrid();
            BTeamBoard.RandomlyPlaceShips(ships);
            Console.WriteLine("Team B's Board:");
            BTeamBoard.Display();

            Console.WriteLine("Game started");
        }


        public void DividePlayersIntoTeams(List<Player> players)
        {
            if (players.Count < 4)
            {
                throw new ArgumentException("Not enough players to form two teams.");
            }

            ATeam = new Team("Team A")
            {
                Players = new List<Player> { players[0], players[1] },
                Board = new Board()
            };
            BTeam = new Team("Team B")
            {
                Players = new List<Player> { players[2], players[3] },
                Board = new Board()
            };

            ATeamPlayer1Id = players[0].ConnectionId;
            ATeamPlayer2Id = players[1].ConnectionId;
            BTeamPlayer1Id = players[2].ConnectionId;
            BTeamPlayer2Id = players[3].ConnectionId;
        }

        public void PrintTeams()
        {
            Console.WriteLine("Team A:");
            foreach (var player in ATeam.Players)
            {
                Console.WriteLine($"Player ID: {player.ConnectionId}, Name: {player.Name}");
            }

            Console.WriteLine("Team B:");
            foreach (var player in BTeam.Players)
            {
                Console.WriteLine($"Player ID: {player.ConnectionId}, Name: {player.Name}");
            }
        }
    }
}
