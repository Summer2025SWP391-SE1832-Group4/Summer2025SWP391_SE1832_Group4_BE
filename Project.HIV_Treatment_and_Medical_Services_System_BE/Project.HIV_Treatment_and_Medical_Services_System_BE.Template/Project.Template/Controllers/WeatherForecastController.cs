using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IMediator mediator, IMapper mapper, ILogger<WeatherForecastController> logger) : ControllerBase(mediator, mapper)
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })];
    }
}
