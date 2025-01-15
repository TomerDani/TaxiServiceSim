using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiServiceSim
{
    public class Taxi
    {
        public enum TaxiStatus
        {
            Idle,                // Taxi is not occupied
            HeadingToCustomer,   // Taxi is on the way to pick up the customer
            HeadingToDestination // Taxi is on the way to the customer's destination
        }
        public TaxiStatus currentStatus = TaxiStatus.Idle;
        public string TaxiID { get; private set; } = GenerateCarNumber();
        public string DriverName { get; private set; } = "";
        public double PositionX { get;  set; } = TaxiSim.RandomNumber();
        public double PositionY { get;  set; } = TaxiSim.RandomNumber();
        public OrderTaxi? CurrentOrder { get; set; } = null;

        private int Speed = 20; // 72 KPH | 20M/S

        //Constructors
        public Taxi(string driverName, double positionX, double positionY)
        {
            DriverName = driverName;
            PositionX = positionX;
            PositionY = positionY;
        }
        public Taxi(string driverName)
        {
            DriverName = driverName;
        }

        public void ProcessTaxiOrder() 
        {
            double timeRemaining = TaxiSim.SimulatorTickTimer;

            //Check if this taxi got an order
            if (CurrentOrder != null)
            {
                //Start moving towards the customer
                if(currentStatus == TaxiStatus.HeadingToCustomer) 
                {
                    timeRemaining = MoveTaxi(CurrentOrder.PickupLocationX, CurrentOrder.PickupLocationY);

                    //If mannaged to reach the customer before time, prepare to head towards destination
                    if(timeRemaining > 0) 
                    {
                        currentStatus = TaxiStatus.HeadingToDestination;
                    }
                }

                //Move towards destination
                if (currentStatus == TaxiStatus.HeadingToDestination)
                {
                    MoveTaxi(CurrentOrder.DestinationX, CurrentOrder.DestinationY, timeRemaining);

                    //If reached the restination, return to idle and discard the completed order.
                    if (CurrentOrder.DestinationX == PositionX && CurrentOrder.DestinationY == PositionY)
                    {
                        currentStatus = TaxiStatus.Idle;
                        CurrentOrder = null;
                    }
                }
            }
        }


        //Move taxi to destination
        public double MoveTaxi(double destinationX, double destinationY, double timeRemaining = TaxiSim.SimulatorTickTimer)
        {
            double remainingDistanceToTravel = Speed * timeRemaining;

            double XDistanceToTravel = destinationX - PositionX;
            double YDistanceToTravel = destinationY - PositionY;

            //First move on the X axis
            if (Math.Abs(XDistanceToTravel) > remainingDistanceToTravel) //Is too far from X destination in this time snippet
            {
                PositionX += Math.Sign(XDistanceToTravel) * remainingDistanceToTravel;
                remainingDistanceToTravel = 0;
            }
            else //Can reach X in this time snippet
            {
                PositionX = destinationX;
                remainingDistanceToTravel = remainingDistanceToTravel - Math.Abs(XDistanceToTravel);
            }

            //Then move on the Y axis if possible
            if (Math.Abs(YDistanceToTravel) > remainingDistanceToTravel) //Is too far from Y destination in this time snippet
            {
                PositionY += Math.Sign(YDistanceToTravel) * remainingDistanceToTravel;
                remainingDistanceToTravel = 0;
            }
            else //Can reach Y in this time snippet
            {
                PositionY = destinationY;
                remainingDistanceToTravel = remainingDistanceToTravel - Math.Abs(YDistanceToTravel);
            }

            return (remainingDistanceToTravel / Speed); //Time remaining
        }


        public void PrintDetails()
        {
            Console.WriteLine($"--------Taxi ID: {TaxiID}-----------");
            Console.WriteLine($"Driver: {DriverName}");
            Console.WriteLine($"Location: ({PositionX}, {PositionY})");
            Console.WriteLine("Status: " + currentStatus);
            Console.WriteLine();
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

    }
}
