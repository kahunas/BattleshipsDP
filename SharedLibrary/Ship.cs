using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Ship
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        public List<(int, int)> Coordinates { get; set; }

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
        }

        public void Place(List<(int, int)> coordinates)
        {
            Coordinates = coordinates;
        }

        public bool Hit(int row, int col)
        {
            var coordinate = (row, col);
            if (Coordinates.Contains(coordinate))
            {
                Health--;
                Coordinates.Remove(coordinate);
                return true;
            }
            return false;
        }

        public bool IsDestroyed()
        {
            return Health == 0;
        }
    }

}
