using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.State
{
    public interface IGameState
    {
        void EnterState(BattleshipsGame game);
        void ExecuteState(BattleshipsGame game);
        void ExitState(BattleshipsGame game);
    }
}
