using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models;
using System;
using System.Threading.Tasks;
using VacationRental.Models;

namespace VacationRental.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        public IMapper Mapper;
        public IBookingService _bookingService;
        public IRentalService _rentalService;
        public ICalendarService _calendarService;

        public CalendarController(IMapper mapper, IBookingService bookingService, IRentalService rentalService, ICalendarService calendarService)
        {
            Mapper = mapper;
            _bookingService = bookingService;
            _rentalService = rentalService;
            _calendarService = calendarService;
        }

        /// <summary>
        /// Retrieve the booking information for the given query parameters
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="start"></param>
        /// <param name="nights"></param>
        /// <returns>Returns a CalendarViewModel object</returns>
        [HttpGet]
        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Positive Nights");
            var rental = await _rentalService.GetRentalById(rentalId);
            if (rental is null)
                throw new ApplicationException("Not Found");

            var bookings = _bookingService.GetByRentalId(rentalId);

            //return new CalendarViewModel(rentalId, bookings, rental.PreparationTimeInDays, start, nights);

            return _calendarService.GetCalendar(rentalId, bookings, rental.PreparationTimeInDays, start, nights);
        }
    }
}
