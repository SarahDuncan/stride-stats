using Application.Queries.GetAthlete;
using Application.Queries.GetAthlete.ResultModels;
using AutoMapper;
using Domain.Responses;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetAthleteApiResponse, GetAthleteQueryResult>();
            CreateMap<Domain.Responses.Models.SummaryClub, SummaryClub>();
            CreateMap<Domain.Responses.Models.SummaryGear, SummaryGear>();
            CreateMap<Domain.Responses.Models.ActivityTypeEnum, ActivityTypeEnum>();
        }
    }
}
