using Confluent.Kafka;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "test-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe("test-topic");

Console.WriteLine("Ожидание сообщений");

try
{
    while (true)
    {
        var result = consumer.Consume();
        Console.WriteLine($"Получено: {result.Message.Value}");
    }
}
catch (OperationCanceledException)
{
    consumer.Close();
}