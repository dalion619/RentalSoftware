using DataContext.Models;
using VacationRental.Models;
using AutoMapper;

namespace VacationRental.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RentalBindingModel, Rental>();
            CreateMap<RentalViewModel, Rental>();
            CreateMap<Rental, RentalViewModel>();
            CreateMap<BookingBindingModel, Booking>();
            CreateMap<BookingViewModel, Booking>();
            CreateMap<Booking, BookingViewModel>();
        }
    }
}
