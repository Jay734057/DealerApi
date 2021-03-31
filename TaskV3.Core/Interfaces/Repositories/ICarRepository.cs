using System.Collections.Generic;
using System.Threading.Tasks;
using TaskV3.Core.Models;

namespace TaskV3.Core.Interfaces.Repositories
{
    public interface ICarRepository
    {
        Task<int> CreateCarAsync(Car car);

        Task<bool> CreateCarsAsync(IEnumerable<Car> cars);

        Task<Car> GetCarByIdAsync(int id);

        Task<Car> GetCarByMakeModelAndYearAsync(string make, string model, int year);
    }
}
