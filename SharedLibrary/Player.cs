using SharedLibrary.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Player : ICloneable, IVisitable
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public bool IsReady { get; set; }
        public bool IsTeamLeader { get; set; }
        public bool IsReadyForBattle { get; set; } // Add new property
        public int ActionsCount { get; private set; }

        public Player(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
            IsReady = false;
            IsTeamLeader = false;
            IsReadyForBattle = false;
            ActionsCount = 0;
        }

        public object Clone()
        {
            // Return a new Player instance with the same values
            return new Player(this.ConnectionId, this.Name) 
            { 
                IsReady = this.IsReady,
                IsTeamLeader = this.IsTeamLeader,
                IsReadyForBattle = this.IsReadyForBattle
            };
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}