using Application.Queries.Common;

namespace Application.Queries.GetAthletesStats
{
    public class GetAthletesStatsQueryResult
    {
        public double BiggestRideDistance { get; set; }
        public double BiggestClimbElevationGain { get; set; }
        public TotalsResult RecentRideTotals { get; set; }
        public TotalsResult AllRideTotals { get; set; }
        public TotalsResult RecentRunTotals { get; set; }
        public TotalsResult AllRunTotals { get; set; }
        public TotalsResult RecentSwimTotals { get; set; }
        public TotalsResult AllSwimTotals { get; set; }
        public TotalsResult YtdRideTotals { get; set; }
        public TotalsResult YtdRunTotals { get; set; }
        public TotalsResult YtdSwimTotals { get; set; }
    }
}
