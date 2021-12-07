using DataContext.Models;
using DataContext.UnitOfWork;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentalService _rentalService;

        public BookingService(IUnitOfWork unitOfWork, IRentalService rentalService)
        {
            _unitOfWork = unitOfWork;
            _rentalService = rentalService;
        }
        public async Task<bool> IsFree(int rentalId, DateTime start, DateTime end)
        {
            var rental = await _rentalService.GetRentalById(rentalId);
            var bookings = await GetByRentalAndDate(rentalId, start, end);
            return rental.Units > bookings.Count();
        }

        public IEnumerable<Booking> GetByRentalId(int rentalId)
        {
            var bookings = _unitOfWork.BookingRepository.Find(x => x.RentalId == rentalId);
            return bookings;
        }

        public async Task<IEnumerable<Booking>> GetByRentalAndDate(int rentalId, DateTime startDate, DateTime endDate)
        {
            var rental = await _rentalService.GetRentalById(rentalId);
            var bookings = await _unitOfWork.BookingRepository.GetAll();

            bookings = bookings.Where(x => x.RentalId == rentalId &&
                                       x.Start <= endDate &&
                                       startDate <= x.Start.AddDays(x.Nights + rental.PreparationTimeInDays)).ToList();

            return bookings.ToList();
        }

        public async Task<int> GetFreeUnit(int rentalId, DateTime start, DateTime end)
        {
            var bookings = await GetByRentalAndDate(rentalId, start, end);
            if (bookings.Any())
                return bookings.Max(x => x.Unit) + 1;
            else return 1;
        }



        public async Task<Booking> GetByBookingId(int bookingId)
        {
            return await _unitOfWork.BookingRepository.Get(bookingId);
        }


        public async Task<int> Create(Booking booking)
        {
            await _unitOfWork.BookingRepository.AddAsync(booking);
            return await _unitOfWork.Complete();
        }




    }
}
