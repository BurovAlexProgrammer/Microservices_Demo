using Confluent.Kafka;

namespace Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ConsumerConfig _config;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _config = new ConsumerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? "localhost:9093",
            GroupId = "csharp-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe("test-topic");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Consumed: {cr.Value} at {cr.TopicPartitionOffset}");
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer shutting down...");
                consumer.Close();
            }
        });
    }
}