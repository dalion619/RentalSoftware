using DataContext.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        Task<int> Create(Booking booking);
        List<Booking> GetByRentalId(int rentalId);
        Task<Booking> GetByBookingId(int bookingId);
        Task<List<Booking>> GetByRentalAndDate(DateTime startDate, DateTime endDate, int rentalId);
        Task<int> GetFreeUnit(int rentalId, DateTime startDate, DateTime endDate);
        Task<bool> IsFree(DateTime startDate, DateTime endDate, int rentalId);
    }
}