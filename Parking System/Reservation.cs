using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System
{
    internal class Reservation
    {
        private string name;
        private DateTime dateStart;
        private DateTime dateEnd;
        private string vehicleNumber;
        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }


        public string VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; }
        }

        public DateTime DateStart
        {
            get { return dateStart; }
            set { dateStart = value; }
        }

        public DateTime DateEnd
        {
            get { return dateEnd; }
            set { dateEnd = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Reservation(string name, string vehicleNumber, DateTime dateStart, DateTime dateEnd, double price)
        {
            this.name = name;
            this.dateStart = dateStart;
            this.dateEnd = dateEnd;
            this.vehicleNumber = vehicleNumber;
            this.price = price;
        }
    }
}
