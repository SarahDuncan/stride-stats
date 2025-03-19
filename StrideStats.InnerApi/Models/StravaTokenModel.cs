using System.Text.Json.Serialization;

namespace StrideStats.InnerApi.Models
{
    public class StravaTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("expires_at")]
        public int ExpiresAt { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
