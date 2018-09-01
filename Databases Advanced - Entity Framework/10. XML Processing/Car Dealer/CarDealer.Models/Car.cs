using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.PartCars = new HashSet<PartCar>();
            this.Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public ICollection<PartCar> PartCars { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
