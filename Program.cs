namespace TaxiServiceSim
{
    public class TaxiServiceSim
    {
        private static void Main(string[] args)
        {
            TaxiSimulator simulator = TaxiSimulator.Instance;
            OrderManager orderManager = OrderManager.Instance;

            List<Taxi> taxiList = new List<Taxi>()
            {
                new Taxi("Alice", 900, 900),
                new Taxi("Bob", 4567.89, 1234.56),
                new Taxi("Charlie"),
                new Taxi("Diana", 9876.54, 3210.98),
                new Taxi("Eli"),
                new Taxi("Fiona", 5678.12, 8765.43),
                new Taxi("George", 4321.10, 2109.87),
                new Taxi("Hannah"),
                new Taxi("Ian", 3456.78, 6543.21),
                new Taxi("Jane", 1000 , 1000),
            };

            orderManager.TaxiList = taxiList;

            orderManager.PrintTaxiListState();

            simulator.RunSim();
        }
    }
}