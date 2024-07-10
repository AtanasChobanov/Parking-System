using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Parking_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, ParkingSlot> parking = new Dictionary<string, ParkingSlot>();
            for (char i = 'A'; i <= 'C'; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    ParkingSlot parkSlot = new ParkingSlot();
                    string id = i + j.ToString();
                    parking.Add(id, parkSlot);
                }
            }

            while (true)
            {
                foreach (var parkSlot in parking)
                {
                    parkSlot.Value.SetCurrentReservation();
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Добре дошли в Системата за управление на паркинг!");
                PrintList(parking);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Въведете команда (book, buy-now, sub, release) или 'exit' за изход: ");
                Console.ResetColor();
                string[] input = Console.ReadLine().Split(" ");
                if (input[0] == "exit")
                    break;
                Console.ForegroundColor = ConsoleColor.Yellow;
                ProcessCommand(parking, input);
                Console.ResetColor();
                Console.ReadLine();
                Console.Clear();
            }

        }

        public static void ProcessCommand(Dictionary<string, ParkingSlot> parking, string[] input)
        {
            try
            {
                string command = input[0];
                string slotId = input[1];
                
                var slot = parking[slotId];
                if (command == "release")
                {
                    slot.Release(slotId);
                    return;
                }

                string name = input[2];
                string vehicleNumber = input[3];
                DateTime dateOfResStart = DateTime.Parse(input[4]);
                DateTime dateOfResEnd = DateTime.Parse(input[5]);

                switch (command)
                {
                    case "book":
                        slot.PreReserve(slotId, name, vehicleNumber, dateOfResStart, dateOfResEnd);
                        break;

                    case "buy-now":
                        if (!(slot.CurrReservation == null))
                        {
                            Console.WriteLine("Това паркомясто вече е заето.");
                            return;
                        }
                        slot.ReserveNow(slotId, name, vehicleNumber, dateOfResEnd);
                        break;

                    case "sub":
                        if (!(slot.CurrReservation == null))
                        {
                            Console.WriteLine("Това паркомясто вече е заето.");
                            return;
                        }
                        slot.Subscribtion(slotId, name, vehicleNumber);
                        break;

                    default:
                        Console.WriteLine("Невалидна команда.");
                        break;
                }
                
            }
            catch (Exception ex) { Console.WriteLine("Невалидна команда."); }
        }

        static void PrintList(Dictionary<string, ParkingSlot> parking)
        {
            foreach (var slot in parking)
            {
                Console.WriteLine($"{slot.Key} - {slot.Value.ToString()}");
            }
        }

        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }
    }
}
