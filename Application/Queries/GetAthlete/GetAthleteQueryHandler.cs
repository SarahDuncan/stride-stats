using AutoMapper;
using Domain.Interfaces.Api;
using Domain.Requests;
using Domain.Responses;
using MediatR;

namespace Application.Queries.GetAthlete
{
    public class GetAthleteQueryHandler : IRequestHandler<GetAthleteQuery, GetAthleteQueryResult>
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        public GetAthleteQueryHandler(IApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<GetAthleteQueryResult> Handle(GetAthleteQuery request, CancellationToken cancellationToken)
        {
            var apiResult = await _apiClient.Get<GetAthleteApiResponse>(new GetAthleteApiRequest());
            return _mapper.Map<GetAthleteQueryResult>(apiResult);
        }
    }
}
