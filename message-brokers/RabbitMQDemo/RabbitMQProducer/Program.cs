using RabbitMQ.Client;
using System.Text;

Console.WriteLine("RabbitMQ Producer started!");

var factory = new ConnectionFactory() 
{ 
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    Port = 5672
};

try
{
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.QueueDeclare(
        queue: "test",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    while (true)
    {
        Console.Write("Enter message (or 'exit' to quit): ");
        var message = Console.ReadLine();

        if (message?.ToLower() == "exit")
            break;

        var body = Encoding.UTF8.GetBytes(message ?? string.Empty);
        
        channel.BasicPublish(
            exchange: "",
            routingKey: "test",
            basicProperties: null,
            body: body);
        
        Console.WriteLine($"[Sent] {message}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}