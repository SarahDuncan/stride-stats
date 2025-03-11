using Domain.Interfaces.Api;

namespace Domain.Requests
{
    public class GetAthleteApiRequest : IGetApiRequest
    {
        public string GetUrl => "athlete";
    }
}
