using DataContext.Models;
using DataContext.Repositories.Interfaces;

namespace DataContext.Repositories
{
    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public BookingRepository(ApiContext context) : base(context) { }
    }
}
