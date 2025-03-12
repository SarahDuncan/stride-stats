using Application.Api;
using Application.Mappings;
using Application.Queries.GetAthlete;
using Domain.Interfaces.Api;
using System.Net.Http.Headers;
using System.Reflection;

namespace StrideStats.InnerApi.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add HttpClient
            services.AddHttpClient<IApiClient, ApiClient>()
                .AddHttpMessageHandler(() => new AuthorizationHandler(configuration));

            // Add MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAthleteQueryHandler).Assembly));

            // Add AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Add Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "StrideStats API", 
                    Version = "v1" ,
                    Description = "Consumes the Strava V3 API"
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(xmlPath);
            });
        }

        public class AuthorizationHandler : DelegatingHandler
        {
            private readonly IConfiguration _configuration;

            public AuthorizationHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var accessToken = await GetAccessTokenAsync();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return await base.SendAsync(request, cancellationToken);
            }

            private Task<string> GetAccessTokenAsync()
            {
                return Task.FromResult(_configuration["ApiSettings:AccessToken"]);
            }
        }
    }
}
