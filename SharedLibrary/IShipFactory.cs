using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IShipFactory
    {
        Ship CreateDestroyer(string name, int size);
        Ship CreateSubmarine(string name, int size);
        Ship CreateBattleship(string name, int size);
        Ship CreateCarrier(string name, int size);
    }
}
