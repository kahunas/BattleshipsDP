using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Factory
{
    public abstract class LevelFactory
    {
        public abstract Board GetBoard();
        public abstract List<(Ship, Square)> GetShips();

    }

    public class EasyFactory : LevelFactory
    {
        public override Board GetBoard() => new EasyBoard();
        public override List<(Ship, Square)> GetShips() => new EasyShipCollection().GetShipCollection();
    }

    public class MediumFactory : LevelFactory
    {
        public override Board GetBoard() => new MediumBoard();
        public override List<(Ship, Square)> GetShips() => new MediumShipCollection().GetShipCollection();
    }

    public class HardFactory : LevelFactory
    {
        public override Board GetBoard() => new HardBoard();
        public override List<(Ship, Square)> GetShips() => new HardShipCollection().GetShipCollection();
    }
}
