using MediatR;

namespace Application.Queries.GetAthletesStats
{
    public class GetAthletesStatsQuery : IRequest<GetAthletesStatsQueryResult>
    {
        public long AthleteId { get; set; }
    }
}
