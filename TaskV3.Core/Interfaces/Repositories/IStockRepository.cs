using System.Collections.Generic;
using System.Threading.Tasks;
using TaskV3.Core.Models;

namespace TaskV3.Core.Interfaces.Repositories
{
    public interface IStockRepository
    {
        Task<bool> CreateStockAsync(Stock stock);
        Task<bool> UpdateStockAsync(Stock stock);
        Task<Stock> GetStockAsync(int carId, int dealerId);
        Task<bool> DeleteStockAsync(Stock stock);
        Task<bool> DeleteStocksAsync(IEnumerable<Stock> stocks);
        Task<IEnumerable<Stock>> GetStocksByMakeAndModelAsync(string make, string model, int dealerId);
    }
}
