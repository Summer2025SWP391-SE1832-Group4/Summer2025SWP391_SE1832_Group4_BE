using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application;
using Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.Behaviors;
using Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.Middlewares;
using HIV_Treatment_and_Medical_Services_System_BE.Project.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("sqlConnection"));
});

// Add Application Dependency injection services.
builder.Services.AddApplicationServices();

// Add Exception Handler.
//builder.Services.AddExceptionHandler<ErrorExceptionHandling>();

// Add Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controller.
builder.Services.AddControllers();

// Add Pipeline Behavior for validation flow.
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler("/Error");

app.UseMiddleware<CustomMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
