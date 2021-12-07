using DataContext.Models;
using DataContext.UnitOfWork;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Rental> GetRentalById(int rentalId)
        {
            return await _unitOfWork.RentalRepository.GetRentalById(rentalId);
        }

        public async Task<Rental> Create(Rental rental)
        {
            await _unitOfWork.RentalRepository.AddAsync(rental);
            var created = await _unitOfWork.Complete();

            if (created == 0)
            {
                return null;
            }

            return await _unitOfWork.RentalRepository.Get(rental.Id);
        }

        public async Task<bool> Update(Rental rental)
        {
            _unitOfWork.RentalRepository.Update(rental);
            var updated = await _unitOfWork.Complete();
            return updated > 0;
        }
    }
}
