using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Factory
{
    public interface IShipCollection
    {
        List<(Ship, Square)> GetShipCollection();
    }

    public class EasyShipCollection : IShipCollection
    {
        private DestroyerCreator destroyer;
        private SubmarineCreator submarine;
        private BattleshipCreator battleship;
        private CarrierCreator carrier;
        public List<(Ship, Square)> GetShipCollection()
        {
            return new List<(Ship, Square)>
            {
                (destroyer.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (battleship.GetShip(), Square.Ship),
                (battleship.GetShip(), Square.Ship)
            };
        }
    }

    public class MediumShipCollection : IShipCollection
    {
        private DestroyerCreator destroyer;
        private SubmarineCreator submarine;
        private BattleshipCreator battleship;
        private CarrierCreator carrier;
        public List<(Ship, Square)> GetShipCollection()
        {
            return new List<(Ship, Square)>
            {
                (destroyer.GetShip(), Square.Ship),
                (destroyer.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (battleship.GetShip(), Square.Ship),
                (new Carrier(), Square.Ship)
            };
        }
    }

    public class HardShipCollection : IShipCollection
    {
        private DestroyerCreator destroyer;
        private SubmarineCreator submarine;
        private BattleshipCreator battleship;
        private CarrierCreator carrier;
        public List<(Ship, Square)> GetShipCollection()
        {
            return new List<(Ship, Square)>
            {
                (new Destroyer(), Square.Ship),
                (new Destroyer(), Square.Ship),
                (new Submarine(), Square.Ship),
                (new Submarine(), Square.Ship),
                (new Battleship(), Square.Ship),
                (new Battleship(), Square.Ship),
                (new Carrier(), Square.Ship),
                (new Carrier(), Square.Ship)
            };
        }
    }
}
