using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ServiceWorker;

public class Worker : BackgroundService
{
    private string _planPath = string.Empty;
    private string _mqHost = string.Empty;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _planPath = configuration["PlanPath"] ?? String.Empty;
        _mqHost = configuration["rabbitmqHost"] ?? "localhost";
    }


    // Receiver - Receive message from RabbitMQ
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Connecting to host: {_mqHost}", DateTimeOffset.Now);
        var factory = new ConnectionFactory { HostName = _mqHost }; // indsæt miljø varibel // noget alle 
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "booking",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);


        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var plan = JsonSerializer.Deserialize<Plan>(message);
            CreatePlan(plan);

        };


        channel.BasicConsume(queue: "booking",
                             autoAck: true,
                             consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    // Sender 
    // Save plan to csv file
    // Sender - Send message to RabbitMQ
    // Sender - Save plan string to CSV file
    public string CreatePlan(Plan plan)
    {
        try
        {
            if (plan == null)
            {
                return "Invalid plan object.";
            }

            // Create a unique filename based on a timestamp or GUID
            var uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";
            var filePath = Path.Combine(_planPath, uniqueFileName);

            // Format the plan data as a CSV line
            var csvLine = $"{plan.KundeNavn},{plan.OpsamlingsTid},{plan.Startsted},{plan.Endested}";

            // Create the "plan" folder if it doesn't exist
            // Directory.CreateDirectory("plan");

            // Write the CSV line to the new file
            File.WriteAllText(filePath, csvLine);

            _logger.LogInformation("Plan data saved to CSV: {filePath}", filePath);

            return csvLine;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving plan data to CSV.");
            return $"Error: {ex.Message}";
        }
    }


}
