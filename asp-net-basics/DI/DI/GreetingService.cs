namespace DI;
public interface ITransientGreetingService : IGreetingService { }
public interface IScopedGreetingService : IGreetingService { }
public interface ISingletonGreetingService : IGreetingService { }

public class GreetingService : 
    ITransientGreetingService, 
    IScopedGreetingService, 
    ISingletonGreetingService
{
    private readonly ILogger<GreetingService> _logger;
    private readonly Guid _instanceId = Guid.NewGuid();

    public GreetingService(ILogger<GreetingService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Создан GreetingService (ID: {Id})", _instanceId);
    }

    public string Greet(string name)
    {
        _logger.LogInformation("Вызван Greet (ID: {Id}, Name: {Name})", _instanceId, name);
        return $"Привет, {name}!";
    }
}