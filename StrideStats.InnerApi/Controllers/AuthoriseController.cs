using AutoMapper;
using Domain.Interfaces.Cache;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using StrideStats.InnerApi.Models;
using System.Text.Json;

namespace StrideStats.InnerApi.Controllers
{
    /// <summary>
    /// Controller for handling authorisation.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoriseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthoriseController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="httpClient">The HttpClient instance.</param>
        public AuthoriseController(IConfiguration configuration, HttpClient httpClient, ITokenService tokenService, IMapper mapper)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Initiates the OAuth authorization process by redirecting to Strava's authorization URL.
        /// </summary>
        /// <returns>A redirect response to the Strava authorization URL.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var clientId = _configuration["ApiSettings:ClientId"];
            var redirectUri = _configuration["ApiSettings:RedirectUri"];

            var authUrl = _configuration["ApiSettings:AuthUrl"];
            authUrl = authUrl?.Replace("{clientId}", clientId);
            authUrl = authUrl?.Replace("{redirectUri}", redirectUri);

            return Redirect(authUrl);
        }

        /// <summary>
        /// Handles the callback from Strava after authorization, exchanges the authorization code for an access token.
        /// </summary>
        /// <param name="code">The authorization code received from Strava.</param>
        /// <returns>An HTTP response indicating the result of the token exchange.</returns>
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Auth code missing");
            }

            var tokenUrl = "https://www.strava.com/oauth/token";

            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string?, string?>("client_id", _configuration["ApiSettings:ClientId"]),
                new KeyValuePair<string?, string?>("client_secret", _configuration["ApiSettings:ClientSecret"]),
                new KeyValuePair<string?, string?>("code", code),
                new KeyValuePair<string?, string?>("grant_type", "authorization_code")
            });

            var response = await _httpClient.PostAsync(_configuration["ApiSettings:TokenUrl"], tokenRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Error retrieving auth token: {content}");
            }

            var tokenResponse = JsonSerializer.Deserialize<StravaTokenModel>(content);

            // todo: move this mapping
            await _tokenService.StoreTokenAsync(new StravaTokenResponse
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresAt = tokenResponse.ExpiresAt,
                ExpiresIn = tokenResponse.ExpiresIn
            });

            return Ok();
        }
    }
}
