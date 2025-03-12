using Domain.Interfaces.Api;

namespace Domain.Requests
{
    public class GetAthletesStatsApiRequest : IGetApiRequest
    {
        private readonly long _athleteId;

        public GetAthletesStatsApiRequest(long athleteId)
        {
            _athleteId = athleteId;
        }

        public string GetUrl => $"athletes/{_athleteId}/stats";
    }
}
