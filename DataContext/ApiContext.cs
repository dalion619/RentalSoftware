using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Rental> Rentals { get; set; }
    }
}
