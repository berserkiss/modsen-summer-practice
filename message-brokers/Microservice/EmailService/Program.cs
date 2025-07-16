using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

Console.WriteLine("EmailService starting...");


using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

var logger = loggerFactory.CreateLogger<Program>();

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


const string dlxExchange = "dlx-exchange";
var queueArgs = new Dictionary<string, object>
{
    { "x-dead-letter-exchange", dlxExchange }
};

// Объявление очереди
channel.QueueDeclare(queue: "user-registrations",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: queueArgs);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var processedIds = new HashSet<Guid>();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    try
    {
        var body = ea.Body.ToArray();
        var message = JsonSerializer.Deserialize<RegistrationMessage>(Encoding.UTF8.GetString(body));

        if (message == null)
        {
            throw new ArgumentNullException(nameof(message), "Invalid message format");
        }

        // Проверка идемпотентности
        if (processedIds.Contains(message.UserId))
        {
            channel.BasicAck(ea.DeliveryTag, multiple: false);
            logger.LogInformation("Duplicate message for user {UserId}", message.UserId);
            return;
        }


        logger.LogInformation("Sending welcome email to {Email} (UserID: {UserId})", 
            message.Email, message.UserId);


        processedIds.Add(message.UserId);
        channel.BasicAck(ea.DeliveryTag, multiple: false);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing message");
        channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
    }
};

channel.BasicConsume(
    queue: "user-registrations",
    autoAck: false,
    consumer: consumer);

logger.LogInformation("Ready to process messages. Press Ctrl+C to exit.");
Console.CancelKeyPress += (_, _) => Environment.Exit(0);
while (true) Thread.Sleep(1000);

public class RegistrationMessage
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
}