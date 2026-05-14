

    public class Car : Vehicle
    {
        public Car(string plate, string model) { LicensePlate = plate; Model = model; }
        public override void CalculateServiceCost() => Console.WriteLine(">> تكلفة صيانة السيارة: 100$");
        public override void PerformMaintenance() => Console.WriteLine($">> فحص المحرك والزيوت للسيارة {Model}.");
    }

    public class Truck : Vehicle
    {
        public Truck(string plate, string model) { LicensePlate = plate; Model = model; }
        public override void CalculateServiceCost() => Console.WriteLine(">> تكلفة صيانة الشاحنة: 250$");
        public override void PerformMaintenance() => Console.WriteLine($">> فحص أنظمة الهيدروليك للشاحنة {Model}.");
    }

    public class Motorcycle : Vehicle
    {
        public Motorcycle(string plate, string model) { LicensePlate = plate; Model = model; }
        public override void CalculateServiceCost() => Console.WriteLine(">> تكلفة صيانة الدراجة: 50$");
        public override void PerformMaintenance() => Console.WriteLine($">> فحص السلسلة والمكابح للدراجة {Model}.");
    }

    public delegate void MaintenanceHandler(string message);

    public class GarageManager
    {
        private List<Vehicle> _vehicles = new List<Vehicle>();
        public event MaintenanceHandler OnMaintenanceComplete;

        public void AddVehicle(Vehicle v)
        {
            if (Validator.IsValidPlate(v.LicensePlate))
            {
                _vehicles.Add(v);
                GarageStats.IncrementCount();
            }
        }

        public void ProcessAll()
        {
            foreach (var v in _vehicles)
            {
                v.PerformMaintenance();
                v.CalculateServiceCost();
                OnMaintenanceComplete?.Invoke($"إشعار: الصيانة اكتملت للمركبة {v.Model} ({v.LicensePlate})");
            }
        }
    }

    public static class Validator
    {
        public static bool IsValidPlate(string plate) => !string.IsNullOrWhiteSpace(plate);
    }

    public static class GarageStats
    {
        public static int TotalVehiclesProcessed { get; private set; }
        public static void IncrementCount() => TotalVehiclesProcessed++;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            GarageManager myGarage = new GarageManager();

            myGarage.OnMaintenanceComplete += (msg) => {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg);
                Console.ResetColor();
            };

            myGarage.AddVehicle(new Car("A-123", "Toyota Camry"));
            myGarage.AddVehicle(new Truck("T-789", "Volvo FH16"));
            myGarage.AddVehicle(new Motorcycle("M-456", "Yamaha R1"));

            myGarage.ProcessAll();

            Console.WriteLine("\n-------------------------------------------");
            Console.WriteLine($"إجمالي المركبات: {GarageStats.TotalVehiclesProcessed}");
            Console.WriteLine("-------------------------------------------");
            Console.ReadKey();
        }
    }
}
