using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Builder
{
    public class ShotBuilder : IShotBuilder
    {
        private string _name;
        private List<(int, int)> _spread;

        public IShotBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public IShotBuilder SetSpread(List<(int, int)> spread)
        {
            _spread = spread;
            return this;
        }

        public Shot Build()
        {
            return new Shot(_name, _spread);
        }
    }
}
