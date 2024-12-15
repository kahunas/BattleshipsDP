using SharedLibrary.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.ChainOfResponsibility
{
    // Abstract Handler
    public abstract class ShotHandler
    {
        protected ShotHandler _nextHandler;

        public void SetNext(ShotHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract IShotCollection Handle(string shotType);
    }

    // Concrete Handlers for each shot type
    public class SimpleShotHandler : ShotHandler
    {
        public override IShotCollection Handle(string shotType)
        {
            if (shotType == "Simple")
            {
                return new SimpleShot();
            }
            return _nextHandler?.Handle(shotType);
        }
    }

    public class BigShotHandler : ShotHandler
    {
        public override IShotCollection Handle(string shotType)
        {
            if (shotType == "Big")
            {
                return new BigShot();
            }
            return _nextHandler?.Handle(shotType);
        }
    }

    public class SlasherShotHandler : ShotHandler
    {
        public override IShotCollection Handle(string shotType)
        {
            if (shotType == "Slasher")
            {
                return new SlasherShot();
            }
            return _nextHandler?.Handle(shotType);
        }
    }

    public class PiercerShotHandler : ShotHandler
    {
        public override IShotCollection Handle(string shotType)
        {
            if (shotType == "Piercer")
            {
                return new PiercerShot();
            }
            return _nextHandler?.Handle(shotType);
        }
    }

    public class CrossShotHandler : ShotHandler
    {
        public override IShotCollection Handle(string shotType)
        {
            if (shotType == "Cross")
            {
                return new CrossShot();
            }
            return _nextHandler?.Handle(shotType);
        }
    }

}
