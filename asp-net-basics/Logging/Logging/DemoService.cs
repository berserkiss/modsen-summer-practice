namespace Logging;
using Microsoft.Extensions.Logging;

public interface IDemoService
{
    void DoWork();
}

public class DemoService : IDemoService
{
    private readonly ILogger<DemoService> _logger;

    public DemoService(ILogger<DemoService> logger)
    {
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.LogInformation("Starting work in DemoService");
        
        // Логирование перед исключением
        _logger.LogWarning("About to throw an exception");
        
        throw new Exception("This is a demo exception from DemoService");
    }
}