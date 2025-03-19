using Application.Api;
using Application.Mappings;
using Application.Queries.GetAthlete;
using Domain.Interfaces.Api;
using Domain.Interfaces.Cache;
using Infrastructure.Cache;
using System.Reflection;

namespace StrideStats.InnerApi.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add HttpClient
            services.AddHttpClient<IApiClient, ApiClient>();

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
                    Version = "v1",
                    Description = "Consumes the Strava V3 API"
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(xmlPath);
            });

            // Add HealthChecks
            services.AddHealthChecks();

            // Add Memory Cache
            services.AddMemoryCache();
            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
