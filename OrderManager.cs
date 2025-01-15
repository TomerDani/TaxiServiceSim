using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiServiceSim
{
    //Manager all taxi orders
    public static class OrderManager
    {
        // Global queue for all taxi orders
        private static Queue<OrderTaxi> globalOrderQueue = new Queue<OrderTaxi>();

        // Add an order to the global queue
        public static void AddOrder(OrderTaxi order)
        {
            globalOrderQueue.Enqueue(order);
            Console.WriteLine($"Order {order.OrderID} added to the global queue.");
            Console.WriteLine("Order details:");
            order.PrintOrderDetails();
        }

        // Process the next order in the global queue
        public static OrderTaxi? GetNextOrder()
        {
            if (globalOrderQueue.Count > 0)
            {
                return globalOrderQueue.Dequeue();
            }

            else
            {
                Console.WriteLine("No orders in the global queue.");
                return null;
            }
        }

        // Check if the global queue is empty
        public static bool IsQueueEmpty()
        {
            return globalOrderQueue.Count == 0;
        }
    }
}
