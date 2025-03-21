using MediatR;

namespace Application.Commands.UpdateAthlete
{
    public class UpdateAthleteCommand : IRequest<UpdateAthleteCommandResult>
    {
        public float Weight { get; set; }

        public UpdateAthleteCommand(float weight)
        {
            Weight = weight;
        }
    }
}
