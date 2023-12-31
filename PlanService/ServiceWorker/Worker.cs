using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceWorker.Models;



namespace ServiceWorker;

public class Worker : BackgroundService
{
    private string _planPath = string.Empty;
    private string _mqHost = string.Empty;
    private readonly ILogger<Worker> _logger;
    private readonly PlanRepository _planRepository;


    public Worker(ILogger<Worker> logger, IConfiguration configuration, PlanRepository planRepository)
    {
        _logger = logger;
        _planPath = configuration["PlanPath"] ?? String.Empty;
        _mqHost = configuration["rabbitmqHost"] ?? "localhost";
        _logger.LogInformation($"Connecting to host: {_mqHost}");
        _planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
    }


    // Receiver - Receive message from RabbitMQ
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Connecting to host: {_mqHost}", DateTimeOffset.Now);
        var factory = new ConnectionFactory { HostName = _mqHost }; // indsæt miljø varibel // noget alle 
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declare the "booking" queue
        channel.QueueDeclare(queue: "booking",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);


        // Declare the "maintenanceVisits" queue
        channel.QueueDeclare(queue: "maintenanceVisits",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

        // Set up consumer for the "booking" queue
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var plan = System.Text.Json.JsonSerializer.Deserialize<Plan>(message);
            _logger.LogInformation($"Received plan: {plan}");
            _planRepository.InsertPlan(plan);
        };

        // Set up consumer for the "maintenanceVisits" queue
        var maintenanceVisitsConsumer = new EventingBasicConsumer(channel);
        maintenanceVisitsConsumer.Received += (model, ea) =>
        {

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation(message);
            try
            {
                // var maintenanceVisit = JsonSerializer.Deserialize<MaintenanceVisit>(message);
                var maintenanceVisit = JsonConvert.DeserializeObject<MaintenanceVisit>(message);


                _logger.LogInformation($"Received maintenance visit: {maintenanceVisit.Description}");
                _planRepository.InsertMaintenanceVisit(maintenanceVisit);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        };


        // Start consuming messages from the "booking" queue
        channel.BasicConsume(queue: "booking",
                             autoAck: true,
                             consumer: consumer);

        // Start consuming messages from the "maintenanceVisits" queue
        channel.BasicConsume(queue: "maintenanceVisits",
                         autoAck: true,
                         consumer: maintenanceVisitsConsumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }



    // Sender 
    // Save plan to csv file
    // Sender - Send message to RabbitMQ
    // Sender - Save plan string to CSV file
    /* public string CreatePlan(Plan plan)
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

     */

}
