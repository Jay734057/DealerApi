using TaskV3.Core.Models;

namespace TaskV3.Core.Interfaces.Business
{
    public interface IAuthenticationProvider
    {
        string GenerateJWTToken(Dealer dealer);
        void CreatePasswordHash(string password,out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
