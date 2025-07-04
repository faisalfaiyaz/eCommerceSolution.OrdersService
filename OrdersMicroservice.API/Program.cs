using BusinessLogicLayer;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using OrdersMicroservice.API.Middlewares;

namespace OrdersMicroservice.API;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a builder to configure and build the web application
        var builder = WebApplication.CreateBuilder(args);

        // Register controllers to the service container
        builder.Services.AddControllers();

        // Register custom business logic layer services (extension method defined in API project)
        builder.Services.AddBusinessLogicLayer(builder.Configuration);

        // Register custom data access layer services, like MongoDB repositories
        builder.Services.AddDataAccessLayer(builder.Configuration);

        // Enable automatic FluentValidation model validation
        builder.Services.AddFluentValidationAutoValidation();

        // Add support for minimal API documentation (endpoint explorer for Swagger)
        builder.Services.AddEndpointsApiExplorer();

        // Register Swagger generator for API documentation
        builder.Services.AddSwaggerGen();

        // Configure CORS (Cross-Origin Resource Sharing) policy
        builder.Services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                // Allow requests from Angular frontend running at localhost:4200
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Build the application from the configured builder
        var app = builder.Build();

        // Use custom middleware for centralized exception handling (defined by us)
        app.UseExceptionHandlingMiddleware();

        // Enable routing middleware (processes routing of incoming requests)
        app.UseRouting();

        // Enable CORS with the configured policy
        app.UseCors();

        // Enable Swagger middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger();

        // Enable Swagger UI (HTML UI for testing APIs)
        app.UseSwaggerUI();

        // Redirect HTTP requests to HTTPS
        app.UseHttpsRedirection();

        // Enable authentication middleware 
        app.UseAuthentication();

        // Enable authorization middleware (checks if user has access to resources)
        app.UseAuthorization();

        // Map controller routes (attribute-based routing)
        app.MapControllers();

        // Run the application (start listening to HTTP requests)
        app.Run();

    }
}
