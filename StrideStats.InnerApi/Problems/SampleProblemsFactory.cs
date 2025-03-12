using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace StrideStats.InnerApi.Problems
{
    public class SampleProblemsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;

        public SampleProblemsFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value;
        }

        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            var problem = new ProblemDetails
            {
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
                Status = statusCode
            };

            if (_options is not null)
            {
                if (_options.ClientErrorMapping.TryGetValue(statusCode.Value, out var clientErrorData))
                {
                    problem.Title ??= clientErrorData.Title;
                    problem.Type ??= clientErrorData.Link;
                }
            }

            var traceId = httpContext?.TraceIdentifier;

            if (traceId is not null)
            {
                problem.Extensions["traceId"] = traceId;
            }

            return problem;
        }

        // Not needed right now
        //public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        //{
        //    return new ValidationProblemDetails(modelStateDictionary)
        //    {
        //        Title = title,
        //        Type = type,
        //        Detail = detail,
        //        Instance = instance,
        //        Status = statusCode
        //    };
        //}
    }
}
