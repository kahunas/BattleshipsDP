using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Ship
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        public List<(int, int)> Coordinates { get; set; }
        public List<(int, int)> HitCoordinates { get; set; } // New list to track hit coordinates

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Health = size;
            Coordinates = new List<(int, int)>();
            HitCoordinates = new List<(int, int)>();
        }

        public void Place(List<(int, int)> coordinates)
        {
            Coordinates = coordinates;
        }

        public bool Hit(int row, int col)
        {
            var coordinate = (row, col);

            if (Coordinates.Contains(coordinate))
            {
                if (!HitCoordinates.Contains(coordinate))
                {
                    HitCoordinates.Add(coordinate);
                    Health--;

                    Console.WriteLine($"Ship hit at ({row}, {col}) - Remaining health: {Health}");

                    // Debug: Print the remaining coordinates
                    Console.WriteLine($"Remaining Coordinates: {string.Join(", ", Coordinates)}");
                    Console.WriteLine($"Hit Coordinates: {string.Join(", ", HitCoordinates)}");

                    // Check if the ship is destroyed
                    if (Health == 0)
                    {
                        Console.WriteLine($"Ship {Name} is destroyed!");
                    }

                    return true;
                }
            }

            return false;
        }



        public bool IsDestroyed()
        {
            // A ship is destroyed if all its parts are hit
            return Health == 0;
        }
    }

}
