// Add this to a new file BattleshipsTurnHandler.cs
using SharedLibrary;
using SharedLibrary.Bridge;

public class BattleshipsTurnHandler : TurnHandler
{
    public BattleshipsTurnHandler(BattleshipsGame game) : base(game)
    {
    }

    protected override bool ValidatePlayerTurn(string playerId)
    {
        return _game.CurrentPlayerId == playerId;
    }

    protected override bool ValidateShot(int row, int col, string shotType)
    {
        // Validate coordinates are within board bounds
        if (row < 0 || row >= _game.boardSize || col < 0 || col >= _game.boardSize)
            return false;

        // Get the team and validate shot availability
        var team = _game.GetTeamByPlayer(_game.CurrentPlayerId) == "Team A" ? _game.ATeam : _game.BTeam;
        
        if (shotType != "Simple")
        {
            IShotCollection shot = shotType switch
            {
                "Big" => new BigShot(),
                "Piercer" => new PiercerShot(),
                "Slasher" => new SlasherShot(),
                "Cross" => new CrossShot(),
                _ => null
            };

            if (shot == null || !team.TakeShot(shot.GetType()))
                return false;
        }

        return true;
    }

    protected override void ProcessShot(int row, int col, string shotType, string playerId)
    {
        bool isGameOver;
        _game.ShootCell(row, col, playerId, out isGameOver);
    }

    protected override bool CheckGameOver()
    {
        if (_game.ATeamBoard.AllShipsDestroyed() || _game.BTeamBoard.AllShipsDestroyed())
        {
            _game.GameOver = true;
            return true;
        }
        return false;
    }

    protected override void UpdateNextPlayer()
    {
        if (_game.CurrentPlayerId == _game.ATeamPlayer1Id)
            _game.CurrentPlayerId = _game.ATeamPlayer2Id;
        else if (_game.CurrentPlayerId == _game.ATeamPlayer2Id)
            _game.CurrentPlayerId = _game.BTeamPlayer1Id;
        else if (_game.CurrentPlayerId == _game.BTeamPlayer1Id)
            _game.CurrentPlayerId = _game.BTeamPlayer2Id;
        else if (_game.CurrentPlayerId == _game.BTeamPlayer2Id)
            _game.CurrentPlayerId = _game.ATeamPlayer1Id;
    }
}