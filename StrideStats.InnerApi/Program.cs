using StrideStats.InnerApi.AppStart;
using System.Text.Json.Serialization;
using Application.Mappings;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(app =>
{
    app.SwaggerEndpoint("/swagger/v1/swagger.json", "Stride Stats API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
