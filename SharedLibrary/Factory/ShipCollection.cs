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
        private readonly DestroyerCreator destroyer;
        private readonly SubmarineCreator submarine;
        private readonly BattleshipCreator battleship;
        private readonly CarrierCreator carrier;

        // Constructor to initialize the creators
        public EasyShipCollection()
        {
            destroyer = new DestroyerCreator();
            submarine = new SubmarineCreator();
            battleship = new BattleshipCreator();
            carrier = new CarrierCreator();
        }
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
        private readonly DestroyerCreator destroyer;
        private readonly SubmarineCreator submarine;
        private readonly BattleshipCreator battleship;
        private readonly CarrierCreator carrier;

        public MediumShipCollection()
        {
            destroyer = new DestroyerCreator();
            submarine = new SubmarineCreator();
            battleship = new BattleshipCreator();
            carrier = new CarrierCreator();
        }

        public List<(Ship, Square)> GetShipCollection()
        {
            return new List<(Ship, Square)>
            {
                (destroyer.GetShip(), Square.Ship),
                (destroyer.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (battleship.GetShip(), Square.Ship),
                (carrier.GetShip(), Square.Ship)
            };
        }
    }

    public class HardShipCollection : IShipCollection
    {
        private readonly DestroyerCreator destroyer;
        private readonly SubmarineCreator submarine;
        private readonly BattleshipCreator battleship;
        private readonly CarrierCreator carrier;

        public HardShipCollection()
        {
            destroyer = new DestroyerCreator();
            submarine = new SubmarineCreator();
            battleship = new BattleshipCreator();
            carrier = new CarrierCreator();
        }

        public List<(Ship, Square)> GetShipCollection()
        {
            return new List<(Ship, Square)>
            {
                (destroyer.GetShip(), Square.Ship),
                (destroyer.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (submarine.GetShip(), Square.Ship),
                (battleship.GetShip(), Square.Ship),
                (battleship.GetShip(), Square.Ship),
                (carrier.GetShip(), Square.Ship),
                (carrier.GetShip(), Square.Ship)
            };
        }
    }
}
