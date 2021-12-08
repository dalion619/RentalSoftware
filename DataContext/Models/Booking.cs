using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataContext.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        //public Rental Rental { get; set; } //Once you add this, it throws a massive fucking error about duplicate entry.
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
