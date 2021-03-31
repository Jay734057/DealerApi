using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;

namespace TaskV3.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApiContext _context;

        public StockRepository(ApiContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateStockAsync(Stock stock)
        {
            _context.Add(stock);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStockAsync(Stock stock)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStocksAsync(IEnumerable<Stock> stocks)
        {
            _context.Stocks.RemoveRange(stocks);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Stock> GetStockAsync(int carId, int dealerId)
        {
            var stock = await _context.Stocks.SingleOrDefaultAsync(e => e.CarId == carId && e.DealerId == dealerId);
            return stock;
        }

        public async Task<IEnumerable<Stock>> GetStocksByMakeAndModelAsync(string make, string model, int dealerId)
        {
            return await _context.Stocks.AsNoTracking().Include(e => e.Car)
                .Where(e => e.Car.Make == make && e.Car.Model == model && e.DealerId == dealerId).ToListAsync();
        }

        public async Task<bool> UpdateStockAsync(Stock stock)
        {
            _context.Update(stock);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
