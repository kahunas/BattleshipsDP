using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Factory
{
    public class EasyBoard : Board
    {
        public EasyBoard(int size = 8) : base(size)
        {

        }
    }

    public class MediumBoard : Board
    {
        public MediumBoard(int size = 10) : base(size)
        {

        }
    }

    public class HardBoard : Board
    {
        public HardBoard(int size = 12) : base(size)
        {

        }
    }
}
