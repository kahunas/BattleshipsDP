using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Player : ICloneable
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }

        public Player(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }

        public object Clone()
        {
            // Return a new Player instance with the same values
            return new Player(this.ConnectionId, this.Name) { IsReady = this.IsReady };
        }

    }
}
