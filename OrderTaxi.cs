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
        public string CustomerName { get; private set; } = string.Empty;
        public double PickupLocationX { get; private set; }
        public double PickupLocationY { get; private set; }
        public double DestinationX { get; private set; }
        public double DestinationY { get; private set; }

        //Creates order with random data
        public OrderTaxi()
        {
            CustomerName = TaxiSimulator.GenerateRandomName();
            PickupLocationX = TaxiSimulator.RandomNumber();
            PickupLocationY = TaxiSimulator.RandomNumber();
            (DestinationX, DestinationY) = TaxiSimulator.GenerateNearbyCoordinates(PickupLocationX, PickupLocationY);
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
            Console.WriteLine("--------------------------------");
        }      
    }
}
