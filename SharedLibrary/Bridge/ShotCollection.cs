using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Builder;

namespace SharedLibrary.Bridge
{
    public interface IShotCollection
    {
        public IShot shot { get; }
        public int Amount { get; set; }
        public List<(int, int)> GetSpread(int row, int col);
    }
    public class BigShot : IShotCollection
    {
        public IShot shot { get; }
        public int Amount { get; set; }
        public BigShot(int count = 0)
        {
            shot = InitializeShot();
            this.Amount = count;
        }

        private IShot InitializeShot()
        {
            IShotBuilder builder = new ShotBuilder();
            return builder.SetName("Big").SetSpread(new List<(int, int)>
        {
            (0, 0), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1)
        }).Build();
        }

        public List<(int, int)> GetSpread(int row, int col)
        {
            return shot.ShotCoordinates(row, col);
        }
    }

    public class PiercerShot : IShotCollection
    {
        public IShot shot { get; }
        public int Amount { get; set; }
        public PiercerShot(int count = 0)
        {
            shot = InitializeShot();
            this.Amount = count;
        }

        private IShot InitializeShot() {
            IShotBuilder builder = new ShotBuilder();
            return builder.SetName("Piercer").SetSpread(new List<(int, int)>
            {
                (0, 0), (-1, 0), (-2, 0), (-3, 0)
            }).Build();
        }
        public List<(int, int)> GetSpread(int row, int col)
        {
            return shot.ShotCoordinates(row, col);
        }
    }

    public class SlasherShot : IShotCollection
    {
        public IShot shot { get; }
        public int Amount { get; set; }
        public SlasherShot(int count = 0)
        {
            shot = InitializeShot();
            this.Amount = count;
        }

        private IShot InitializeShot()
        {
            IShotBuilder builder = new ShotBuilder();
            return builder.SetName("Slasher").SetSpread(new List<(int, int)>
            {
                (0, 0), (0, 1), (0, 2), (0, 3)
            }).Build();
        }
        public List<(int, int)> GetSpread(int row, int col)
        {
            return shot.ShotCoordinates(row, col);
        }
    }

    public class CrossShot : IShotCollection
    {
        public IShot shot { get; }
        public int Amount { get; set; }
        public CrossShot(int count = 0)
        {
            shot = InitializeShot();
            this.Amount = count;
        }

        private IShot InitializeShot()
        {
            IShotBuilder builder = new ShotBuilder();
            return builder.SetName("Cross").SetSpread(new List<(int, int)>
            {
                (0, 0), (1, 0), (-1, 0), (0, 1), (0, -1)
            }).Build();
        }
        public List<(int, int)> GetSpread(int row, int col)
        {
            return shot.ShotCoordinates(row, col);
        }
    }

    public class SimpleShot : IShotCollection
    {
        public IShot shot { get; }
        public int Amount { get; set; }
        public SimpleShot(int count = 0)
        {
            shot = InitializeShot();
            this.Amount = count;
        }

        private IShot InitializeShot()
        {
            IShotBuilder builder = new ShotBuilder();
            return builder.SetName("Simple").SetSpread(new List<(int, int)> { (0, 0) }).Build();
        }
        public List<(int, int)> GetSpread(int row, int col)
        {
            return shot.ShotCoordinates(row, col);
        }
    }
}
