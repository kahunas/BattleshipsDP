namespace SharedLibrary
{
    public interface IGridAdapter
    {
        Square[,] GetArrayGrid();  // Returns 2D array format
        List<List<Square>> GetListGrid();  // Returns nested list format (serializable)
        void SetGrid(Square[,] grid);
    }
}