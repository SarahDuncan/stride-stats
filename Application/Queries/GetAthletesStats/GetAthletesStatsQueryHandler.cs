using AutoMapper;
using Domain.Interfaces.Api;
using Domain.Requests;
using Domain.Responses;
using MediatR;

namespace Application.Queries.GetAthletesStats
{
    public class GetAthletesStatsQueryHandler : IRequestHandler<GetAthletesStatsQuery, GetAthletesStatsQueryResult>
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        public GetAthletesStatsQueryHandler(IApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<GetAthletesStatsQueryResult> Handle(GetAthletesStatsQuery request, CancellationToken cancellationToken)
        {
            var apiResult = await _apiClient.Get<GetAthletesStatsApiResponse>(new GetAthletesStatsApiRequest(request.AthleteId));
            return _mapper.Map<GetAthletesStatsQueryResult>(apiResult);
        }
    }
}
