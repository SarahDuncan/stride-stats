using Domain.Interfaces.Api;
using Domain.Interfaces.Cache;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Application.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public ApiClient(HttpClient httpClient, IConfiguration configuration, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
            _tokenService = tokenService;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
            HttpResponseMessage response;

            try
            {
                var accessToken = await _tokenService.GetTokenAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new UnauthorizedAccessException("Access token not found.");
                }
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException exception)
            {
                throw new Exception($"HTTP request failed, exception: {exception.Message}", exception);
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                return JsonSerializer.Deserialize<TResponse>(json);
            }
            catch (JsonException exception)
            {
                throw new Exception($"JSON deserialization failed, exception: {exception.Message}", exception);
            }
               
        }
    }
}
