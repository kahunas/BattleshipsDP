using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class StandartFieldFactory : IBoard
    {
        public Board CreateBoard(int size) => new StandartBoard(size);
    }

    public class MediumFieldFactory : IBoard
    {
        public Board CreateBoard(int size) => new MediumBoard(size);
    }

    public class AdvancedFieldFactory : IBoard
    {
        public Board CreateBoard(int size) => new AdvancedBoard(size);
    }
}
