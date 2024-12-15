using SharedLibrary.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class GameRoom
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public List<Player> Players { get; set; }
        public BattleshipsGame Game { get; set; }
        public string Difficulty { get; set; }

        // Add these new statistics visitors
        public TeamAStatisticsVisitor TeamAStatisticsVisitor { get; set; } = new TeamAStatisticsVisitor();
        public TeamBStatisticsVisitor TeamBStatisticsVisitor { get; set; } = new TeamBStatisticsVisitor();


        public GameRoom(string roomId, string roomName, string difficulty = "Medium")
        {
            RoomId = roomId;
            RoomName = roomName;
            Players = new List<Player>();
            Game = new BattleshipsGame();
            Difficulty = difficulty;
            Game.SetGameDifficulty(difficulty);
        }

        public GameRoom()
        {
            RoomId = string.Empty;
            RoomName = string.Empty;
            Players = new List<Player>();
            Game = new BattleshipsGame();
        }
    
        public bool TryAddPlayer(Player player)
        {
            if (Players.Count < 4 && !Players.Any(p => p.ConnectionId == player.ConnectionId))
            {
                Players.Add(player);
                switch (Players.Count)
                {
                    case 1:
                        Game.ATeamPlayer1Id = player.ConnectionId;
                        break;
                    case 2:
                        Game.ATeamPlayer2Id = player.ConnectionId;
                        break;
                    case 3:
                        Game.BTeamPlayer1Id = player.ConnectionId;
                        break;
                    case 4:
                        Game.BTeamPlayer2Id = player.ConnectionId;
                        break;
                    default:
                        break;
                }
                return true;
            }
            return false;
        }
    }
}
