namespace Domain.Interfaces.Api
{
    public interface IPostApiRequest
    {
        string PostUrl { get;  }
        object Data { get; }
    }
}
