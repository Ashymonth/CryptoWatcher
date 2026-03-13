using System.Text.Json;
using Confluent.Kafka;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.WalletIngestion.Infrastructure.Integrations.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Kafka;

public class BlockchainTransactionTransactionsConsumer : BackgroundService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly KafkaConfig _config;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BlockchainTransactionTransactionsConsumer> _logger;

    public BlockchainTransactionTransactionsConsumer(KafkaConfig config, IServiceScopeFactory scopeFactory,
        ILogger<BlockchainTransactionTransactionsConsumer> logger)
    {
        _config = config;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig
        {
            GroupId = "uniswap",
            EnableAutoCommit = false,
            BootstrapServers = _config.Host.ToString()
        }).Build();
        
        
        consumer.Assign(new TopicPartitionOffset(_config.RawTransactionsTopic, 0, new Offset(0)));
        consumer.Subscribe(_config.RawTransactionsTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var batch = ConsumeBatch(consumer, 50);

                if (batch.Count == 0)
                {
                    continue;
                }

                using var scope = _scopeFactory.CreateScope();
                
                var transactions = batch
                    .Where(result => result.Message.Value is not null)
                    .Select(result => JsonSerializer.Deserialize<BlockchainTransaction>(result.Message.Value,
                        JsonSerializerOptions)!);

                var consumerService = scope.ServiceProvider.GetRequiredService<IWalletTransactionConsumer>();
                await consumerService.ConsumeTransactionsAsync(transactions, stoppingToken);

                consumer.Commit(batch.Last());
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, "Kafka consume error");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Processing error");
            }
        }
    }

    private static List<ConsumeResult<string, string>> ConsumeBatch(
        IConsumer<string, string> consumer,
        int batchSize)
    {
        var batch = new List<ConsumeResult<string, string>>(batchSize);

        for (var i = 0; i < batchSize; i++)
        {
            var result = consumer.Consume(TimeSpan.FromMilliseconds(100));

            if (result is null)
            {
                break;
            }

            batch.Add(result);
        }

        return batch;
    }
}