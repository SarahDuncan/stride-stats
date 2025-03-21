using Domain.Interfaces.Api;

namespace Domain.Requests
{
    public class UpdateAthleteApiRequest : IPutApiRequest
    {
        public UpdateAthleteApiRequest(float weight)
        {
            Data = new UpdateAthleteApiRequestData
            {
                Weight = weight
            };
        }

        public object Data { get; }

        public string PutUrl => "athlete";
    }

    public class UpdateAthleteApiRequestData
    {
        public float Weight { get; set; }
    }
}
