using Domain.Interfaces.Cache;
using Domain.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Cache
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(6);
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TokenService(IMemoryCache cache, IConfiguration configuration, HttpClient httpClient)
        {
            _cache = cache;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> GetRefreshTokenAsync()
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

            var response = await _httpClient.PostAsync(_configuration["ApiSettings:TokenUrl"], tokenRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var newToken = JsonSerializer.Deserialize<StravaTokenResponse>(content);

            await StoreTokenAsync(newToken);

            return newToken.AccessToken;
        }

        public async Task<string> GetTokenAsync()
        {
            if (_cache.TryGetValue("AccessToken", out string token))
                return token;

            return await GetRefreshTokenAsync();
        }

        public Task StoreTokenAsync(StravaTokenResponse tokenResponse)
        {
            _cache.Set("AccessToken", tokenResponse.AccessToken, _tokenLifetime);
            _cache.Set("RefreshToken", tokenResponse.RefreshToken, TimeSpan.FromDays(30));
            return Task.CompletedTask;
        }
    }
}
