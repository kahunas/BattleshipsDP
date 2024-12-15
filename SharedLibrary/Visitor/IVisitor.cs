using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Visitor
{
    public interface IVisitor
    {
        void Visit(Board board);
        void Visit(Ship ship);
        void Visit(Player player);
    }
}
