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
        public void StartGame()
        {
            Console.WriteLine("Game started");
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

            // Define ships to be placed
            List<(Ship, Square)> shipsToPlaceTeamA = new List<(Ship, Square)>
            {
                (new Ship("Destroyer", 2), Square.Ship),
                (new Ship("Submarine", 3), Square.Ship),
                (new Ship("Cruiser", 3), Square.Ship),
                (new Ship("Battleship", 4), Square.Ship),
                (new Ship("Carrier", 5), Square.Ship)
            };

            List<(Ship, Square)> shipsToPlaceTeamB = new List<(Ship, Square)>
            {
                (new Ship("Destroyer", 2), Square.Ship),
                (new Ship("Submarine", 3), Square.Ship),
                (new Ship("Cruiser", 3), Square.Ship),
                (new Ship("Battleship", 4), Square.Ship),
                (new Ship("Carrier", 5), Square.Ship)
            };

            // Randomly place ships for both teams
            ATeamBoard.RandomlyPlaceShips(shipsToPlaceTeamA);
            BTeamBoard.RandomlyPlaceShips(shipsToPlaceTeamB);

            PrintTeams();


            // Set the first player to start the game
            CurrentPlayerId = ATeamPlayer1Id;
            Console.WriteLine($"First turn goes to Player 1 of Team A: {CurrentPlayerId}");

            // Debug: Print the boards after placing ships
            Console.WriteLine("Team A Board:");
            ATeamBoard.PrintBoard();  // Assuming there's a method to print the board state to the console
            Console.WriteLine("Team B Board:");
            BTeamBoard.PrintBoard();  // Assuming there's a method to print the board state to the console
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

        public void SwitchToNextPlayer()
        {
            // Determine the current player and switch to the next one
            if (CurrentPlayerId == ATeamPlayer1Id)
            {
                CurrentPlayerId = ATeamPlayer2Id;
            }
            else if (CurrentPlayerId == ATeamPlayer2Id)
            {
                CurrentPlayerId = BTeamPlayer1Id;
            }
            else if (CurrentPlayerId == BTeamPlayer1Id)
            {
                CurrentPlayerId = BTeamPlayer2Id;
            }
            else if (CurrentPlayerId == BTeamPlayer2Id)
            {
                CurrentPlayerId = ATeamPlayer1Id;
            }

            Console.WriteLine($"It is now {CurrentPlayerId}'s turn.");
        }

        public void SetPlayerReady(string connectionId)
        {
            var player = ATeam.Players.Concat(BTeam.Players).FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player != null)
            {
                player.IsReady = true;
            }
        }

        public string GetTeamByPlayer(string connectionId)
        {
            return ATeam.Players.Any(p => p.ConnectionId == connectionId) ? "Team A" : "Team B";
        }

        public IEnumerable<Player> GetTeammates(string connectionId)
        {
            return GetTeamByPlayer(connectionId) == "Team A" ? ATeam.Players : BTeam.Players;
        }

        public string ShootCell(int row, int col, string connectionId, out bool isGameOver)
        {
            isGameOver = false;

            // Determine the attacking and defending teams
            string attackingTeam = GetTeamByPlayer(connectionId);
            var opponentBoard = attackingTeam == "Team A" ? BTeamBoard : ATeamBoard;

            // Check if cell has already been shot
            if (opponentBoard.Grid[row, col] == Square.Hit || opponentBoard.Grid[row, col] == Square.Miss)
            {
                return "already_shot";
            }

            // Determine if it's a hit or miss
            string result;
            if (opponentBoard.Grid[row, col] == Square.Ship)
            {
                opponentBoard.Grid[row, col] = Square.Hit;
                result = "hit";

                foreach (var ship in opponentBoard.Ships)
                {
                    if (ship.Hit(row, col))
                    {
                        break;
                    }
                }

                if (opponentBoard.AllShipsDestroyed())
                {
                    isGameOver = true;
                }
            }
            else
            {
                opponentBoard.Grid[row, col] = Square.Miss;
                result = "miss";
            }

            return result;
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
