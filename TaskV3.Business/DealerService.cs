using System;
using System.Threading.Tasks;
using TaskV3.Core.Interfaces.Business;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;

namespace TaskV3.Business
{
    public class DealerService : IDealerService
    {
        private readonly IDealerRepository _dealerRepository;
        private readonly IAuthenticationProvider _authenticationProvider;

        public DealerService(IDealerRepository dealerRepository, IAuthenticationProvider authenticationProvider)
        {
            _dealerRepository = dealerRepository ?? throw new ArgumentNullException(nameof(dealerRepository));
            _authenticationProvider = authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider));
        }

        public async Task<string> AuthenticateAsync(string name, string password)
        {
            //validate email and password
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                return null;

            var dealer = await _dealerRepository.GetDealerByNameAsync(name);

            if (dealer == null)
                return null;

            //verify password
            if (!_authenticationProvider.VerifyPassword(password, dealer.PasswordHash, dealer.PasswordSalt))
                return null;

            return _authenticationProvider.GenerateJWTToken(dealer);
        }

        public async Task<Dealer> GetDealerByIdAsync(int dealerId)
        {
            return await _dealerRepository.GetDealerByIdAsync(dealerId);
        }
    }
}
