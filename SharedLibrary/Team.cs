using SharedLibrary.Bridge;
using SharedLibrary.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Team
    {
        public string Name { get; set; }
        public Board Board { get; set; }
        public List<Player> Players { get; set; }
        public List<IShotCollection> ShotCollection { get; set; }

        public Team(string name)
        {
            Name = name;
            Board = new MediumBoard();
            Players = new List<Player>();
            ShotCollection = new List<IShotCollection> {
                new SimpleShot(),
            new BigShot(),
            new SlasherShot(),
            new PiercerShot(),
            new CrossShot()};
        }


        public void AddShots(Type shotType, IShotCollection shot)
        {
            for (int i = 0; i < ShotCollection.Count; i++)
            {
                if (ShotCollection[i].GetType().Equals(shotType))
                {
                    ShotCollection[i].Amount += shot.Amount;
                    break;
                }
            }
        }

        public bool TakeShot(Type shotType)
        {
            for (int i = 0; i < ShotCollection.Count; i++)
            {
                if (ShotCollection[i].GetType().Equals(shotType) && ShotCollection[i].Amount > 0)
                {
                    ShotCollection[i].Amount--;
                    return true;
                }
            }
            return false;
        }

        public int RemainingShots(Type shotType)
        {
            for (int i = 0; i < ShotCollection.Count; i++)
            {
                if (ShotCollection[i].GetType().Equals(shotType))
                {
                    return ShotCollection[i].Amount;
                }
            }
            return -1;
        }
    }
}
