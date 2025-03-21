namespace Domain.Interfaces.Api
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<TResponse> Put<TResponse>(IPutApiRequest request);
        Task<TResponse?> Post<TResponse>(IPostApiRequest request);
    }
}
