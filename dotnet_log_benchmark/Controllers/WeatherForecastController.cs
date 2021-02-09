using System;
using System.Diagnostics;
using dotnet_log_benchmark.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_log_benchmark.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static ILogger<WeatherForecastController> _Logger;
        private static CalService _CalService;
        private static int LoopCount = 10000;
            private readonly Stopwatch _watch;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, CalService calService)
        // public WeatherForecastController(CalService calService)
        {
            // _Logger = logger;
            _CalService = calService;
            _watch = new Stopwatch();
        }

        [HttpGet]
        public Tuple<int, long> Get([FromQuery] long aLoop, [FromQuery] long bLoop)
        {
            var sum = 0;
            _watch.Start();
            
            for (var i = 0; i < aLoop; i++)
            {
                // for (var j = 0; j < bLoop; j++)
                // {
                //     sum += 1;
                // }
                sum += 1;
                _Logger.LogInformation($"done with loop:{i}");
            }
            _watch.Stop();
            // _CalService.DoMath();
            return new Tuple<int, long>(sum, _watch.ElapsedMilliseconds);
        }
    }

}
