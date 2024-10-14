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
        public Team Team1 { get; set; }
        public string ATeamPlayer1Id { get; set; } //Host
        public string ATeamPlayer2Id { get; set; }

        //--- TEAM B
        public Team Team2 { get; set; }
        public string BTeamPlayer1Id { get; set; }
        public string BTeamPlayer2Id { get; set; }

        public Team ATeam { get; set; }
        public Team BTeam { get; set; }

        List<(Ship, Square)> shipTypes { get; set; }
        public string CurrentPlayerId { get; set; }
        public bool GameStarted { get; set; } = false;
        public bool GameOver { get; set; } = false;
        private bool isTeam1Turn { get; set; }
        private bool isTeam1Player1Turn { get; set; }
        private bool isTeam2Player1Turn { get; set; }



        public BattleshipsGame()
        {
            Team1 = new Team("First Team");
            ATeamPlayer1Id = string.Empty;
            ATeamPlayer2Id = string.Empty;
            Team2 = new Team("Second Team");
            BTeamPlayer1Id = string.Empty;
            BTeamPlayer2Id = string.Empty;
            CurrentPlayerId = string.Empty;
            shipTypes = new List<(Ship, Square)>();
            isTeam1Turn = true;
            isTeam1Player1Turn = true;
            isTeam2Player1Turn = true;

        }
        public void Start()
        {

            if (GameStarted)
            {
                return;
            }
            GameOver = false;
            DividePlayersIntoTeams(new List<Player>
            {
                new Player(ATeamPlayer1Id, "Player 1"),
                new Player(ATeamPlayer2Id, "Player 2"),
                new Player(BTeamPlayer1Id, "Player 3"),
                new Player(BTeamPlayer2Id, "Player 4")
            });

            PrintTeams();
            GameStarted = true;

            Team1.Board = PlaceShips(shipTypes);
            Team2.Board = PlaceShips(shipTypes);
            Team1.Turn(true);

            while (!Team1.HasLost && !Team2.HasLost && !GameOver)
            {
                HandleTurn();
            }

            if (Team1.HasLost)
            {
                GameOver = true;
            }
            else if (Team2.HasLost)
            {
                GameOver = true;
            }
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

        private void HandleTurn()
        {
            Team currentTeam = isTeam1Turn ? Team1 : Team2;
            Team opponentTeam = isTeam1Turn ? Team2 : Team1;

            string currentPlayerId = GetCurrentPlayerId();

            //bool hit = FireAtOpponent(opponentTeam.Board, row: 3, col: 4);
            bool hit = true;

            if (!hit)
            {
                SwitchTurn();
            }

            if (opponentTeam.HasLost)
            {
                GameOver = true;
                return;
            }
        }


        private Board PlaceShips(List<(Ship, Square)> shipTypes)
        {
            Board board = new Board();
            board.RandomlyPlaceShips(shipTypes);
            return board;
        }

        public void PrintTeams()
        {
            Console.WriteLine("Team A:");
            foreach (var player in ATeam.Players)
            {
                Console.WriteLine($"Player ID: {player.ConnectionId}, Name: {player.Name}");
            }
        }
        private bool FireAtOpponent(Board opponentBoard, int row, int col)
        {
            return opponentBoard.Fire(row, col);
        }

        private void SwitchTurn()
        {
            if (isTeam1Turn)
            {
                isTeam1Player1Turn = !isTeam1Player1Turn;
                if (!isTeam1Player1Turn) isTeam1Turn = false;
            }
            else
            {
                isTeam2Player1Turn = !isTeam2Player1Turn;
                if (!isTeam2Player1Turn) isTeam1Turn = true;
            }
        }

        private string GetCurrentPlayerId()
        {
            if (isTeam1Turn)
            {
                return isTeam1Player1Turn ? ATeamPlayer1Id : ATeamPlayer2Id;
            }
            else
            {
                return isTeam2Player1Turn ? BTeamPlayer1Id : BTeamPlayer2Id;
            }
        }
    }
}

