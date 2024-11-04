using SharedLibrary.Bridge;
using SharedLibrary.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Factory
{

    public class Destroyer : Ship
    {
        public Destroyer(string name = "Destroyer", int size = 2) : base(name, size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
            HitCoordinates = new List<(int, int)>();

            // Assign specific shots and their quantities for the Destroyer
            AddBigShot(1);
            AddSlasherShot(2);
            AddPiercerShot(3);
            AddCrossShot(4);
        }
    }

    public class Submarine : Ship
    {
        public Submarine(string name = "Submarine", int size = 3) : base(name, size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
            HitCoordinates = new List<(int, int)>();

            // Assign specific shots and their quantities for the Submarine
            AddSlasherShot(2);
            AddPiercerShot(3);
            AddCrossShot(4);
        }
    }

    public class Battleship : Ship
    {
        public Battleship(string name = "Battleship", int size = 4) : base(name, size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
            HitCoordinates = new List<(int, int)>();

            // Assign specific shots and their quantities for the Battleship
            AddSlasherShot(2);
            AddPiercerShot(3);
            AddCrossShot(4);
        }
    }

    public class Carrier : Ship
    {
        public Carrier(string name = "Carrier", int size = 5) : base(name, size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
            HitCoordinates = new List<(int, int)>();

            // Assign specific shots and their quantities for the Carrier
            AddSlasherShot(2);
            AddPiercerShot(3);
            AddCrossShot(4);
        }
    }
}
