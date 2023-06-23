using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthPolicies.Controllers
{
    [Authorize(AuthenticationSchemes = Startup.Schemes.Bar)]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public WeatherForecastResponse Get()
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return new WeatherForecastResponse()
            {
                ExecutedAuthSchemes = User.Identities.Select(i => i.AuthenticationType).ToArray(),
                WeatherForecasts = forecasts
            };
        }
    }

    public class WeatherForecastResponse
    {
        public string[] ExecutedAuthSchemes { get; set; }
        public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
    }
}
