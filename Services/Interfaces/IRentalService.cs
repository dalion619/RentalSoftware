using DataContext.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRentalService
    {
        Task<Rental> GetRentalById(int rentalId);
        Task<Rental> Create(Rental rental);
        Task<bool> Update(Rental rental);
    }
}