using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ITokenService
    {
        public (string Token, DateTime Expiry) CreateToken(User user)
;
    }
}
