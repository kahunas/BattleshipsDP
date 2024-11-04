using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Factory
{
    public abstract class ShipCreator
    {
        public abstract Ship GetShip();
    }

    public class DestroyerCreator : ShipCreator
    {
        public override Ship GetShip() => new Destroyer();
    }

    public class SubmarineCreator : ShipCreator
    {
        public override Ship GetShip() => new Submarine();
    }

    public class BattleshipCreator : ShipCreator
    {
        public override Ship GetShip() => new Battleship();
    }

    public class CarrierCreator : ShipCreator
    {
        public override Ship GetShip() => new Carrier();
    }
}
