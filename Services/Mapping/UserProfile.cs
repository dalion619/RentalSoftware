using AutoMapper;
using DataContext.Models;
using Services.Models;

namespace Services.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Booking, BookingViewModel>();
        }
    }
}
