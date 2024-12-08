namespace SharedLibrary
{
    public interface IMediator
    {
        void RegisterGameRoom(GameRoom gameRoom);
        void AddPlayerToRoom(string roomId, Player player);
        void StartGame(string roomId);
        void NotifyPlayerReady(string roomId, string playerId);
    }
}