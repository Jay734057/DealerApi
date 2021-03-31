using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;

namespace TaskV3.Repositories
{
    public class DealerRepository : IDealerRepository
    {
        private readonly ApiContext _context;

        public DealerRepository(ApiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Dealer> GetDealerByIdAsync(int dealerId)
        {
            return await _context.Dealers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == dealerId);
        }

        public async Task<Dealer> GetDealerByNameAsync(string name)
        {
            return await _context.Dealers.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}
