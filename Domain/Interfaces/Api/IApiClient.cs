namespace Domain.Interfaces.Api
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
    }
}
