using Common.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RegistrationService.Services;

public class RegistrationService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "user-registrations";
    private const string DlxExchange = "dlx-exchange";

    public RegistrationService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Настройка DLX
        _channel.ExchangeDeclare(DlxExchange, ExchangeType.Fanout, durable: true);
        
        // Параметры основной очереди
        var queueArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", DlxExchange }
        };

        _channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs);
    }

    public void RegisterUser(string email)
    {
        var message = new UserRegisteredEvent(
            UserId: Guid.NewGuid(),
            Email: email,
            Timestamp: DateTime.UtcNow);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish("", QueueName, null, body);
        
        Console.WriteLine($" [x] Sent registration: {email}");
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        GC.SuppressFinalize(this);
    }
}