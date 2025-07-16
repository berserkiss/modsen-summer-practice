using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("RabbitMQ Consumer started!");

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

    var consumer = new EventingBasicConsumer(channel);
    
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"[Received] {message}");
        channel.BasicAck(ea.DeliveryTag, false);
    };

    channel.BasicConsume(
        queue: "test-queue",
        autoAck: false,
        consumer: consumer);

    Console.WriteLine("Press Enter to exit.");
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}