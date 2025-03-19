using Hellang.Middleware.ProblemDetails;
using StrideStats.InnerApi.AppStart;
using System.Text.Json.Serialization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Adds JSON serialisation for enums
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // Handles reference loops by ignoring them
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // Add service registrations
        builder.Services.AddServiceRegistration(builder.Configuration);

        // Add ProblemDetails, further info in this article https://dev.to/andrewlocknet/handling-web-api-exceptions-with-problemdetails-middleware-47jb
        ProblemDetailsExtensions.AddProblemDetails(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler("/error/development-error");
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseProblemDetails();

        app.UseHealthChecks("/healthcheck");

        app.UseHttpsRedirection();

        app.UseCors("AllowSpecificOrigins");

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        app.Run();
    }
}