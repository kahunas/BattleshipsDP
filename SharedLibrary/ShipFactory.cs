using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class BlueShipFactory : IShipFactory
    {
        public Ship CreateDestroyer(string name = "Destroyer")
        {
            return new Destroyer(name, 2);
        }
        public Ship CreateSubmarine(string name = "Submarine")
        {
            return new Submarine(name, 3);
        }
        public Ship CreateBattleship(string name = "Battleship")
        {
            return new Battleship(name, 4);
        }
        public Ship CreateCarrier(string name = "Carrier")
        {
            return new Carrier(name, 5);
        }
    }

    public class RedShipFactory : IShipFactory
    {
        public Ship CreateDestroyer(string name = "Destroyer")
        {
            return new Destroyer(name, 2);
        }
        public Ship CreateSubmarine(string name = "Submarine")
        {
            return new Submarine(name, 3);
        }
        public Ship CreateBattleship(string name = "Battleship")
        {
            return new Battleship(name, 4);
        }
        public Ship CreateCarrier(string name = "Carrier")
        {
            return new Carrier(name, 5);
        }
    }
}
