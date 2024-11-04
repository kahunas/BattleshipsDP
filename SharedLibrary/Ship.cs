using System;
using System.Collections.Generic;
using System.Linq;
using SharedLibrary.Bridge;
using SharedLibrary.Builder;

namespace SharedLibrary
{
    public abstract class Ship
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


        //// Uses a specific type of shot by name, if available
        //public void UseShot(string shotName)
        //{
        //    var shot = SpecialShots.FirstOrDefault(s => s.Name == shotName);
        //    if (shot != null && shot.GetRemainingUses() > 0)
        //    {
        //        shot.UseShot();
        //    }
        //}
        //
        //// Retrieves the available special shots with remaining uses for display or selection
        //public IEnumerable<IShotCollection> GetAvailableShots()
        //{
        //    return SpecialShots.Where(s => s.GetRemainingUses() > 0);
        //}
    }
}