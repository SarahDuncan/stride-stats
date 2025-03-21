using AutoMapper;
using Domain.Interfaces.Api;
using Domain.Requests;
using Domain.Responses;
using MediatR;

namespace Application.Commands.UpdateAthlete
{
    public class UpdateAthleteCommandHandler : IRequestHandler<UpdateAthleteCommand, UpdateAthleteCommandResult>
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        public UpdateAthleteCommandHandler(IApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<UpdateAthleteCommandResult> Handle(UpdateAthleteCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Put<UpdateAthleteApiResponse>(new UpdateAthleteApiRequest(request.Weight));
            return _mapper.Map<UpdateAthleteCommandResult>(response);
        }
    }
}
