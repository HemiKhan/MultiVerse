using Data.Dtos;
using Data.Models;

namespace Services.AccountServices
{
    public interface IAccountService
    {
        public P_ReturnMessage_Result SignIn(LoginRequestDto req);
    }
}
