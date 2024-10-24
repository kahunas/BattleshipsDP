using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Shot : IShot
    {
        public string Name { get; private set; }
        public List<(int, int)> Spread { get; private set; }

        public Shot(string name, List<(int, int)> spread)
        {
            Name = name;
            Spread = spread ?? throw new ArgumentNullException(nameof(spread));
        }

        public List<(int, int)> ShotCoordinates(int row, int col)
        {
            var coordinates = new List<(int, int)>();

            foreach (var spread in Spread)
            {
                int newRow = row + spread.Item1;
                int newCol = col + spread.Item2;

                if (newRow >= 0 && newCol >= 0)
                {
                    coordinates.Add((newRow, newCol));
                }
            }
            return coordinates;
        }
    }

}
