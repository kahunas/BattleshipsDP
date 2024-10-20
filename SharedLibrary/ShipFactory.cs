using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class BlueShipFactory : IShipFactory
    {
        public Ship CreateDestroyer(string name = "Destroyer", int size = 2)
        {
            return new Destroyer(name, size);
        }
        public Ship CreateSubmarine(string name = "Submarine", int size = 3)
        {
            return new Submarine(name, size);
        }
        public Ship CreateBattleship(string name = "Battleship", int size = 4)
        {
            return new Battleship(name, size);
        }
        public Ship CreateCarrier(string name = "Carrier", int size = 5)
        {
            return new Carrier(name, size);
        }
    }

    public class RedShipFactory : IShipFactory
    {
        public Ship CreateDestroyer(string name = "Destroyer", int size = 2)
        {
            return new Destroyer(name, size);
        }
        public Ship CreateSubmarine(string name = "Submarine", int size = 3)
        {
            return new Submarine(name, size);
        }
        public Ship CreateBattleship(string name = "Battleship", int size = 4)
        {
            return new Battleship(name, size);
        }
        public Ship CreateCarrier(string name = "Carrier", int size = 5)
        {
            return new Carrier(name, size);
        }
    }
}
