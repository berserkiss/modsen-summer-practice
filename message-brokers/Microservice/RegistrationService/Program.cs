using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

Console.WriteLine("RegistrationService starting...");

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Настройка DLQ
const string dlxExchange = "dlx-exchange";
channel.ExchangeDeclare(dlxExchange, ExchangeType.Fanout, durable: true);
channel.QueueDeclare("dead-letter-queue", 
    durable: true, 
    exclusive: false, 
    autoDelete: false);
channel.QueueBind("dead-letter-queue", dlxExchange, "");

// Параметры основной очереди
var queueArgs = new Dictionary<string, object>
{
    { "x-dead-letter-exchange", dlxExchange }
};

// Создание основной очереди
channel.QueueDeclare(queue: "user-registrations",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: queueArgs);

// Пример события регистрации
var registrationEvent = new 
{
    UserId = Guid.NewGuid(),
    Email = "user@example.com",
    RegistrationDate = DateTime.UtcNow
};

var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(registrationEvent));

// Отправка сообщения
channel.BasicPublish(
    exchange: "",
    routingKey: "user-registrations",
    basicProperties: null,
    body: body);

Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sent registration event for {registrationEvent.Email}");
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();