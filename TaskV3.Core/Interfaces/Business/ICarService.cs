using System.Collections.Generic;
using System.Threading.Tasks;
using TaskV3.Core.Models;

namespace TaskV3.Core.Interfaces.Business
{
    public interface ICarService
    {
        Task<int> AddCarAsync(Car car);
        Task<bool> RemoveCarAsync(int carId, int dealerId);
        Task<bool> UpdateCarStockAsync(int carId, int amount, int dealerId);
        Task<bool> ListCarsAsync(int carId, int amount, int dealerId);
        Task<IEnumerable<Stock>> SearchCarStocksAsync(string make, string model, int dealerId);
    }
}
