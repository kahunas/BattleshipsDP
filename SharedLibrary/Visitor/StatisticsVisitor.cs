using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Visitor
{
    public class TeamAStatisticsVisitor : IVisitor
    {
        public int Hits { get; private set; } = 0;
        public int Misses { get; private set; } = 0;
        public int PlayerActions { get; private set; } = 0;

        public Dictionary<string, int> PlayerActionCount { get; private set; } = new Dictionary<string, int>();

        public void Visit(Board board)
        {
            // Optionally, track board-level statistics here
        }

        public void Visit(Ship ship)
        {
            if (ship.Health < ship.Size && ship.Health != 0)
            {
                Hits++;
            }
        }

        public void Visit(Player player)
        {
            if (!PlayerActionCount.ContainsKey(player.Name))
            {
                PlayerActionCount[player.Name] = 0;
            }

            PlayerActionCount[player.Name] += player.ActionsCount;
            PlayerActions += player.ActionsCount;
        }
    }

    public class TeamBStatisticsVisitor : IVisitor
    {
        public int Hits { get; private set; } = 0;
        public int Misses { get; private set; } = 0;
        public int PlayerActions { get; private set; } = 0;

        public Dictionary<string, int> PlayerActionCount { get; private set; } = new Dictionary<string, int>();

        public void Visit(Board board)
        {
            // Optionally, track board-level statistics here
        }

        public void Visit(Ship ship)
        {
            if (ship.Health < ship.Size && ship.Health != 0)
            {
                Hits++;
            }
        }

        public void Visit(Player player)
        {
            if (!PlayerActionCount.ContainsKey(player.Name))
            {
                PlayerActionCount[player.Name] = 0;
            }

            PlayerActionCount[player.Name] += player.ActionsCount;
            PlayerActions += player.ActionsCount;
        }
    }

}
