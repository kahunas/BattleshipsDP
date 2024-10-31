using SharedLibrary;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipsDP.Client
{
    public class CommandInvoker
    {
        private readonly List<ShotCommand> _commandHistory = new();

        public void ExecuteCommand(ShotCommand command, List<List<GridCell>> board)
        {
            var existingCommand = _commandHistory.FirstOrDefault(
                c => c.Row == command.Row && c.Col == command.Col && c.IsPlayerShot != command.IsPlayerShot
            );

            if (existingCommand == null)
            {
                command.Execute(board);
                _commandHistory.Add(command);
            }
            else
            {
                _commandHistory.Remove(existingCommand);
                var updatedCommand = new ShotCommand(existingCommand.Row, existingCommand.Col, command.ShotType, existingCommand.IsPlayerShot);
                updatedCommand.Execute(board);
                _commandHistory.Add(updatedCommand);
            }
        }


        //public void UndoLastCommand(List<List<GridCell>> board)
        //{
        //    if (_commandHistory.Count > 0)
        //    {
        //        var lastCommand = _commandHistory.Last();
        //        lastCommand.Undo(board);
        //        _commandHistory.Remove(lastCommand);
        //    }
        //}

        public IEnumerable<ShotCommand> GetHistory() => _commandHistory;
    }
}
