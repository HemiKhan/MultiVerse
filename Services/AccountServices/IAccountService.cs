using Data.Dtos;
using Data.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Services.AccountServices
{
    public interface IAccountService
    {
        Task<SignIn_Res> LoginAsync(SignIn_Req req, GenrateTokenDelgate tokenGenrator, CancellationToken cancellationToken);
        Task<SignIn_Res> LoginFromCookiesAsync(RefreshTokenReq req, CancellationToken cancellationToken);
        string GenerateToken(P_Get_User_Info userInfo, string Encrypted_Key);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
        RefreshTokenRes RefreshToken(RefreshTokenReq _GetRefreshTokenResDTO, CancellationToken cancellationToken);
    }
}
