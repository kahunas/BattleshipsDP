using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Visitor
{
    public class TeamAStatisticsVisitor : IVisitor
    {
        public int Hits { get; set; }
        public int Misses { get; set; }
        public int PlayerActions { get; set; }


        public TeamAStatisticsVisitor() {
            Hits = 0;
            Misses = 0;
            PlayerActions = 0;
        }
        public TeamAStatisticsVisitor(int hits, int misses, int playerActions)
        {
            Hits = hits;
            Misses = misses;
            PlayerActions = playerActions;
        }

        public void Visit(Board board)
        {
            int miss = 0;
            foreach (var row in board.Grid)
            {
                foreach (var cell in row)
                {
                    if (cell == Square.Miss)
                    {
                        miss += 1;
                    }
                }
            }
            Misses = miss;
            Hits = 0;
        }

        public void Visit(Ship ship)
        {
            Hits += ship.Size - ship.Health;
        }

        public void Visit(Player player)
        {
            this.PlayerActions++;
        }
    }

    public class TeamBStatisticsVisitor : IVisitor
    {
        public int Hits { get; set; }
        public int Misses { get; set; }
        public int PlayerActions { get; set; }

        public TeamBStatisticsVisitor()
        {
            Hits = 0;
            Misses = 0;
            PlayerActions = 0;
        }

        public TeamBStatisticsVisitor(int hits, int misses, int playerActions)
        {
            Hits = hits;
            Misses = misses;
            PlayerActions = playerActions;
        }

        public void Visit(Board board)
        {
            int miss = 0;
            foreach (var row in board.Grid)
            {
                foreach (var cell in row)
                {
                    if (cell == Square.Miss)
                    {
                        miss += 1;
                    }
                }
            }
            Misses = miss;
            Hits = 0;
        }

        public void Visit(Ship ship)
        {
            Hits += ship.Size - ship.Health;
        }

        public void Visit(Player player)
        {
            this.PlayerActions++;
        }
    }

}
