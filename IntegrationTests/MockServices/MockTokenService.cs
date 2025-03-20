using Domain.Interfaces.Cache;
using Domain.Responses;

namespace IntegrationTests.MockServices
{
    public class MockTokenService : ITokenService
    {
        public Task<string> GetRefreshTokenAsync()
        {
            return Task.FromResult("mock_refresh_token");
        }

        public Task<string> GetTokenAsync()
        {
            return Task.FromResult("mock_access_token");
        }

        public Task StoreTokenAsync(StravaTokenResponse tokenResponse)
        {
            return Task.CompletedTask;
        }
    }
}
