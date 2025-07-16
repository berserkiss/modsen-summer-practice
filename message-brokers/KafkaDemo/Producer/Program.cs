using Confluent.Kafka;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    Acks = Acks.All
};

using var producer = new ProducerBuilder<Null, string>(config).Build();

Console.WriteLine("Введите сообщения (exit для выхода):");

while (true)
{
    var message = Console.ReadLine();
    if (message == "exit") break;
    
    var result = await producer.ProduceAsync("test-topic", 
        new Message<Null, string> { Value = message });
    
    Console.WriteLine($"Отправлено: {result.Value}");
}