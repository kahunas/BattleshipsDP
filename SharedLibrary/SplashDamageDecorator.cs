using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Builder;

namespace SharedLibrary
{
    public class SplashDamageDecorator : IShot
    {
        private readonly IShot _baseShot;
        private readonly int _splashRadius;

        public SplashDamageDecorator(IShot baseShot, int splashRadius = 1)
        {
            _baseShot = baseShot;
            _splashRadius = splashRadius;
        }

        public string Name => $"{_baseShot.Name} with Splash Damage";

        public List<(int, int)> Spread => _baseShot.Spread;

        public List<(int, int)> ShotCoordinates(int row, int col)
        {
            var coordinates = _baseShot.ShotCoordinates(row, col);

            // Add splash effect around each coordinate based on the splash radius
            var splashCoordinates = new HashSet<(int, int)>();
            foreach (var (x, y) in coordinates)
            {
                for (int i = -_splashRadius; i <= _splashRadius; i++)
                {
                    for (int j = -_splashRadius; j <= _splashRadius; j++)
                    {
                        splashCoordinates.Add((x + i, y + j));
                    }
                }
            }

            return splashCoordinates.ToList();
        }
    }

}
