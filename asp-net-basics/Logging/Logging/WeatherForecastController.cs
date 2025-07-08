namespace Logging;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IDemoService _demoService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        IDemoService demoService,
        ILogger<WeatherForecastController> logger)
    {
        _demoService = demoService;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        _logger.LogInformation("Getting weather forecast");
        
        try
        {
            _demoService.DoWork();
            return Ok(new { Message = "Success" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in WeatherForecastController");
            throw; 
        }
    }
}