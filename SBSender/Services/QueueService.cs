using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace SBSender.Services;

public class QueueService : IQueueService
{
    private readonly IConfiguration _configuration;

    private static readonly JsonSerializerOptions _propertyCase = new()
    {
        PropertyNameCaseInsensitive = true
    };


    public QueueService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
    {
        await using var client = new ServiceBusClient(_configuration.GetConnectionString("AzureServiceBus"));
        ServiceBusSender sender = client.CreateSender(queueName);
        string messageBody = JsonSerializer.Serialize(serviceBusMessage, _propertyCase);
        await sender.SendMessageAsync(new ServiceBusMessage(messageBody));
        // var queueClient = new QueueClient(_configuration.GetConnectionString("AzureServiceBus"), queueName);
        // await queueClient.SendMessageAsync(messageBody);
    }
}