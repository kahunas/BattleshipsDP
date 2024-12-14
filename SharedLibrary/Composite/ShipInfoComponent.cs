using System;
using System.Collections.Generic;

namespace SharedLibrary.Composite
{
    // Base Component
    public abstract class ShipInfoComponent
    {
        protected string name;

        public ShipInfoComponent(string name)
        {
            this.name = name;
        }

        public abstract List<int[]> GetCoordinates(IEnumerable<Ship> ships);
        public abstract object GetStatus(IEnumerable<Ship> ships);
        public virtual void Add(ShipInfoComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(ShipInfoComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsComposite()
        {
            return false;
        }
    }

    // Leaf - Individual Ship
    public class IndividualShipInfo : ShipInfoComponent
    {
        private readonly int shipNumber;

        public IndividualShipInfo(string shipType, int number) : base($"{shipType}-{number}")
        {
            this.shipNumber = number - 1;
        }

        public override List<int[]> GetCoordinates(IEnumerable<Ship> ships)
        {
            var matchingShips = ships.Where(s => s.Name.ToLower() == name.Split('-')[0]).ToList();
            if (shipNumber < matchingShips.Count)
            {
                return matchingShips[shipNumber].Coordinates
                    .Select(c => new int[] { c.Item1, c.Item2 })
                    .ToList();
            }
            return new List<int[]>();
        }

        public override object GetStatus(IEnumerable<Ship> ships)
        {
            var matchingShips = ships.Where(s => s.Name.ToLower() == name.Split('-')[0]).ToList();
            if (shipNumber < matchingShips.Count)
            {
                var ship = matchingShips[shipNumber];
                var status = ship.GetStatus();
                return new
                {
                    LocationInfo = status.Location,
                    Health = status.Health,
                    MaxHealth = status.MaxHealth,
                    IsActive = status.IsActive
                };
            }
            return null;
        }
    }

    // Composite - Ship Category
    public class ShipCategoryInfo : ShipInfoComponent
    {
        private readonly List<ShipInfoComponent> children = new();

        public ShipCategoryInfo(string categoryName) : base(categoryName) { }

        public override List<int[]> GetCoordinates(IEnumerable<Ship> ships)
        {
            var coordinates = new List<int[]>();
            foreach (var child in children)
            {
                coordinates.AddRange(child.GetCoordinates(ships));
            }
            return coordinates;
        }

        public override object GetStatus(IEnumerable<Ship> ships)
        {
            return children.Select(child => child.GetStatus(ships)).ToList();
        }

        public override void Add(ShipInfoComponent component)
        {
            children.Add(component);
        }

        public override void Remove(ShipInfoComponent component)
        {
            children.Remove(component);
        }

        public override bool IsComposite()
        {
            return true;
        }
    }

    // Composite - Navy (Root)
    public class NavyInfo : ShipInfoComponent
    {
        private readonly List<ShipInfoComponent> categories = new();

        public NavyInfo() : base("navy") { }

        public override List<int[]> GetCoordinates(IEnumerable<Ship> ships)
        {
            return ships.SelectMany(s => s.Coordinates)
                .Select(c => new int[] { c.Item1, c.Item2 })
                .ToList();
        }

        public override object GetStatus(IEnumerable<Ship> ships)
        {
            return categories.Select(category => category.GetStatus(ships)).ToList();
        }

        public override void Add(ShipInfoComponent component)
        {
            categories.Add(component);
        }

        public override void Remove(ShipInfoComponent component)
        {
            categories.Remove(component);
        }

        public override bool IsComposite()
        {
            return true;
        }
    }
}
