namespace Domain.Interfaces.Api
{
    public interface IPutApiRequest
    {
        object Data { get; }
        string PutUrl { get; }
    }
}
