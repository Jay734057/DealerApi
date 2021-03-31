using System.Threading.Tasks;
using TaskV3.Core.Models;

namespace TaskV3.Core.Interfaces.Repositories
{
    public interface IDealerRepository
    {
        Task<Dealer> GetDealerByNameAsync(string name);
        Task<Dealer> GetDealerByIdAsync(int dealerId);
    }
}
