using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaxiServiceSim.Taxi;

namespace TaxiServiceSim
{
    //Manager all taxi orders
    public class OrderManager
    {
        #region singleton
        //Singleton
        private static OrderManager _instance = null;
        private static object _lock = new object();

        private OrderManager()
        {
        }

        public static OrderManager Instance
        {
            get
            {
                lock (_lock) 
                {
                    if (_instance == null)
                    {
                        _instance = new OrderManager();
                    }

                    return _instance;
                }
            }
        }
        #endregion

        public List<Taxi> TaxiList { get; set; } = new List<Taxi>();

        //Queue for all taxi orders
        private Queue<OrderTaxi> globalOrderQueue = new Queue<OrderTaxi>();

        //Add an order to the queue
        public void AddOrder(OrderTaxi order)
        {
            globalOrderQueue.Enqueue(order);
            Console.WriteLine($"Order {order.OrderID} added to the global queue.");
            Console.WriteLine("Order details: ");
            order.PrintOrderDetails();
        }

        //Process the next order in the queue
        public void ActivateNextOrder()
        {
            if (globalOrderQueue.Count > 0)
            {
                if (TaxiList.Any(taxi => taxi.currentStatus == TaxiStatus.Idle)) //Run only if there is a free taxi available
                {
                    MatchOrderToTaxi(globalOrderQueue.Dequeue());
                }
                else
                {
                    Console.WriteLine("No taxies aviable at the moment.");
                }
            }
            else
            {
                Console.WriteLine("No orders in the global queue.");
            }
        }

        //Process all taxies current orders
        public void ProcessAllOrders() 
        {
            foreach (Taxi taxi in TaxiList)
            {
                taxi.ProcessTaxiOrder();
                taxi.PrintDetails();
            }
        }

        //Addes the latest orders from the queue to the closest available taxi
        public void MatchOrderToTaxi(OrderTaxi? currentOrder)
        {
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

        //Gets the nearst idle taxi ID
        public string GetClosestFreeTaxiID(double destinationX, double destinationY)
        {
            string closestTaxiID = string.Empty;
            double closestTaxiDistance = TaxiSimulator.MAXIMUM_BOUNDRY_XY ^ 2;

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
