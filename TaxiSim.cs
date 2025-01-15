using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaxiServiceSim.Taxi;

namespace TaxiServiceSim
{
    public class TaxiSim
    {
        #region singleton

        private static readonly TaxiSim _instance = new TaxiSim();

        //On creation
        private TaxiSim()
        {

        }

        public static TaxiSim Instance
        {
            get
            {
                return _instance;
            }

        }
        #endregion

        public List<Taxi> TaxiList { get; set; } = new List<Taxi>();
        public const double SimulatorTickTimer = 20; //20 Seconds per tick

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

        //City boundires: 20km | 20000m for X and Y
        public const int MaximumBoundryXY = 20000;


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
                        AdvanceSim();
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
        public void AdvanceSim()
        {
            OrderManager.AddOrder(new OrderTaxi()); //Adds a new order to the queue
            MatchOrderToTaxi();
            foreach (Taxi taxi in TaxiList) 
            {
                taxi.ProcessTaxiOrder();
                taxi.PrintDetails();
            }
        }

        //Addes the latest orders from the queue to the closest available taxi
        public void MatchOrderToTaxi()
        {
            if (TaxiList.Any(taxi => taxi.currentStatus == TaxiStatus.Idle))//Run only if there is a free taxi available
            {
                OrderTaxi? currentOrder = OrderManager.GetNextOrder();

                if (currentOrder != null) //Make sure the order isnt empty
                {
                    string closestTaxiID = GetClosestFreeTaxiID(currentOrder.PickupLocationX, currentOrder.PickupLocationY);
                    Taxi? targetTaxi = TaxiList.FirstOrDefault(t => t.TaxiID == closestTaxiID);

                    if (targetTaxi != null)
                    {
                        targetTaxi.CurrentOrder = currentOrder;
                        targetTaxi.currentStatus = TaxiStatus.HeadingToCustomer;
                        Console.WriteLine($"Taxi number {targetTaxi.TaxiID} processing order {currentOrder.OrderID}");
                    }
                    else
                    {
                        Console.WriteLine($"No taxi matches the order.");
                    }
                }
                else
                {
                    Console.WriteLine("No orders found.");
                }
            }
            else 
            {
                Console.WriteLine("No taxies aviable at the moment.");
            }
        }

        //Gets the nearst idle taxi ID
        public string GetClosestFreeTaxiID(double destinationX, double destinationY)
        {
            string closestTaxiID = "";
            double closestTaxiDistance = MaximumBoundryXY ^ 2;

            foreach (Taxi taxi in TaxiList)
            {
                double currentTaxiDistance = Math.Abs(destinationX - taxi.PositionX) + Math.Abs(destinationY - taxi.PositionY);
                if (taxi.currentStatus == TaxiStatus.Idle && taxi.CurrentOrder == null && currentTaxiDistance < closestTaxiDistance)
                {
                    closestTaxiID = taxi.TaxiID;
                    closestTaxiDistance = currentTaxiDistance;
                }
            }

            return closestTaxiID;
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

        //Print current taxi list state
        public void PrintTaxiListState() 
        {
            
            Console.WriteLine("---------------TAXI STATES-------------");
            foreach (Taxi taxi in TaxiList)
            {
                Console.WriteLine();
                Console.WriteLine("taxi number " + taxi.TaxiID);
                Console.WriteLine("taxi X " + taxi.PositionX);
                Console.WriteLine("taxi Y " + taxi.PositionY);
                Console.WriteLine();
            }
            Console.WriteLine("------------END TAXI STATES------------");
        }
    }
}
