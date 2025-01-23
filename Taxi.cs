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
        public string TaxiID { get; private set; }
        public string DriverName { get; private set; }
        public double PositionX { get;  private set; } //Mesured in meters
        public double PositionY { get;  private set; } //Mesured in meters
        public OrderTaxi? _currentOrder = null;

        private int Speed = 20; //Measured in M/S (72KH)
        private double timeRemaining = TaxiSimulator.SIMULATOR_TICK_SPEED;

        public OrderTaxi? CurrentOrder
        {
            get
            {
                return _currentOrder;
            }
            set
            {
                _currentOrder = value;
            }
        }

        public Taxi(string driverName, double positionX, double positionY)
        {
            TaxiID = TaxiSimulator.GenerateCarNumber();
            DriverName = driverName;
            PositionX = positionX;
            PositionY = positionY;
        }
        public Taxi(string driverName)
        {
            DriverName = driverName;
            TaxiID = TaxiSimulator.GenerateCarNumber();
            PositionX = TaxiSimulator.RandomNumber();
            PositionX = TaxiSimulator.RandomNumber();
        }

        public void ProcessTaxiOrder() 
        {
            timeRemaining = TaxiSimulator.SIMULATOR_TICK_SPEED;

            //Check if this taxi got an order
            if (currentStatus != TaxiStatus.Idle)
            {
                //Start moving towards the customer
                if(currentStatus == TaxiStatus.HeadingToCustomer) 
                {
                    MoveTaxi(CurrentOrder.PickupLocationX, CurrentOrder.PickupLocationY);

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

                    //If reached the destination, return to idle and discard the completed order.
                    if (CurrentOrder.DestinationX == PositionX && CurrentOrder.DestinationY == PositionY)
                    {
                        currentStatus = TaxiStatus.Idle;
                        CurrentOrder = null;
                    }
                }
            }
        }


        //Move taxi to destination. Distance mesured in meters
        private void MoveTaxi(double destinationX, double destinationY, double timeRemaining = TaxiSimulator.SIMULATOR_TICK_SPEED)
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

            this.timeRemaining = (remainingDistanceToTravel / Speed);
        }


        public void PrintDetails()
        {
            Console.WriteLine($"--------Taxi ID: {TaxiID}-----------");
            Console.WriteLine($"Driver: {DriverName}");
            Console.WriteLine($"Location: ({PositionX}, {PositionY})");
            Console.WriteLine("Status: " + currentStatus);
            Console.WriteLine();
        }
    }
}
