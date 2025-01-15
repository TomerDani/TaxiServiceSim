using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaxiServiceSim.Taxi;

namespace TaxiServiceSim
{
    public class TaxiSimulator
    {
        #region singleton
        //Singleton
        private static readonly TaxiSimulator _instance = new TaxiSimulator();

        private TaxiSimulator()
        {

        }

        public static TaxiSimulator Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        public const double SimulatorTickTimer = 20; //20 Seconds per tick
        public const int MaximumBoundryXY = 20000; //City boundires: 20km | 20000m for X and Y

        private OrderManager orderManager = OrderManager.Instance;

        //Names used to create orders
        readonly static List<string> names = new List<string>
        {
            "Alice", "Bob", "Charlie", "Diana", "Eli",
            "Fiona", "George", "Hannah", "Ian", "Jane",
            "Kevin", "Laura", "Michael", "Nina", "Oscar",
            "Paula", "Quincy", "Rachel", "Steve", "Tina",
            "Uma", "Victor", "Wendy", "Xander", "Yara",
            "Zach", "Amber", "Brian", "Cathy", "Derek",
            "Erica", "Frank", "Grace", "Harry", "Isla",
            "Jack", "Kara", "Liam", "Mila", "Noah",
            "Olivia", "Peter", "Quinn", "Ryan", "Sophia",
            "Thomas", "Ursula", "Vera", "Walter", "Xenia",
            "Yvonne", "Zane"
        };

        public void RunSim() 
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("Press 'Enter' to trigger or 'E' to exit.");
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // Read a key without displaying it.

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        AdvanceSimulator();
                        Console.WriteLine("==========================================================================");
                        break;

                    case ConsoleKey.E:
                        isRunning = false;
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        Console.WriteLine("Unknown command. Type Press 'Enter' to trigger or 'E' to exit.");
                        break;
                }
            }
        }

        //Proceedes to the next simulation instance
        public void AdvanceSimulator()
        {
            orderManager.AddOrder(new OrderTaxi()); //Adds a new order to the queue
            orderManager.ActivateNextOrder(); //Process the latest order
            orderManager.ProcessAllOrders();
        }        

        //Chooses a random name
        public static string GenerateRandomName()
        {
            Random random = new Random();
            return names[random.Next(names.Count)];
        }

        //Generates a random number from 0.00 to 20000.00
        public static double RandomNumber()
        {
            Random random = new Random();
            return Math.Round(new Random().NextDouble() * MaximumBoundryXY, 2);
        }

        
        public static string GenerateCarNumber()
        {
            Random random = new Random();

            // 8-digit format: XXX-XX-XXX
            int part1 = random.Next(100, 1000); // 3 digits
            int part2 = random.Next(10, 100);   // 2 digits
            int part3 = random.Next(100, 1000); // 3 digits
            return $"{part1}-{part2}-{part3}";
        }

        //Generate X and Y coordinates, not farther then 2000M (Physical distance)
        public static (double, double) GenerateNearbyCoordinates(double originalX, double originalY)
        {
            Random random = new Random();

            //Total distance allowed
            int maxDistance = 2000;

            //Randomly split the distance between X and Y
            double distanceX = random.Next(0, maxDistance + 1);
            double distanceY = maxDistance - distanceX;

            //Randomly decide the direction (positive or negative) for X and Y
            double offsetX = originalX - distanceX >= 0 ? -distanceX : distanceX;
            double offsetY = originalY - distanceY >= 0 ? -distanceY : distanceY;

            //Calculate the new coordinates
            double newX = originalX + offsetX;
            double newY = originalY + offsetY;

            //Ensure the new coordinates are within valid bounds (0 to 20000)
            newX = Math.Clamp(newX, 0, 20000);
            newY = Math.Clamp(newY, 0, 20000);

            return (newX, newY);
        }
    }
}
