using Application.Commands.UpdateAthlete;
using Application.Queries.Common;
using Application.Queries.GetAthlete;
using Application.Queries.GetAthletesStats;
using AutoMapper;
using Domain.Responses;
using Domain.Responses.Models;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetAthleteApiResponse, GetAthleteQueryResult>();
            CreateMap<GetAthletesStatsApiResponse, GetAthletesStatsQueryResult>();
            CreateMap<Totals, TotalsResult>();
            CreateMap<UpdateAthleteApiResponse, UpdateAthleteCommandResult>();
        }
    }
}
