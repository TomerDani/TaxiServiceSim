using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiServiceSim
{
    public class OrderTaxi
    {
        public Guid OrderID { get; private set; } = Guid.NewGuid();
        public string CustomerName { get; private set; } = "";
        public double PickupLocationX { get; private set; }
        public double PickupLocationY { get; private set; }
        public double DestinationX { get; private set; }
        public double DestinationY { get; private set; }

        //Constructors
        public OrderTaxi() //Creates order with random data
        {
            CustomerName = TaxiSim.GenerateRandomName();
            PickupLocationX = TaxiSim.RandomNumber();
            PickupLocationY = TaxiSim.RandomNumber();
            //DestinationX = TaxiSim.RandomNumber();
            //DestinationY = TaxiSim.RandomNumber();

            (DestinationX, DestinationY) = GenerateNearbyCoordinates(PickupLocationX, PickupLocationY);
        }

        //Creates order with set data
        public OrderTaxi(string customerName, double pickupLocationX, double pickupLocationY, double destinationX, double destinationY)
        {
            CustomerName = customerName;
            PickupLocationX = pickupLocationX;
            PickupLocationY = pickupLocationY;
            DestinationX = destinationX;
            DestinationY = destinationY;
        }

        //Print order details
        public void PrintOrderDetails()
        {
            Console.WriteLine($"Order ID: {OrderID}");
            Console.WriteLine($"Customer: {CustomerName}");
            Console.WriteLine($"Pickup Location X: {PickupLocationX}");
            Console.WriteLine($"Pickup Location Y: {PickupLocationY}");
            Console.WriteLine($"Destination X: {DestinationX}");
            Console.WriteLine($"Destination Y: {DestinationY}");
            Console.WriteLine(); //Empty space line
        }

        //Genetares a new coordinate within
        static double GenerateNearbyCoordinate(double original)
        {
            Random random = new Random();
            //Generate a random offset between -2000 and 2000
            int offset = random.Next(-2000, 2001);

            //Add the offset to the original coordinate
            double newCoordinate = original + offset;

            //Ensure the new coordinate is within the valid range (0 to 20000)
            return Math.Clamp(newCoordinate, 0, 20000);
        }


        //Generate X and Y coordinates, not farther then 2000M (Physical distance)
        private static (double, double) GenerateNearbyCoordinates(double originalX, double originalY)
        {
            Random random = new Random();
             
            //Total distance allowed
            int maxDistance = 2000;

            //Randomly split the distance between X and Y
            double distanceX = random.Next(0, maxDistance + 1);
            double distanceY = maxDistance - distanceX;

            //Randomly decide the direction (positive or negative) for X and Y
            //
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
