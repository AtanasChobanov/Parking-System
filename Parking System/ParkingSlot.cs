using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System
{
    internal class ParkingSlot
    {
        private Reservation currReservation;
        private List<Reservation> reservationsQueue;

        public Reservation CurrReservation 
        {
            get { return currReservation; }
            set { currReservation = value; }
        }
        public List<Reservation> ReservationsQueue 
        {
            get { return reservationsQueue; }
            set { reservationsQueue = value; }
        }

        public ParkingSlot()
		{
            this.ReservationsQueue = new List<Reservation>();
		}

        public override string ToString()
        {
            if (currReservation == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                return "Свободно";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return $"Заето от {this.CurrReservation.Name} ({this.CurrReservation.VehicleNumber}) от {this.CurrReservation.DateStart} до {this.CurrReservation.DateEnd} ";
            } 
        }

        public void SetCurrentReservation()
        {
            try
            {
                if (this.currReservation.DateEnd <= Program.GetCurrentDate())
                {
                    this.currReservation = null;
                }
                
            }
            catch (Exception ex) { }

            foreach (var reservation in ReservationsQueue)
            {
                if (reservation.DateStart <= Program.GetCurrentDate() && reservation.DateEnd >= Program.GetCurrentDate())
                {
                    this.currReservation = reservation;
                    this.reservationsQueue.Remove(reservation);
                    return;
                }
            }
        }

        public bool CheckReservationWithOthers(DateTime dateOfResStart, DateTime dateOfResEnd)
        {
            foreach (var res in this.ReservationsQueue)
            {
                if ((dateOfResStart >= res.DateStart && dateOfResStart <= res.DateEnd) || (dateOfResEnd >= res.DateStart && dateOfResEnd <= res.DateEnd))
                {
                    Console.WriteLine("Паркомястото е вече заето за тези дати.");
                    return false;
                }
            }
            return true;
        }

        public void PreReserve(string slotId, string name, string vehicleNumber, DateTime dateOfResStart, DateTime dateOfResEnd)
        {

            TimeSpan timeUntilReservation = dateOfResStart - Program.GetCurrentDate();
            // Проверяваме дали желаната дата за резервация (dateStart) е до 1 седмица преди сегашната дата
            if (timeUntilReservation.TotalDays <= 7 && timeUntilReservation.TotalDays >= 0)
            {
                TimeSpan periodOfReservation = dateOfResEnd - dateOfResStart;

                double numberOfTotalDays = periodOfReservation.TotalDays;
                // Периода може да бъде 1-7 дни
                if (numberOfTotalDays >= 1 && numberOfTotalDays <= 7)
                {
                    if (!CheckReservationWithOthers(dateOfResStart, dateOfResEnd)) 
                    {
                        return;
                    }
                    // Извършваме предварителното запазване
                    double price;
                    if (timeUntilReservation.TotalDays >= 2)
                    {
                        price = periodOfReservation.TotalHours * 1.2;
                    }
                    else
                    {
                        price = periodOfReservation.TotalHours;
                    }

                    
                    Reservation reservation = new Reservation(name, vehicleNumber, dateOfResStart, dateOfResEnd, price);
                    this.ReservationsQueue.Add(reservation);

                    Console.WriteLine($"Паркомясто {slotId} е успешно резервирано от {name} ({vehicleNumber}), от {dateOfResStart} до {dateOfResEnd}.\n" +
                        $"Цената, която трябва да платите за целия си престой е: {price:f2}");
                }
                else
                {
                    Console.WriteLine("Невалидна резервация: Броят дни за резервация трябва да бъде между 1 и 7 дни.");
                }
            }
            else
            {
                Console.WriteLine("Невалидна резервация: До 1 седмица преди началото на резервацията може да се запазва свободно място.");
            }
        }

        public void ReserveNow(string slotId, string name, string vehicleNumber, DateTime dateOfResEnd)
        {
            DateTime dateOfResStart = Program.GetCurrentDate();
            TimeSpan periodOfReservation = dateOfResEnd - dateOfResStart;

            if ((Program.GetCurrentDate().DayOfWeek == DayOfWeek.Saturday || Program.GetCurrentDate().DayOfWeek == DayOfWeek.Sunday)
                && (periodOfReservation.TotalHours >= 1 && (dateOfResEnd.DayOfWeek == DayOfWeek.Sunday || dateOfResEnd.DayOfWeek == DayOfWeek.Saturday)) // weekend
                || periodOfReservation.TotalHours >= 1 && periodOfReservation.TotalHours <= 24 /*workday*/)
            {
                if (!CheckReservationWithOthers(dateOfResStart, dateOfResEnd))
                {
                    return;
                }
                double price = periodOfReservation.TotalHours;

                Reservation reservation = new Reservation(name, vehicleNumber, dateOfResStart, dateOfResEnd, price);
                this.currReservation = reservation;

                Console.WriteLine($"Паркомясто {slotId} е успешно резервирано от {name} ({vehicleNumber}), от {Program.GetCurrentDate()} до {dateOfResEnd}.\n" +
                        $"Цената, която трябва да платите за целия си престой е: {price:f2}");

            }
            else
            {
                Console.WriteLine("Невалидна заявка!");
            }
        }

        public void Subscribtion(string slotId, string name, string vehicleNumber)
        {
            DateTime dateOfResStart = Program.GetCurrentDate();
            DateTime dateOfResEnd = Program.GetCurrentDate().AddDays(30);
            if (!CheckReservationWithOthers(dateOfResStart, dateOfResEnd))
            {
                return;
            }
            double price = 168;

            Reservation reservation = new Reservation(name, vehicleNumber, dateOfResStart, dateOfResEnd, price);
            this.currReservation = reservation;

            Console.WriteLine($"Паркомясто {slotId} е успешно резервирано от  {name}  ( {vehicleNumber} ), с абонамент от {dateOfResStart} до {dateOfResEnd}.\n" +
                        $"Цената, която трябва да платите за целия си престой е: {price:f2}");

        }

        public void Release(string slotId)
        {
            if (this.CurrReservation == null)
            {
                Console.WriteLine($"Паркомясто {slotId} е вече свободно.");
                return;
            }

            TimeSpan periodOfReservation = this.currReservation.DateEnd - this.currReservation.DateStart;
            
            if (periodOfReservation.TotalDays == 30)
            {
                Console.WriteLine("Абонаментите не подлежат на възстановяване.");
                return;
            }

            TimeSpan timeLeft = this.currReservation.DateEnd - Program.GetCurrentDate();
            double refund = timeLeft.TotalHours * (this.currReservation.Price / periodOfReservation.TotalHours) * 0.7;

            this.currReservation = null;

            Console.WriteLine($"Паркомясто {slotId} е успешно освободено.\n" +
                $"Възстановената сума : {refund:f2}");
        }
    }
}
