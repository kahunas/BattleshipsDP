using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{

    public class Destroyer : Ship
    {
        public Destroyer(string name, int size = 2) : base(name, size)
        {
            this.Name = name;
            this.Size = size;
            this.Health = size;
            this.Coordinates = new List<(int, int)>();
            this.HitCoordinates = new List<(int, int)>();
        }
    }
    public class Submarine : Ship
    {
        public Submarine(string name, int size = 3) : base(name, size)
        {
            this.Name = name;
            this.Size = size;
            this.Health = size;
            this.Coordinates = new List<(int, int)>();
            this.HitCoordinates = new List<(int, int)>();
        }
    }
    public class Battleship : Ship
    {
        public Battleship(string name, int size = 4) : base(name, size)
        {
            this.Name = name;
            this.Size = size;
            this.Health = size;
            this.Coordinates = new List<(int, int)>();
            this.HitCoordinates = new List<(int, int)>();
        }
    }
    public class Carrier : Ship
    {
        public Carrier(string name, int size = 5) : base(name, size)
        {
            this.Name = name;
            this.Size = size;
            this.Health = size;
            this.Coordinates = new List<(int, int)>();
            this.HitCoordinates = new List<(int, int)>();
        }
    }
}
