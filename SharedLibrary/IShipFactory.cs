using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IShipFactory
    {
        Ship CreateDestroyer(string name);
        Ship CreateSubmarine(string name);
        Ship CreateBattleship(string name);
        Ship CreateCarrier(string name);
    }
}
