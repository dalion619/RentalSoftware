using System;
using AutoMapper;
using DataContext.Models;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Models;
using Services.Interfaces;
using System.Threading.Tasks;

namespace VacationRental.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : Controller
    {
        public IMapper Mapper;
        public IBookingService _bookingService;
        public IRentalService _rentalService;

        public BookingsController(IMapper mapper, IBookingService bookingService, IRentalService rentalService)
        {
            Mapper = mapper;
            _bookingService = bookingService;
            _rentalService = rentalService;
        }

        /// <summary>
        /// Retrieves the booking information for the given booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Returns a BookingViewModel object</returns>
        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            var booking = await _bookingService.GetByBookingId(bookingId);

            if(booking == null)
            {
                throw new ApplicationException("Not Found");
            }

            return Mapper.Map<BookingViewModel>(booking);
        }

        /// <summary>
        ///  Creates a new booking for the existing rental
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns a ResourceIdViewModel object</returns>
        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("PositiveNights");

            var bookingId = 0;
                
            if (await _bookingService.IsFree(model.RentalId, model.Start, model.Start.AddDays(model.Nights)))
            {
                var booking = Mapper.Map<Booking>(model);
                booking.Unit = await _bookingService.GetFreeUnit(model.RentalId, model.Start, model.Start.AddDays(model.Nights));
                bookingId = await _bookingService.Create(booking);
            }
            else
            {
                throw new ApplicationException("Not Available");
            }

            return new ResourceIdViewModel(bookingId);
        }

    }
}
