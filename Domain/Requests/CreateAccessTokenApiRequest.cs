using Domain.Interfaces.Api;

namespace Domain.Requests
{
    public class CreateAccessTokenApiRequest : IPostApiRequest
    {
        public CreateAccessTokenApiRequest(string postUrl, FormUrlEncodedContent requestData)
        {
            PostUrl = postUrl;
            Data = new CreateAccessTokenApiResponseData() { Request = requestData };
        }

        public string PostUrl { get; }

        public object Data { get; }
    }

    public class CreateAccessTokenApiResponseData()
    {
        public FormUrlEncodedContent Request { get; set; }
    }
}
