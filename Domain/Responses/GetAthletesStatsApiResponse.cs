using Domain.Responses.Models;
using System.Text.Json.Serialization;

namespace Domain.Responses
{
    public class GetAthletesStatsApiResponse
    {
        [JsonPropertyName("biggest_ride_distance")]
        public double BiggestRideDistance { get; set; }
        [JsonPropertyName("biggest_climb_elevation_gain")]
        public double BiggestClimbElevationGain { get; set; }
        [JsonPropertyName("recent_ride_totals")]
        public Totals? RecentRideTotals { get; set; }
        [JsonPropertyName("all_ride_totals")]
        public Totals? AllRideTotals { get; set; }
        [JsonPropertyName("recent_run_details")]
        public Totals? RecentRunTotals { get; set; }
        [JsonPropertyName("all_run_totals")]
        public Totals? AllRunTotals { get; set; }
        [JsonPropertyName("recent_swim_totals")]
        public Totals? RecentSwimTotals { get; set; }
        [JsonPropertyName("all_swim_totals")]
        public Totals? AllSwimTotals { get; set; }
        [JsonPropertyName("ytd_ride_totals")]
        public Totals? YtdRideTotals { get; set; }
        [JsonPropertyName("ytd_run_totals")]
        public Totals? YtdRunTotals { get; set; }
        [JsonPropertyName("ytd_swim_totals")]
        public Totals? YtdSwimTotals { get; set; }
    }
}
