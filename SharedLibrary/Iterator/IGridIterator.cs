using System.Collections;

namespace SharedLibrary.Iterator
{
    public interface IGridIterator
    {
        bool HasNext();
        GridCell Next();
        void Reset();
        void SkipEmpty();
        void SetFilter(Func<GridCell, bool> filter);
    }
}
