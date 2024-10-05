using System;
using System.Collections.Generic;
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

        List<(Ship, Square)> shipTypes { get; set; }
        public string CurrentPlayerId { get; set; }
        public bool GameStarted { get; set; } = false;
        public bool GameOver { get; set; } = false;
        private bool isTeam1Turn {  get; set; }
        private bool isTeam1Player1Turn { get; set; }
        private bool isTeam2Player1Turn { get; set; }



        public BattleshipsGame()
        {
            Team1 = new Team();
            ATeamPlayer1Id = string.Empty;
            ATeamPlayer2Id = string.Empty;
            Team2 = new Team();
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

        private void HandleTurn()
        {
            Team currentTeam = isTeam1Turn ? Team1 : Team2;
            Team opponentTeam = isTeam1Turn ? Team2 : Team1;

            string currentPlayerId = GetCurrentPlayerId();
            
            bool hit = FireAtOpponent(opponentTeam.Board, row: 3, col: 4);

            if (hit)
            {
               
            }
            else
            {
                
            }

            if (opponentTeam.HasLost)
            {
                GameOver = true;
                return;
            }

            SwitchTurn();
        }

        private Board PlaceShips(List<(Ship, Square)> shipTypes)
        {
            Board board = new Board();
            board.RandomlyPlaceShips(shipTypes);
            return board;
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
