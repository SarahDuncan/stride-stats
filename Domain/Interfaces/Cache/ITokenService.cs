using Domain.Responses;

namespace Domain.Interfaces.Cache
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task StoreTokenAsync(StravaTokenResponse tokenResponse);
        Task<string> GetRefreshTokenAsync();
    }
}
