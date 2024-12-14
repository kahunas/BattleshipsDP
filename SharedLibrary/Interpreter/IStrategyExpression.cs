using SharedLibrary.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Interpreter
{
    public interface IStrategyExpression
    {
        void PlaceShips(Board board, List<(Ship, Square)> ships);
    }

    public class RandomPlacementExpression : IStrategyExpression
    {
        public void PlaceShips(Board board, List<(Ship, Square)> ships)
        {
            var strategy = new RandomPlacementStrategy();
            strategy.PlaceShips(board, ships);

        }
    }

    public class EdgePlacementExpression : IStrategyExpression
    {
        public void PlaceShips(Board board, List<(Ship, Square)> ships)
        {
            var strategy = new EdgePlacementStrategy();
            strategy.PlaceShips(board, ships);
        }
    }

    public class SpacedPlacementExpression : IStrategyExpression
    {
        public void PlaceShips(Board board, List<(Ship, Square)> ships)
        {
            var strategy = new SpacedPlacementStrategy();
            strategy.PlaceShips(board, ships);
        }
    }
}
