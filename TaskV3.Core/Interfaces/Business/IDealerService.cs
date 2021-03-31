using System.Threading.Tasks;
using TaskV3.Core.Models;

namespace TaskV3.Core.Interfaces.Business
{
    public interface IDealerService
    {
        Task<string> AuthenticateAsync(string name, string password);
        Task<Dealer> GetDealerByIdAsync(int dealerId);
    }
}
