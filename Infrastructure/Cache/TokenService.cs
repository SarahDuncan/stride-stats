using Domain.Interfaces.Api;
using Domain.Interfaces.Cache;
using Domain.Requests;
using Domain.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Cache
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(6);
        private readonly IConfiguration _configuration;
        private readonly IApiClient _apiClient;

        public TokenService(IMemoryCache cache, IConfiguration configuration, IApiClient apiClient)
        {
            _cache = cache;
            _configuration = configuration;
            _apiClient = apiClient;
        }

        public virtual async Task<string> GetRefreshTokenAsync()
        {
            var request = CreateRefreshTokenRequest();
            var response = await _apiClient.Post<CreateAccessTokenApiResponse>(request);
            await StoreTokenAsync(response);
            return response.AccessToken;
        }

        public virtual async Task<string> GetTokenAsync()
        {
            if (_cache.TryGetValue("AccessToken", out string token))
                return token;

            return await GetRefreshTokenAsync();
        }

        public Task StoreTokenAsync(CreateAccessTokenApiResponse apiResponse)
        {
            _cache.Set("AccessToken", apiResponse.AccessToken, _tokenLifetime);
            _cache.Set("RefreshToken", apiResponse.RefreshToken, TimeSpan.FromDays(30));
            return Task.CompletedTask;
        }

        private IPostApiRequest CreateRefreshTokenRequest()
        {
            if (!_cache.TryGetValue("RefreshToken", out string refreshToken))
                throw new UnauthorizedAccessException("Refresh token not found.");

            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _configuration["ApiSettings:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _configuration["ApiSettings:ClientSecret"]),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
            });

            return new CreateAccessTokenApiRequest(_configuration["ApiSettings:TokenUrl"], tokenRequest);
        }
    }
}
