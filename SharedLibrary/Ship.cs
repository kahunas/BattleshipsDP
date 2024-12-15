using System;
using System.Collections.Generic;
using System.Linq;
using SharedLibrary.Bridge;
using SharedLibrary.Builder;
using SharedLibrary.Visitor;

namespace SharedLibrary
{
    public abstract class Ship : IVisitable
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        public List<(int, int)> Coordinates { get; set; }
        public List<(int, int)> HitCoordinates { get; set; }

        // List of special shots each ship can use, implementing the Bridge pattern
        public List<IShotCollection> SpecialShots { get; }

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
            HitCoordinates = new List<(int, int)>();
            SpecialShots = new List<IShotCollection> {
                new SimpleShot(),
            new BigShot(),
            new SlasherShot(),
            new PiercerShot(),
            new CrossShot()};
        }

        // Method to place the ship at specific coordinates on the board
        public void Place(List<(int, int)> coordinates)
        {
            Coordinates = coordinates;
        }

        // Method to handle a hit on the ship at specified coordinates
        public bool Hit(int row, int col)
        {
            var coordinate = (row, col);

            if (Coordinates.Contains(coordinate) && !HitCoordinates.Contains(coordinate))
            {
                HitCoordinates.Add(coordinate);
                Health--;
                
                Console.WriteLine($"Ship hit at ({row}, {col}) - Remaining health: {Health}");
                Console.WriteLine($"Remaining Coordinates: {string.Join(", ", Coordinates)}");
                Console.WriteLine($"Hit Coordinates: {string.Join(", ", HitCoordinates)}");

                if (IsDestroyed())
                {
                    Console.WriteLine($"Ship {Name} is destroyed!");
                }

                return true;
            }

            return false;
        }

        // Checks if the ship is destroyed
        public bool IsDestroyed()
        {
            return Health == 0;
        }

        public string GetStatusInfo()
        {
            if (IsDestroyed())
                return $"{Name} has been destroyed!";
            return $"{Name} hit! {Health}/{Size} health remaining";
        }

        public void AddBigShot(int amount)
        {
            for (int i = 0; i < SpecialShots.Count; i++)
            {
                if (SpecialShots[i] is BigShot)
                {
                    SpecialShots[i].Amount += amount ;
                    break;
                }
            }
        }

        public void AddPiercerShot(int amount)
        {
            for (int i = 0; i < SpecialShots.Count; i++)
            {
                if (SpecialShots[i] is PiercerShot)
                {
                    SpecialShots[i].Amount += amount;
                    break;
                }
            }
        }

        public void AddSlasherShot(int amount)
        {
            for (int i = 0; i < SpecialShots.Count; i++)
            {
                if (SpecialShots[i] is SlasherShot)
                {
                    SpecialShots[i].Amount += amount;
                    break;
                }
            }
        }

        public void AddCrossShot(int amount)
        {
            for (int i = 0; i < SpecialShots.Count; i++)
            {
                if (SpecialShots[i] is CrossShot)
                {
                    SpecialShots[i].Amount += amount;
                    break;
                }
            }
        }

        public string GetLocationInfo()
        {
            if (Coordinates.Count == 0) return "Not placed";
            
            var start = Coordinates[0];
            var end = Coordinates[^1];
            return $"From {(char)('A' + start.Item1)}{start.Item2 + 1} to {(char)('A' + end.Item1)}{end.Item2 + 1}";
        }

        public (string Location, int Health, int MaxHealth, bool IsActive) GetStatus()
        {
            return (GetLocationInfo(), Health, Size, Health > 0);
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}