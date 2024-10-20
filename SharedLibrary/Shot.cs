using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Shot
    {
        public string Name { get; set; }
        List<(int, int)> Spread { get; set; }

        public Shot(string name, List<(int, int)> spread)
        {
            Name = name;
            Spread = spread;
        }
        
        public List<(int, int)> ShotCoordinates(int row, int col)
        {
            List<(int, int)> coordinates = new List<(int, int)> ();

            foreach ((int, int) spread in Spread)
            {
                if (row + spread.Item1 >= 0 && col + spread.Item2 >= 0)
                {
                    (int, int) coordinate = (row + spread.Item1, col + spread.Item2);
                    coordinates.Add(coordinate);
                }
            }
            return coordinates;
        }
    }

}
