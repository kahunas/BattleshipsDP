namespace SharedLibrary.Iterator
{
    public class BoardIterator : IGridIterator
    {
        private readonly Board _board;
        private int _currentRow = 0;
        private int _currentCol = 0;
        private Func<GridCell, bool> _filter = null;

        public BoardIterator(Board board)
        {
            _board = board;
        }

        public bool HasNext()
        {
            while (_currentRow < _board.Size)
            {
                while (_currentCol < _board.Size)
                {
                    var cell = GridCell.Create(_currentRow, _currentCol, _board.Grid[_currentRow][_currentCol]);
                    if (_filter == null || _filter(cell))
                    {
                        return true;
                    }
                    _currentCol++;
                }
                _currentRow++;
                _currentCol = 0;
            }
            return false;
        }

        public GridCell Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements to iterate");

            var cell = GridCell.Create(_currentRow, _currentCol, _board.Grid[_currentRow][_currentCol]);
            Console.WriteLine($"Iterator: Visiting cell [{_currentRow},{_currentCol}] with state {cell.State}");
            
            _currentCol++;
            if (_currentCol >= _board.Size)
            {
                _currentCol = 0;
                _currentRow++;
            }

            return cell;
        }

        public void Reset()
        {
            _currentRow = 0;
            _currentCol = 0;
        }

        public void SkipEmpty()
        {
            SetFilter(cell => cell.State != Square.Empty);
        }

        public void SetFilter(Func<GridCell, bool> filter)
        {
            _filter = filter;
            Reset();
        }
    }
}
