namespace SharedLibrary
{
    public class GridAdapter : IGridAdapter
    {
        private Square[,] _grid;
        private int _size;

        public GridAdapter(int size)
        {
            _size = size;
            _grid = new Square[size, size];
        }

        public Square[,] GetArrayGrid()
        {
            return _grid;
        }

        public List<List<Square>> GetListGrid()
        {
            var listGrid = new List<List<Square>>();
            for (int row = 0; row < _size; row++)
            {
                var rowList = new List<Square>();
                for (int col = 0; col < _size; col++)
                {
                    rowList.Add(_grid[row, col]);
                }
                listGrid.Add(rowList);
            }
            return listGrid;
        }

        public void SetGrid(Square[,] grid)
        {
            _grid = grid;
        }
    }
}