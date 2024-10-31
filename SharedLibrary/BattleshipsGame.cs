using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Strategies;

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
        public int boardSize = 10;

        private List<ITurnObserver> turnObservers = new List<ITurnObserver>();
        private Dictionary<string, IShipPlacementStrategy> placementStrategies;
        private string teamAStrategy = "Random";
        private string teamBStrategy = "Random";

        //List<(Ship, Square)> shipsToPlace = new List<(Ship, Square)>
        //    {
        //        (new Ship("Destroyer", 2), Square.Ship),
        //        (new Ship("Submarine", 3), Square.Ship),
        //        (new Ship("Cruiser", 3), Square.Ship),
        //        (new Ship("Battleship", 4), Square.Ship),
        //        (new Ship("Carrier", 5), Square.Ship)
        //    };
        
        public BattleshipsGame()
        {
            ATeamPlayer1Id = string.Empty;
            ATeamPlayer2Id = string.Empty;
            BTeamPlayer1Id = string.Empty;
            BTeamPlayer2Id = string.Empty;
            CurrentPlayerId = string.Empty;

            placementStrategies = new Dictionary<string, IShipPlacementStrategy>
            {
                { "Random", new RandomPlacementStrategy() },
                { "Edge", new EdgePlacementStrategy() },
                { "Spaced", new SpacedPlacementStrategy() }
            };
        }

        public void StartGame()
        {
            Console.WriteLine("Game started");

            GameStarted = true;
            GameOver = false;

            //define player prototypes
            var prototypePlayerA1 = new Player(ATeamPlayer1Id, "Player 1");
            var prototypePlayerA2 = new Player(ATeamPlayer2Id, "Player 2");
            var prototypePlayerB1 = new Player(BTeamPlayer1Id, "Player 3");
            var prototypePlayerB2 = new Player(BTeamPlayer2Id, "Player 4");

            // Clone and modify prototypes to create new player instances
            DividePlayersIntoTeams(new List<Player>
            {
                (Player)prototypePlayerA1.Clone(),
                (Player)prototypePlayerA2.Clone(),
                (Player)prototypePlayerB1.Clone(),
                (Player)prototypePlayerB2.Clone()
            });

            ATeamBoard = ATeam.Board;
            BTeamBoard = BTeam.Board;

            // Define ships to be placed
            IShipFactory blueShipFactory = new BlueShipFactory();
            IShipFactory redShipFactory = new RedShipFactory();

            List<(Ship, Square)> shipsToPlaceTeamA = new List<(Ship, Square)>
            {
                (blueShipFactory.CreateDestroyer("Destroyer"), Square.Ship),
                (blueShipFactory.CreateSubmarine("Submarine"), Square.Ship),
                (blueShipFactory.CreateSubmarine("Submarine"), Square.Ship),
                (blueShipFactory.CreateBattleship("Battleship"), Square.Ship),
                (blueShipFactory.CreateCarrier("Carrier"), Square.Ship)
            };

            List<(Ship, Square)> shipsToPlaceTeamB = new List<(Ship, Square)>
            {
                (redShipFactory.CreateDestroyer("Destroyer"), Square.Ship),
                (redShipFactory.CreateSubmarine("Submarine"), Square.Ship),
                (redShipFactory.CreateSubmarine("Submarine"), Square.Ship),
                (redShipFactory.CreateBattleship("Battleship"), Square.Ship),
                (redShipFactory.CreateCarrier("Carrier"), Square.Ship)
            };

            // Use the selected strategies for each team
            placementStrategies[teamAStrategy].PlaceShips(ATeamBoard, shipsToPlaceTeamA);
            placementStrategies[teamBStrategy].PlaceShips(BTeamBoard, shipsToPlaceTeamB);

            //PrintTeams();

            // Set the first player to start the game
            CurrentPlayerId = ATeamPlayer1Id;

            //Console.WriteLine($"First turn goes to Player 1 of Team A: {CurrentPlayerId}");

            // Debug: Print the boards after placing ships
            //Console.WriteLine("Team A Board:");
            //ATeamBoard.PrintBoard();
            //Console.WriteLine("Team B Board:");
            //BTeamBoard.PrintBoard();

            // Notify observers about the first turn
            NotifyTurnObservers();
        }

        public void DividePlayersIntoTeams(List<Player> players)
        {
            if (players.Count < 4)
            {
                throw new ArgumentException("Not enough players to form two teams.");
            }

            // Set team leaders (first player of each team)
            players[0].IsTeamLeader = true;
            players[2].IsTeamLeader = true;

            ATeam = new Team("Team A")
            {
                Players = new List<Player> { players[0], players[1] },
                Board = new Board(boardSize)
            };
            BTeam = new Team("Team B")
            {
                Players = new List<Player> { players[2], players[3] },
                Board = new Board(boardSize)
            };

            ATeamPlayer1Id = players[0].ConnectionId;
            ATeamPlayer2Id = players[1].ConnectionId;
            BTeamPlayer1Id = players[2].ConnectionId;
            BTeamPlayer2Id = players[3].ConnectionId;
        }

        public void UpdateTurn()
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


            // Notify all observers that the turn has changed
            NotifyTurnObservers();
        }

        //~~~ Observer ~~~
        public void RegisterTurnObserver(ITurnObserver observer)
        {
            if (!turnObservers.Contains(observer))
            {
                turnObservers.Add(observer);
            }
        }

        public void UnregisterTurnObserver(ITurnObserver observer)
        {
            if (turnObservers.Contains(observer))
            {
                turnObservers.Remove(observer);
            }
        }

        private void NotifyTurnObservers()
        {
            foreach (var observer in turnObservers)
            {
                observer.UpdateTurn(CurrentPlayerId);
            }
        }
        //~~~ Observer ~~~

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
            
            // Determine if it's a hit or miss
            string result = null;
            if (boardSize > row && boardSize > col)
            {
                // Check if cell has already been shot
                if (opponentBoard.Grid[row, col] == Square.Hit || opponentBoard.Grid[row, col] == Square.Miss)
                {
                    result = "already_shot";
                }

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
            }
            else
            {
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

        public List<IShot> DefineShots()
        {
            var builder = new ShotBuilder();

            return new List<IShot>
    {
        builder.SetName("Simple").SetSpread(new List<(int, int)> { (0, 0) }).Build(),
        builder.SetName("Big").SetSpread(new List<(int, int)>
        {
            (0, 0), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1)
        }).Build(),
        builder.SetName("Piercer").SetSpread(new List<(int, int)>
        {
            (0, 0), (1, 0), (2, 0), (3, 0)
        }).Build(),
        builder.SetName("Slasher").SetSpread(new List<(int, int)>
        {
            (0, 0), (0, 1), (0, 2), (0, 3)
        }).Build(),
        builder.SetName("Cross").SetSpread(new List<(int, int)>
        {
            (0, 0), (1, 0), (-1, 0), (0, 1), (0, -1)
        }).Build()
    };
        }

        // Add new method to set team strategy
        public void SetTeamStrategy(string team, string strategy)
        {
            if (placementStrategies.ContainsKey(strategy))
            {
                if (team == "Team A")
                    teamAStrategy = strategy;
                else if (team == "Team B")
                    teamBStrategy = strategy;
            }
        }
    }
}
