using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using SBShared.Models;

const string CONNECTIONSTRING = "CONNECTION STRING AZURE SERVICE BUS";
const string QUEUENAME = "personqueue";

var _propertyCase = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
};

async Task ReceivedMessage()
{
    ServiceBusClient client = new ServiceBusClient(CONNECTIONSTRING);

    ServiceBusProcessor processor = client.CreateProcessor(QUEUENAME, new ServiceBusProcessorOptions
    {
        MaxConcurrentCalls = 1,
        AutoCompleteMessages = false
    });

    processor.ProcessMessageAsync += ProcessMessageAsync;
    processor.ProcessErrorAsync += ExceptionReceivedHandler;

    await processor.StartProcessingAsync();

    Console.ReadLine();

    await processor.CloseAsync();
}

async Task ProcessMessageAsync(ProcessMessageEventArgs args)
{
    var jsonString = Encoding.UTF8.GetString(args.Message.Body);
    PersonModel? person = JsonSerializer.Deserialize<PersonModel>(jsonString, _propertyCase);
    Console.WriteLine($"Person Received: {person?.FirstName} {person?.LastName}");

    await args.CompleteMessageAsync(args.Message);
}

async Task ExceptionReceivedHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine($"Message handler exception: {args.Exception}");
    await Task.CompletedTask;
}

await ReceivedMessage();