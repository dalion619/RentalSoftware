using AutoMapper;
using DataContext.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using VacationRental.Models;

namespace VacationRental.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        public IMapper Mapper;
        public IBookingService _bookingService;
        public IRentalService _rentalService;

        public RentalsController(IMapper mapper, IBookingService bookingService, IRentalService rentalService)
        {
            Mapper = mapper;
            _bookingService = bookingService;
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            var rental = await _rentalService.GetRentalById(rentalId);

            if(rental == null)
            {
                throw new ApplicationException("Not Found");
            }

             return Mapper.Map<RentalViewModel>(rental);
        }

        [HttpPost]
        //[Route("api/v1/rentals")]
        //[Route("~/api/v1/vacationrental/rentals")]
        public async Task<IActionResult> Post(RentalBindingModel model)
        {
            var rentalId = await _rentalService.Create(Mapper.Map<Rental>(model));
            return Ok(rentalId);
        }

        [HttpPut]
        //[Route("~/api/v1/vacationrental/rentals/{rentalId:int}")]
        public async void Put(int rentalId, RentalBindingModel model)
        {
            var exists = await _rentalService.GetRentalById(rentalId);

            //I do not have a test in place for this one, must create it.
            if (exists == null)
            {
                throw new ApplicationException("Not Found");
            }

            var rental = new Rental()
            {
                Id = rentalId,
                PreparationTimeInDays = (int)model.PreparationTimeInDays,
                Units = model.Units
            };

            //if (originalRental.PreparationTimeInDays != model.PreparationTimeInDays)

            await _rentalService.Update(rental);


            //TO-DO:
            //If the length of preparation time is changed then it should be updated for all existing bookings. 
            //The request should fail if decreasing the number of units or increasing the length of preparation time will produce overlapping between 
            //existing bookings and/or their preparation times.

            //for each existing booking in this rental, add 1 extra day of preparation time.

        }

        //[Route("~/api/v1/vacationrental/rentals")]
        //[HttpPost]
        //public ResourceIdViewModel PostVacation(RentalBindingModel model)
        //{
        //    return Post(model);
        //}
    }
}
