using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Interpreter
{
    public static class StrategyFactory
    {
        public static IStrategyExpression Create(string strategyName)
        {
            return strategyName switch
            {
                "Random" => new RandomPlacementExpression(),
                "Edge" => new EdgePlacementExpression(),
                "Spaced" => new SpacedPlacementExpression(),
                _ => throw new ArgumentException("Invalid strategy.")
            };
        }
    }
}
