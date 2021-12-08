using DataContext.Models;
using Services.Contracts.Request;
using Services.Contracts.Response;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CalendarService : ICalendarService
    {
        private IBookingService _bookingService;
        private IRentalService _rentalService;

        public CalendarService(IBookingService bookingService, IRentalService rentalService)
        {
            _bookingService = bookingService;
            _rentalService = rentalService;
        }

        public async Task<GetCalendarResponse> GetCalendar(GetCalendarRequest request)
        {
            GetCalendarResponse response = new GetCalendarResponse();

            try
            {
                if (request.NumberOfNights < 0) //If user has entered 0 nights
                {
                    response.Message = "Nights must be positive";
                    response.Succeeded = false;
                    return response;
                }

                var rentals = await _rentalService.GetAll();

                if (rentals.Where(w => w.Id == request.RentalId).Count() == 0) //If there are no rentals available with that Id
                {
                    response.Message = "Not Found";
                    response.Succeeded = false;
                    return response;
                }

                CalendarViewModel calendarViewModel = new CalendarViewModel
                {
                    RentalId = request.RentalId,
                    Dates = new List<CalendarDateViewModel>()
                };

                for (var i = 0; i < request.NumberOfNights; i++) //for each booking night
                {
                    CalendarDateViewModel calendarDateViewModel = new CalendarDateViewModel
                    {
                        Date = request.BookingStartDate.Date.AddDays(i),
                        Bookings = new List<CalendarBookingViewModel>(),
                        PreparationTimes = new List<PreparationTimesViewModel>()
                    };

                    var rental = await _rentalService.GetByRentalId(request.RentalId); //get the preparation days
                    var preparationTimeInDays = rental.PreparationTimeInDays;
                    var bookings = await _bookingService.GetAll(); //get all bookings

                    foreach (var booking in bookings.Where(x => x.Id == request.RentalId)) //for each actual booking with this rental Id
                    {

                        if (calendarDateViewModel.Date >= booking.Start && calendarDateViewModel.Date < booking.Start.AddDays(booking.Nights))
                        {
                            calendarDateViewModel.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = 1 });
                        }

                        if (calendarDateViewModel.Date >= booking.Start.AddDays(booking.Nights) && calendarDateViewModel.Date < booking.Start.AddDays(booking.Nights + preparationTimeInDays))
                        {
                            calendarDateViewModel.PreparationTimes.Add(new PreparationTimesViewModel { Unit = 1 });
                        }
                    }

                    calendarViewModel.Dates.Add(calendarDateViewModel);
                }

                response.CalendarViewModel = calendarViewModel;
                response.Succeeded = true;

            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
