using System;

namespace SharedLibrary.Template
{
    public class BattleshipsTurnHandler : TurnHandler
    {
        private readonly BattleshipsGame game;

        public BattleshipsTurnHandler(BattleshipsGame game)
        {
            this.game = game;
        }

        protected override bool ValidatePlayer(string playerId)
        {
            return playerId == game.CurrentPlayerId && !game.GameOver;
        }

        protected override bool ValidateMove(int row, int col)
        {
            if (row == -1 && col == -1) return true; // Initial turn
            return row >= 0 && row < game.boardSize && col >= 0 && col < game.boardSize;
        }

        protected override string ProcessMove(string playerId, int row, int col, string shotType)
        {
            if (row == -1 && col == -1) return "initial"; // Initial turn

            bool isGameOver;
            string result = game.ShootCell(row, col, playerId, out isGameOver);
            game.GameOver = isGameOver;
            return result;
        }

        protected override void UpdateGameState(string result)
        {
            if (result == "hit" || result == "miss")
            {
                // Switch to next player's turn
                if (game.CurrentPlayerId == game.ATeamPlayer1Id)
                    game.CurrentPlayerId = game.ATeamPlayer2Id;
                else if (game.CurrentPlayerId == game.BTeamPlayer1Id)
                    game.CurrentPlayerId = game.BTeamPlayer2Id;
                else if (game.CurrentPlayerId == game.ATeamPlayer2Id)
                    game.CurrentPlayerId = game.BTeamPlayer1Id;
                else if (game.CurrentPlayerId == game.BTeamPlayer2Id)
                    game.CurrentPlayerId = game.ATeamPlayer1Id;
            }
        }

        protected override void ProcessNextTurn()
        {
            if (!game.GameOver)
            {
                foreach (var observer in observers)
                {
                    observer.UpdateTurn(game.CurrentPlayerId);
                }
            }
        }
    }
}
