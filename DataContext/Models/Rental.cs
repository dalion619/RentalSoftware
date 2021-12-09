using DataContext.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Models
{
    public class Rental
    {
        public Rental()
        {
            Bookings = new HashSet<Booking>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Units { get; set; }
        public RentalTypeEnum RentalType { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
