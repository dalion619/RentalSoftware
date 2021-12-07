using DataContext.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        Task<int> Create(Booking booking);
        Task<Booking> GetByBookingId(int bookingId);
        Task<IEnumerable<Booking>> GetByRentalAndDate(int rentalId, DateTime startDate, DateTime endDate);
        IEnumerable<Booking> GetByRentalId(int rentalId);
        Task<int> GetFreeUnit(int rentalId, DateTime start, DateTime end);
        Task<bool> IsFree(int rentalId, DateTime start, DateTime end);
    }
}