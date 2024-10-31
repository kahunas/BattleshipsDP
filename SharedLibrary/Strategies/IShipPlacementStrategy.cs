namespace SharedLibrary.Strategies
{
    public interface IShipPlacementStrategy
    {
        void PlaceShips(Board board, List<(Ship, Square)> ships);
    }
}
