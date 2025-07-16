using Common.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmailService.Services;

public class EmailService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "user-registrations";
    private const string DlxExchange = "dlx-exchange";
    private readonly HashSet<Guid> _processedIds = new();

    public EmailService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Должно точно соответствовать параметрам из Producer!
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

    public void StartListening()
    {
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += (model, ea) =>
        {
            try
            {
                var message = JsonSerializer.Deserialize<UserRegisteredEvent>(
                    Encoding.UTF8.GetString(ea.Body.ToArray()));

                if (message == null || _processedIds.Contains(message.UserId))
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                if (message.Email.Contains("fail"))
                {
                    throw new Exception("Simulated processing error");
                }

                Console.WriteLine($"Sending email to: {message.Email}");
                Console.WriteLine($"User ID: {message.UserId}");
                Console.WriteLine($"Registered at: {message.Timestamp:g}");
                Console.WriteLine(new string('-', 30));
                
                _processedIds.Add(message.UserId);
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(QueueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        GC.SuppressFinalize(this);
    }
}