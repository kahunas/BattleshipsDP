using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Builder
{
    public interface IShotBuilder
    {
        IShotBuilder SetName(string name);
        IShotBuilder SetSpread(List<(int, int)> spread);
        Shot Build();
    }
}
