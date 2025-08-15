using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Producer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IProducer<Null, string> _producer;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? "localhost:9093",
            SecurityProtocol = SecurityProtocol.Plaintext
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int counter = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            var value = $"Message {counter++}";
            var deliveryResult = await _producer.ProduceAsync("test-topic", new Message<Null, string> { Value = value });
            _logger.LogInformation($"Produced: {value} to {deliveryResult.TopicPartitionOffset}");
            await Task.Delay(5000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _producer?.Dispose();
        base.Dispose();
    }
}