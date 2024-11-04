using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Builder
{
    public interface IShot
    {
        string Name { get; }
        List<(int, int)> Spread { get; }
        List<(int, int)> ShotCoordinates(int row, int col);
    }
}
