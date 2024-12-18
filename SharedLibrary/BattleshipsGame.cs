﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Builder;
using SharedLibrary.Factory;
using SharedLibrary.Interpreter;
using SharedLibrary.Strategies;
using SharedLibrary.Visitor;

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
        public int boardSize;
        private LevelFactory _levelFactory { get; set; }

        private List<ITurnObserver> turnObservers = new List<ITurnObserver>();
        private TurnHandler _turnHandler;
        IStrategyExpression ATeamStrategyExpression;
        IStrategyExpression BTeamStrategyExpression;
        private string teamAStrategy = "Random";
        private string teamBStrategy = "Random";

        public IVisitor TeamAStatistics;
        public IVisitor TeamBStatistics;


        public BattleshipsGame()
        {
            ATeamPlayer1Id = string.Empty;
            ATeamPlayer2Id = string.Empty;
            BTeamPlayer1Id = string.Empty;
            BTeamPlayer2Id = string.Empty;
            CurrentPlayerId = string.Empty;
            TeamAStatistics = new TeamAStatisticsVisitor();
            TeamBStatistics = new TeamBStatisticsVisitor();
            _turnHandler = new BattleshipsTurnHandler(this);
        }

        public void SetGameDifficulty(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    _levelFactory = new EasyFactory();
                    break;
                case "Medium":
                    _levelFactory = new MediumFactory();
                    break;
                case "Hard":
                    _levelFactory = new HardFactory();
                    break;
                default:
                    break;
            }
        }

        public void StartGame()
        {
            Console.WriteLine("Game started");

            GameStarted = true;
            GameOver = false;

            //define player prototypes
            var prototypePlayerA1 = new Player(ATeamPlayer1Id, "Player 1");
            prototypePlayerA1.IsTeamLeader = true;
            var prototypePlayerA2 = new Player(ATeamPlayer2Id, "Player 2");
            var prototypePlayerB1 = new Player(BTeamPlayer1Id, "Player 3");
            prototypePlayerB1.IsTeamLeader = true;
            var prototypePlayerB2 = new Player(BTeamPlayer2Id, "Player 4");

            // Clone and modify prototypes to create new player instances
            DividePlayersIntoTeams(new List<Player>
            {
                (Player)prototypePlayerA1.Clone(),
                (Player)prototypePlayerA2.Clone(),
                (Player)prototypePlayerB1.Clone(),
                (Player)prototypePlayerB2.Clone()
            });

            ATeamBoard = _levelFactory.GetBoard();
            BTeamBoard = _levelFactory.GetBoard();

            boardSize = ATeamBoard.Size;

            // Set the first player to start the game
            CurrentPlayerId = ATeamPlayer1Id;

            _turnHandler.ExecuteTurn(CurrentPlayerId, -1, -1, "");
        }

        public void PlaceShips()
        {
            // Use the selected strategies for each team
            ATeamStrategyExpression.PlaceShips(ATeamBoard, _levelFactory.GetShips());
            BTeamStrategyExpression.PlaceShips(BTeamBoard, _levelFactory.GetShips());

            // Display ship counts for both teams
            Console.WriteLine("\nTeam A Ships:");
            DisplayShipCounts(ATeamBoard.Ships);
            
            Console.WriteLine("\nTeam B Ships:");
            DisplayShipCounts(BTeamBoard.Ships);
        }

        private void DisplayShipCounts(List<Ship> ships)
        {
            var shipCounts = ships
                .GroupBy(s => s.Name)
                .Select(g => new { Type = g.Key, Count = g.Count() });

            foreach (var shipType in shipCounts)
            {
                Console.WriteLine($"{shipType.Type}: {shipType.Count}");
            }
        }

        public void CountShots()
        {
            foreach (var ship in ATeamBoard.Ships)
            {
                foreach (var shot in ship.SpecialShots)
                {
                    ATeam.AddShots(shot.GetType(), shot);
                }
            }
            foreach (var ship in BTeamBoard.Ships)
            {
                foreach (var shot in ship.SpecialShots)
                {
                    BTeam.AddShots(shot.GetType(), shot);
                }
            }
        }

        public void DividePlayersIntoTeams(List<Player> players)
        {
            if (players.Count < 4)
            {
                throw new ArgumentException("Not enough players to form two teams.");
            }

            // Remove the manual team leader setting here since it's already set in the prototypes
            ATeam = new Team("Team A")
            {
                Players = new List<Player> { players[0], players[1] },
                Board = new MediumBoard()
            };
            BTeam = new Team("Team B")
            {
                Players = new List<Player> { players[2], players[3] },
                Board = new MediumBoard()
            };

            ATeamPlayer1Id = players[0].ConnectionId;
            ATeamPlayer2Id = players[1].ConnectionId;
            BTeamPlayer1Id = players[2].ConnectionId;
            BTeamPlayer2Id = players[3].ConnectionId;
        }

        public void ExecuteTurn(string playerId, int row, int col, string shotType)
        {
            _turnHandler.ExecuteTurn(playerId, row, col, shotType);
        }

        public void RegisterTurnObserver(ITurnObserver observer)
        {
            _turnHandler.RegisterObserver(observer);
        }

        public void UnregisterTurnObserver(ITurnObserver observer)
        {
            _turnHandler.UnregisterObserver(observer);
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
            string attackingTeam = GetTeamByPlayer(connectionId);
            var opponentBoard = attackingTeam == "Team A" ? BTeamBoard : ATeamBoard;

            //Console.WriteLine($"\n=== Processing shot at [{row},{col}] by {attackingTeam} ===");

            if (boardSize > row && boardSize > col)
            {
                var iterator = opponentBoard.CreateIterator();
                iterator.SetFilter(cell => cell.Row == row && cell.Col == col);

                if (iterator.HasNext())
                {
                    var targetCell = iterator.Next();
                    //Console.WriteLine($"Iterator found cell at [{targetCell.Row},{targetCell.Col}] with state: {targetCell.State}");

                    if (targetCell.State == Square.Hit || targetCell.State == Square.Miss)
                    {
                        return "already_shot";
                    }

                    if (targetCell.State == Square.Ship)
                    {
                        opponentBoard.Grid[row][col] = Square.Hit;
                        //Console.WriteLine("Hit confirmed!");

                        foreach (var ship in opponentBoard.Ships)
                        {
                            if (ship.Hit(row, col))
                            {
                                //Console.WriteLine($"Hit registered on ship: {ship.Name}");
                                break;
                            }
                        }

                        if (!opponentBoard.CheckForHits())
                        {
                            isGameOver = true;
                            //Console.WriteLine("Game Over - All ships destroyed!");
                        }

                        //opponentBoard.DisplayBoardState();
                        return "hit";
                    }
                    
                    opponentBoard.Grid[row][col] = Square.Miss;
                    //Console.WriteLine("Miss recorded!");
                    //opponentBoard.DisplayBoardState();
                    return "miss";
                }
            }

            return "miss";
        }

        // Add new method to set team strategy
        public void SetTeamStrategy(string team, string strategy)
        {
            if (team == "Team A")
            {
                ATeamStrategyExpression = StrategyFactory.Create(strategy);
            }
            else if (team == "Team B")
            {
                BTeamStrategyExpression = StrategyFactory.Create(strategy);
            }
        }

        public LevelFactory GetLevelFactory()
        {
            return this._levelFactory;
        }
    }
}
