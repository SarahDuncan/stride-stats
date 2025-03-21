using Domain.Responses;

namespace Domain.Interfaces.Cache
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task StoreTokenAsync(CreateAccessTokenApiResponse tokenResponse);
        Task<string> GetRefreshTokenAsync();
    }
}
