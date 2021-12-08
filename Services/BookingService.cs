using DataContext.Models;
using DataContext.UnitOfWork;
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
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentalService _rentalService;

        public BookingService(IUnitOfWork unitOfWork, IRentalService rentalService)
        {
            _unitOfWork = unitOfWork;
            _rentalService = rentalService;
        }

        public async Task<AddBookingResponse> AddBooking(AddBookingRequest request)
        {
            AddBookingResponse response = new AddBookingResponse();

            try
            {
                if (request.NumberOfNigths < 0)
                {
                    response.Message = "Nights must be positive";
                    response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -2 };
                    response.Succeeded = false;
                    return response;
                }

                var rental = await _rentalService.GetByRentalId(request.RentalId); //I need to make sure that when I retrieve this rental that a list of bookings comes attached too.

                if (rental == null)
                {
                    response.Message = "Rental not found";
                    response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -2 };
                    response.Succeeded = false;
                    return response;
                }

                for (var i = 0; i < request.NumberOfNigths; i++)
                {
                    var count = 0;

                    foreach (var bookingItem in rental.BookingCollection) //I need get my collection of bookings into here.  Basically this is just an iteration of every booking in
                                                                          //this rental that is already in storage in this rental plain and simple.
                    {
                        if ((bookingItem.Start <= request.StartDate.Date && bookingItem.Start.AddDays(bookingItem.Nights + rental.PreparationTimeInDays) > request.StartDate.Date)
                            || (bookingItem.Start < request.StartDate.AddDays(request.NumberOfNigths + rental.PreparationTimeInDays) && bookingItem.Start.AddDays(bookingItem.Nights + rental.PreparationTimeInDays) >= request.StartDate.AddDays(request.NumberOfNigths + rental.PreparationTimeInDays))
                            || (bookingItem.Start > request.StartDate && bookingItem.Start.AddDays(bookingItem.Nights + rental.PreparationTimeInDays) < request.StartDate.AddDays(request.NumberOfNigths + rental.PreparationTimeInDays)))
                        {
                            count++;
                        }
                    }

                    var rental2 = await _rentalService.GetByRentalId(request.RentalId); //This is basically to get the number of units available in this rental
                    var rentalUnits = rental2.Units;

                    //if (count >= _iRentalRepository.GetById(request.RentalId).Units)


                    if (count >= rentalUnits) //And if more bookings are put against how many units are available the request fails.
                    {
                        response.Message = "Not available";
                        response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -2 };
                        response.Succeeded = false;
                        return response;
                    }
                }

                Booking booking = new Booking()
                {
                    RentalId = request.RentalId,
                    Nights = request.NumberOfNigths,
                    Start = request.StartDate.Date,
                    //Rental = rental //Once you add this, it throws a massive fucking error about duplicate entry.
                };

                await _unitOfWork.BookingRepository.AddAsync(booking);
                await _unitOfWork.Complete();

                response.Succeeded = true;
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = booking.Id };

            }
            catch (Exception exception)
            {
                response.Succeeded = false;
                response.Message = exception.Message;
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -1 };
            }

            return response;
        }

        public async Task<GetBookingResponse> GetBooking(GetBookingRequest request)
        {
            GetBookingResponse response = new GetBookingResponse();

            try
            {
                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(request.bookingId);

                if (booking == null)
                {
                    response.Succeeded = false;
                    response.Message = "Booking not found";
                    return response;
                }

                response.Succeeded = true;

                BookingViewModel bookingViewModel = new BookingViewModel
                {
                    Id = booking.Id,
                    Nights = booking.Nights,
                    RentalId = booking.RentalId,
                    Start = booking.Start
                };

                response.BookingViewModel = bookingViewModel;
            }
            catch (Exception exception)
            {
                response.Succeeded = false;
                response.Message = exception.Message;
            }

            return response;
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await _unitOfWork.BookingRepository.GetAll();
        }
    }
}
