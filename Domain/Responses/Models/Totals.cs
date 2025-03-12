using System.Text.Json.Serialization;

namespace Domain.Responses.Models
{
    public class Totals
    {
        [JsonPropertyName("count")]
        public double Count { get; set; }
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
        [JsonPropertyName("moving_time")]
        public double MovingTime { get; set; }
        [JsonPropertyName("elapsed_time")]
        public double ElapsedTime { get; set; }
        [JsonPropertyName("elevation_gain")]
        public double ElevationGain { get; set; }
        [JsonPropertyName("achievement_count")]
        public double AchievementCount { get; set; }
    }
}
